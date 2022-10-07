using FluentValidation.Results;
using MensaApp.API.Filters;
using MensaApp.API.Models;
using MensaApp.Application.Abstraction.Services;
using MensaApp.Domain.Entities;
using MensaApp.Infrastructure.Validations.ContactValidate.CreateContact;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;

namespace MensaApp.API.Controllers
{
    [AllowAnonymous]
    //[ServiceFilter(typeof(CheckWhiteList))]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductService _productService;
        private readonly IContactService _contactService;
        private readonly IToastNotification _toastNotification;

        public HomeController(ILogger<HomeController> logger, IProductService productService, IContactService contactService, IToastNotification toastNotification)
        {
            _logger = logger;
            _productService = productService;
            _contactService = contactService;
            _toastNotification = toastNotification;
        }

        public IActionResult Index()
        {
            try
            {
                _logger.LogInformation("Ana sayfa açıldı");
            }
            catch (Exception ex)
            {
                _logger.LogError("Ana sayfa hata ile karşılaşıldı");
                return RedirectToAction("Error", "PageNotFound");
            }
            return View();
        }

        public IActionResult CarouselCard()
        {
            var product = _productService.CarouselProduct();
            return View(product);
        }

        [HttpGet]
        public IActionResult ContactForm()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> ContactForm(ContactUserViewModel model)
        {
            Contact contact = new();
            contact.Name = model.Name;
            contact.Description = model.Description;
            contact.Email = model.Email;
            contact.Subject = model.Subject;
            contact.CreateDate = DateTime.UtcNow;
            contact.MessageStatus = false;
            CreateContactValidator validations = new();
            ValidationResult result = await validations.ValidateAsync(contact);
            if (result.IsValid)
            {
                _toastNotification.AddSuccessToastMessage($"{model.Name} Teşekkürler! Mesajın başarılı bir şekilde gönderildi", new NotyOptions()
                {
                    Theme = "metroui"
                });
                await _contactService.AddAsync(contact);
                return RedirectToAction("Index");
            }
            else
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }
            }
            return View(contact);
        }
    }
}
