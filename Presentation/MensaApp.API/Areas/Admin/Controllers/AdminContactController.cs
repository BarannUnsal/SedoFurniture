using MensaApp.Application.Abstraction.Services;
using MensaApp.Application.Pagination;
using MensaApp.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;

namespace MensaApp.API.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class AdminContactController : Controller
    {
        readonly IContactService _contactService;
        readonly IToastNotification _toastNotification;
        readonly ILogger<AdminContactController> _logger;

        public AdminContactController(IContactService contactService, IToastNotification toastNotification, ILogger<AdminContactController> logger)
        {
            _contactService = contactService;
            _toastNotification = toastNotification;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetContacts(string? sortOrder, string? currentFilter, string? searchString, int? pageNumber)
        {
            try
            {
                ViewData["IdSortParm"] = String.IsNullOrEmpty(sortOrder) ? "id_desc" : "";
                ViewData["NameSortParm"] = sortOrder == "Name" ? "name_desc" : "Name";
                ViewData["EmailSortParm"] = sortOrder == "Email" ? "email_desc" : "Email";
                ViewData["CreateDateSortParm"] = sortOrder == "CreateDate" ? "createDate_desc" : "CreateDate";
                ViewData["CurrentFilter"] = searchString;
                ViewData["CurrentSort"] = sortOrder;

                ViewBag.contactCount = _contactService.GetAll().Count().ToString();

                var contacts = from x in _contactService.GetAll().ToList()
                               select x;


                if (searchString != null)
                {
                    pageNumber = 1;
                }
                else
                {
                    searchString = currentFilter;
                }

                if (!String.IsNullOrEmpty(searchString))
                {
                    contacts = contacts.Where(x => x.Name.ToUpper().Contains(searchString.ToUpper())
                                          || x.Email.ToUpper().Contains(searchString.ToUpper()));
                }

                switch (sortOrder)
                {
                    case "id_desc":
                        contacts = contacts.OrderByDescending(x => x.Id);
                        break;
                    case "Name":
                        contacts = contacts.OrderBy(x => x.Name);
                        break;
                    case "name_desc":
                        contacts = contacts.OrderByDescending(x => x.Name);
                        break;
                    case "Email":
                        contacts = contacts.OrderBy(x => x.Email);
                        break;
                    case "email_desc":
                        contacts = contacts.OrderByDescending(x => x.Email);
                        break;
                    case "CreateDate":
                        contacts = contacts.OrderBy(x => x.CreateDate);
                        break;
                    case "createDate_desc":
                        contacts = contacts.OrderByDescending(x => x.CreateDate);
                        break;
                    default:
                        contacts = contacts.OrderBy(x => x.Id);
                        break;
                }
                int pageSize = 5;
                return View(PaginatedList<Contact>.Create(contacts.ToList(), pageNumber ?? 1, pageSize));
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetContactDetails(int id)
        {
            var contact = await _contactService.GetByIdAsync(id);
            _logger.LogInformation("Admin kullanıcısı gelen bir mesajı okudu");
            contact.MessageStatus = true;
            return View(contact);
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveMessage(int id)
        {
            await _contactService.RemoveAsync(id);
            _logger.LogInformation("Admin kullanıcısı gelen bir mesajı sildi");
            _toastNotification.AddErrorToastMessage("Mesaj başarıyla silindi");
            return RedirectToAction("GetContacts");
        }
    }
}
