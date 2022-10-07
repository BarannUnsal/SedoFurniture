using Microsoft.AspNetCore.Mvc;

namespace MensaApp.API.ViewComponents.HomeServices
{
    public class HomePageService : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
