using Microsoft.AspNetCore.Mvc;

namespace MensaApp.API.ViewComponents.Product
{
    public class BannerProduct : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
