using FluentValidation.Results;
using MensaApp.API.Areas.Admin.Models.Abouts;
using MensaApp.Application.Abstraction.Services;
using MensaApp.Domain.Entities;
using MensaApp.Infrastructure.Validations.AboutValidate.CreateAbout;
using MensaApp.Infrastructure.Validations.AboutValidate.UpdateAbout;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;

namespace MensaApp.API.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class AdminAboutController : Controller
    {
        readonly IAboutService _aboutService;
        private readonly IToastNotification _toastNotification;
        readonly ILogger<AdminAboutController> _logger;
        public AdminAboutController(IAboutService aboutService, IToastNotification toastNotification, ILogger<AdminAboutController> logger)
        {
            _aboutService = aboutService;
            _toastNotification = toastNotification;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult AboutUs()
        {
            var about = _aboutService.GetAll().ToList();
            _logger.LogInformation("Admin kullanıcı hakkımızda sayfasına girdi");
            return View(about);
        }

        [HttpGet]
        public IActionResult CreateAbout()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> CreateAbout(CreateAboutViewModel model)
        {
            About about = new() { Title = model.Title, Description = model.Description };
            CreateAboutValidation validations = new();
            ValidationResult result = validations.Validate(about);
            if (result.IsValid)
            {
                _toastNotification.AddSuccessToastMessage($"{model.Title} Başarıyla Eklendi", new NotyOptions()
                {
                    Theme = "metroui"
                });
                await _aboutService.AddAsync(about);
                _logger.LogInformation("Admin kullanıcı yeni hakkımızda ekledi");
                return RedirectToAction("AboutUs");
            }
            else
                foreach (var item in result.Errors)
                {
                    _logger.LogInformation("Admin kullanıcı hakkımızda eklerken hata ile karşılaştı");
                    ModelState.AddModelError(item.ErrorMessage, item.PropertyName);
                }
            return View(about);
        }

        [HttpGet]
        public async Task<IActionResult> EditAbout(int id)
        {
            var about = await _aboutService.GetByIdAsync(id);
            _logger.LogInformation("Admin kullanıcısı hakkımızda güncelleme sayfasına girdi");
            return View(about);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> EditAbout(UpdateAboutViewModel model)
        {
            var about = await _aboutService.GetByIdAsync(model.Id);
            about.Title = model.Title;
            about.Description = model.Description;
            _aboutService.Update(about);
            UpdateAboutValidation validations = new();
            ValidationResult result = validations.Validate(about);
            if (result.IsValid)
            {
                _toastNotification.AddSuccessToastMessage($"{model.Title} Başarıyla Güncellendi", new NotyOptions()
                {
                    Theme = "metroui"
                });
                _logger.LogInformation("Admin kullanıcı hakkımızda güncellemesi yaptı");
                return RedirectToAction("AboutUs");
            }
            else
                foreach (var item in result.Errors)
                {
                    _logger.LogError("Admin kullanıcısı hakkımızda güncellerken hata ile karşılaştı");
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }
            return View(about);
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveAbout(int id)
        {
            await _aboutService.RemoveAsync(id);
            _logger.LogInformation("Admin kullanıcısı hakkımızda sildi");
            _toastNotification.AddErrorToastMessage("Başarıyla Silindi!");
            return RedirectToAction("AboutUs");
        }
    }
}
