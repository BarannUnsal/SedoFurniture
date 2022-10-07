using MensaApp.Application.Abstraction.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MensaApp.API.Controllers
{
    [AllowAnonymous]
    public class AboutController : Controller
    {
        readonly IAboutService _aboutService;
        readonly ILogger<AboutController> logger;

        public AboutController(IAboutService aboutService, ILogger<AboutController> logger)
        {
            _aboutService = aboutService;
            this.logger = logger;
        }


        [HttpGet]
        [Route("/about-us")]
        public IActionResult AboutUs()
        {
            try
            {
                var about = _aboutService.GetAll().ToList();
                logger.LogInformation("Hakkımızda sayfasına girildi.");
                return View(about);
            }
            catch (Exception ex)
            {
                logger.LogError("Hakkımızda sayfasında hata ile karşılaşıldı!", ex);
            }
            return View();
        }
    }
}
