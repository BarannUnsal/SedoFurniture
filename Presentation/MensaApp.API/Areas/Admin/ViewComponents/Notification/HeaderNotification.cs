using MensaApp.Application.Abstraction.Services;
using Microsoft.AspNetCore.Mvc;

namespace MensaApp.API.Areas.Admin.ViewComponents.Notification
{
    public class HeaderNotification : ViewComponent
    {
        readonly IContactService _contactService;

        public HeaderNotification(IContactService contactService)
        {
            _contactService = contactService;
        }

        public IViewComponentResult Invoke()
        {
            ViewBag.all = _contactService.GetAll().Count().ToString();
            var contact = _contactService.GetAll().OrderByDescending(x => x.Id).Take(3).ToList();
            return View(contact);
        }
    }
}
