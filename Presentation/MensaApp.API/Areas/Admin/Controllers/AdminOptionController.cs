using Mapster;
using MensaApp.API.Areas.Admin.Models.Admin;
using MensaApp.Domain.Entities.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;

namespace MensaApp.API.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class AdminOptionController : Controller
    {
        readonly SignInManager<AppUser> _signInManager;
        readonly UserManager<AppUser> _userManager;
        private readonly IToastNotification _toastNotification;
        readonly ILogger<AdminOptionController> _logger;
        public AdminOptionController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, IToastNotification toastNotification, ILogger<AdminOptionController> logger)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _toastNotification = toastNotification;
            _logger = logger;
        }

        public async Task<IActionResult> Settings()
        {
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            AdminViewModel model = user.Adapt<AdminViewModel>();

            return View(model);
        }

        public IActionResult UserEdit()
        {
            AppUser user = _userManager.FindByNameAsync(User.Identity.Name).Result;

            AdminViewModel adminView = user.Adapt<AdminViewModel>();

            return View(adminView);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> UserEdit(AdminViewModel model)
        {
            if (ModelState.IsValid)
            {

                AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);

                string phone = await _userManager.GetPhoneNumberAsync(user);

                if (phone != model.PhoneNumber)
                {
                    if (_userManager.Users.Any(x => x.PhoneNumber == model.PhoneNumber))
                    {
                        ModelState.AddModelError("", "Telefon numarası kayıtlıdır.");
                        return View(model);
                    }
                }

                user.UserName = model.UserName;
                user.Email = model.Email;
                user.NameSurname = model.NameSurname;
                user.PhoneNumber = model.PhoneNumber;

                IdentityResult result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    await _userManager.UpdateSecurityStampAsync(user);

                    await _signInManager.SignOutAsync();
                    await _signInManager.SignInAsync(user, true);

                    _toastNotification.AddSuccessToastMessage("Kullanıcı bilgileri başarıyla güncellendi", new NotyOptions()
                    {
                        Theme = "metroui"
                    });
                    _logger.LogInformation("Admin kullanıcısı bilgilerini güncelledi");
                    return RedirectToAction("Settings", "AdminOption");
                }
                else
                    foreach (var item in result.Errors)
                        ModelState.AddModelError("", item.Description);
            }

            return View(model);
        }


        public IActionResult ChangePassword()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> ChangePassword(PasswordChangeAdminViewModel model)
        {
            if (ModelState.IsValid)
            {
                AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);

                bool exist = await _userManager.CheckPasswordAsync(user, model.OldPassword);

                if (exist)
                {
                    IdentityResult result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.PasswordConfirm);
                    if (result.Succeeded)
                    {
                        await _userManager.UpdateSecurityStampAsync(user);

                        await _signInManager.SignOutAsync();
                        await _signInManager.PasswordSignInAsync(user, model.Password, true, false);

                        _toastNotification.AddSuccessToastMessage("Şifreniz başarıyla güncellenmiştir", new NotyOptions()
                        {
                            Theme = "metroui"
                        });
                        _logger.LogInformation("Admin kullanıcısı şifre bilgilerini güncelledi");
                        return RedirectToAction("Settings");
                    }
                    else
                    {
                        foreach (var item in result.Errors)
                        {
                            ModelState.AddModelError("", item.Description);
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Eski şifreniz yanlış");
                }
            }
            return View(model);
        }
    }
}
