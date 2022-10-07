using MensaApp.Application.Abstraction.Services;
using Microsoft.AspNetCore.Mvc;

namespace MensaApp.API.ViewComponents.Product
{
    public class GetAllProduct : ViewComponent
    {
        readonly IProductService _productService;

        public GetAllProduct(IProductService productService)
        {
            _productService = productService;
        }

        public IViewComponentResult Invoke()
        {
            var product = _productService.GetProductWithCategory();
            return View(product);
        }
    }
}
