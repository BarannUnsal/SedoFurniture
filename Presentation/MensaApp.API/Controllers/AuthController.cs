using MensaApp.API.Models;
using MensaApp.Domain.Entities.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using Serilog;

namespace MensaApp.API.Controllers
{
    [AllowAnonymous]
    public class AuthController : Controller
    {
        readonly SignInManager<AppUser> _signInManager;
        readonly UserManager<AppUser> _userManager;
        private readonly IToastNotification _toastNotification;
        private ILogger<AuthController> _logger;

        public AuthController(SignInManager<AppUser> signInManager, IToastNotification toastNotification, UserManager<AppUser> userManager, ILogger<AuthController> logger)
        {
            _signInManager = signInManager;
            _toastNotification = toastNotification;
            _userManager = userManager;
            _logger = logger;
        }

        public IActionResult Index()
        {

            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Index(UserViewModel userViewModel, string? returnUrl = "/")
        {
            try
            {
                if (ModelState.IsValid)
                {
                    try
                    {

                        AppUser appUser = await _userManager.FindByNameAsync(userViewModel.Username);
                        try
                        {
                            if (appUser != null)
                            {
                                try
                                {
                                    if (await _userManager.IsLockedOutAsync(appUser))
                                    {
                                        ModelState.AddModelError("", "Hesabınız bir süreliğine kitlenmiştir. Lütfen sonra tekrar deneyin");

                                        return View(userViewModel);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    _logger.LogInformation("Error Login -> Hesap kitlendi!", ex);
                                }

                                await _signInManager.SignOutAsync();
                                var result = await _signInManager.PasswordSignInAsync(appUser, userViewModel.Password, userViewModel.RememberMe, false);

                                if (result.Succeeded)
                                {

                                    if (!string.IsNullOrEmpty(returnUrl))
                                    {
                                        if (Url.IsLocalUrl(returnUrl))
                                        {
                                            return Redirect(returnUrl);
                                        }
                                        else
                                        {
                                            return Redirect("/");
                                        }
                                    }

                                    await _userManager.ResetAccessFailedCountAsync(appUser);

                                    _toastNotification.AddSuccessToastMessage("Giriş başarılı! Hoş Geldiniz", new NotyOptions()
                                    {
                                        Theme = "metroui"
                                    });

                                    _logger.LogInformation("Login işlemi -> Kullanıcı sisteme giriş yaptı");

                                    return RedirectToAction("Dashboard", "AdminDashboard", new { area = "admin" });
                                }
                                else
                                {
                                    try
                                    {

                                        await _userManager.AccessFailedAsync(appUser);


                                        int fail = await _userManager.GetAccessFailedCountAsync(appUser);

                                        ModelState.AddModelError("", $"{fail} kez başarısız giriş");

                                        if (fail == 3)
                                        {
                                            await _userManager.SetLockoutEndDateAsync(appUser, new System.DateTimeOffset(DateTime.Now.AddMinutes(20)));
                                            ModelState.AddModelError("", "Hesabınız geçici süreliğine kitlenmiştir. Lütfen daha sonra tekrar deneyin");
                                        }
                                        else
                                        {
                                            ModelState.AddModelError("", "Kullanıcı adı veya şifre yanlış");
                                        }
                                        return RedirectToAction("Index");
                                    }
                                    catch (Exception ex)
                                    {
                                        _logger.LogError("Error Login -> Giriş başarısız!", ex);
                                    }
                                }
                            }
                            else
                            {
                                ModelState.AddModelError("", "Kullanıcı bulunamadı");
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError("Error Login -> Kullanıcı bulunamadı!", ex);
                        }

                    }
                    catch (Exception ex)
                    {
                        _logger.LogError("Error Login -> Model State hatası!", ex);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Login -> Beklenmeyen hata!", ex);
            }
            return View(userViewModel);
        }

        public async Task LogOut()
        {
            try
            {
                await _signInManager.SignOutAsync();
                Log.Information("Kullanıcı çıkış yaptı");
                _toastNotification.AddErrorToastMessage("Çıkış başarılı");
            }
            catch (Exception ex)
            {
                Log.Error("Çıkış hatalı", ex);
            }
        }
    }
}
