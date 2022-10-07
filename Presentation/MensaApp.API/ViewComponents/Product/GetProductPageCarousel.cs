using MensaApp.Application.Abstraction.Services;
using Microsoft.AspNetCore.Mvc;

namespace MensaApp.API.ViewComponents.Product
{
    public class GetProductPageCarousel : ViewComponent
    {
        readonly IProductService _productService;

        public GetProductPageCarousel(IProductService productService)
        {
            _productService = productService;
        }

        public IViewComponentResult Invoke()
        {
            var product = _productService.GetProductPage();
            return View(product);
        }
    }
}
