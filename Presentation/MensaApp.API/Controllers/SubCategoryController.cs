using MensaApp.Application.Abstraction.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MensaApp.API.Controllers
{
    [AllowAnonymous]
    public class SubCategoryController : Controller
    {
        private readonly ILogger<SubCategoryController> _logger;
        readonly ISubCategoryService subCategoryService;

        public SubCategoryController(ILogger<SubCategoryController> logger, ISubCategoryService subCategoryService)
        {
            _logger = logger;
            this.subCategoryService = subCategoryService;
        }

        [HttpGet]
        [Route("subcategory/{id}/{Name}")]
        public async Task<IActionResult> GetSubCategoryProduct(int id)
        {
            try
            {
                var cat = await subCategoryService.GetSubCategoryIdWithCategoryAsync(id);
                var subCatName = cat.Name;
                var catName = cat.Category.CategoryName;
                var catId = cat.Category.Id;
                ViewBag.subCatName = subCatName;
                ViewBag.catName = catName;
                ViewBag.catID = catId;
                ViewBag.subID = id;
                _logger.LogInformation("Alt kategori sayfasına girildi");
            }
            catch (Exception ex)
            {
                _logger.LogError("Alt kategori sayfasına girilirken beklenmeyen hata");
                return RedirectToAction("Error", "PageNotFound");
            }
            return View();
        }
    }
}
