using MensaApp.Application.Abstraction.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;

namespace MensaApp.API.Controllers
{
    [AllowAnonymous]
    public class ProductController : Controller
    {
        readonly IProductService _productService;
        readonly IDataProtector dataProtector;
        readonly ILogger<ProductController> _logger;
        public ProductController(IProductService productService, IDataProtectionProvider dataProtectionProvider, ILogger<ProductController> logger)
        {
            _productService = productService;
            dataProtector = dataProtectionProvider.CreateProtector("ProductController");
            _logger = logger;
        }

        [HttpGet]
        public IActionResult SearchTerm(string? searchString)
        {
            try
            {
                var products = from x in _productService.GetProductWithCategory()
                               select x;

                if (!String.IsNullOrEmpty(searchString))
                {
                    ViewBag.srch = searchString;
                    products = products.Where(x => x.Name!.ToUpper().Contains(searchString.ToUpper()));
                    ViewBag.count = products.Count().ToString();
                }
                else
                {
                    return NotFound();
                }
                return View(products.ToList());

            }
            catch (Exception ex)
            {
                _logger.LogError("Search bar hatası", ex);
                throw;
            }
        }

        [HttpGet]
        [Route("product/{id}/{Name}")]
        public IActionResult ProductDetail(int id)
        {
            try
            {
                var product = _productService.GetProductIdWithCategory(id);
                _logger.LogInformation("Ürün detay sayfasına girildi");
                if (product == null)
                {
                    _logger.LogError("Ürünler sayfasında 404 hatası");
                    return RedirectToAction("PageNotFound", "Error");
                }
                return View(product);
            }
            catch (Exception ex)
            {
                _logger.LogError("Ürün detay sayfasında hata", ex);
                return RedirectToAction("Error", "PageNotFound");
            }
            return View();
        }
    }
}
