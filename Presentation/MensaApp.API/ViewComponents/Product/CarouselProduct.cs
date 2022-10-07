using Microsoft.AspNetCore.Mvc;

namespace MensaApp.API.ViewComponents.Product
{
    public class CarouselProduct : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
