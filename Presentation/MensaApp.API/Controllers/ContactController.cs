using FluentValidation.Results;
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
    public class ContactController : Controller
    {
        private readonly IToastNotification _toastNotification;
        private readonly IContactService _contactService;
        private readonly ILogger<ContactController> _logger;

        public ContactController(IToastNotification toastNotification, IContactService contactService, ILogger<ContactController> logger)
        {
            _toastNotification = toastNotification;
            _contactService = contactService;
            _logger = logger;
        }

        [Route("/contact")]
        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Contact(ContactUserViewModel model)
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
                try
                {
                    _toastNotification.AddSuccessToastMessage($"{model.Name} Teşekkürler! Mesajın başarılı bir şekilde gönderildi", new NotyOptions()
                    {
                        Theme = "metroui"
                    });
                    await _contactService.AddAsync(contact);
                    return RedirectToAction("Contact");
                }
                catch (Exception ex)
                {
                    _logger.LogError("İletişim formu doludururken beklenmeyen hata ->>", ex);
                }
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
