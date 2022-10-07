using Microsoft.AspNetCore.Mvc;

namespace MensaApp.API.ViewComponents.ContactHomePage
{
    public class ContactInfo : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
