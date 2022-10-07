using MensaApp.Application.Abstraction.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MensaApp.API.Controllers
{
    [AllowAnonymous]
    public class CategoryController : Controller
    {
        readonly ICategoryService _categoryService;
        readonly ILogger<CategoryController> _logger;

        public CategoryController(ICategoryService categoryService, ILogger<CategoryController> logger)
        {
            _categoryService = categoryService;
            _logger = logger;
        }

        [HttpGet]
        [Route("category/{id}/{CategoryName}")]
        public IActionResult CategoryDetails(int id)
        {
            try
            {
                var cat = _categoryService.GetByIdAsync(id);
                var name = cat.Result.CategoryName;
                ViewBag.CatName = name;
                ViewBag.CatID = id;
                _logger.LogInformation("Kategori sayfasına girildi.");
            }
            catch (Exception ex)
            {
                _logger.LogError("Kategori sayfasında hata ile karşılaşıldı", ex);
                return RedirectToAction("Error", "PageNotFound");
            }
            return View();
        }
    }
}
