using FluentValidation.Results;
using MensaApp.API.Areas.Admin.Models.Categories;
using MensaApp.Application.Abstraction.Services;
using MensaApp.Application.Pagination;
using MensaApp.Domain.Entities;
using MensaApp.Infrastructure.Validations.CategoryValidate.CreateCategory;
using MensaApp.Infrastructure.Validations.CategoryValidate.UpdateCategory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;

namespace MensaApp.API.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class AdminCategoryController : Controller
    {
        readonly ICategoryService _categoryService;
        readonly IToastNotification _toastNotification;
        readonly ILogger<AdminCategoryController> _logger;

        public AdminCategoryController(ICategoryService categoryService, IToastNotification toastNotification, ILogger<AdminCategoryController> logger)
        {
            _categoryService = categoryService;
            _toastNotification = toastNotification;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Categories(string? sortOrder, string? currentFilter, string? searchString, int? pageNumber)
        {
            try
            {
                ViewData["IdSortParm"] = String.IsNullOrEmpty(sortOrder) ? "id_desc" : "";
                ViewData["NameSortParm"] = sortOrder == "Name" ? "name_desc" : "Name";
                ViewData["CreateDateSortParm"] = sortOrder == "CreateDate" ? "createDate_desc" : "CreateDate";
                ViewData["UpdateDateSortParm"] = sortOrder == "UpdateDate" ? "updateDate_desc" : "UpdateDate";
                ViewData["CurrentFilter"] = searchString;
                ViewData["CurrentSort"] = sortOrder;

                ViewBag.catCount = _categoryService.GetAll().Count().ToString();

                var categories = from x in _categoryService.GetAll().ToList()
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
                    categories = categories.Where(x => x.CategoryName.ToUpper().Contains(searchString.ToUpper()));
                }

                switch (sortOrder)
                {
                    case "id_desc":
                        categories = categories.OrderByDescending(x => x.Id);
                        break;
                    case "Name":
                        categories = categories.OrderBy(x => x.CategoryName);
                        break;
                    case "name_desc":
                        categories = categories.OrderByDescending(x => x.CategoryName);
                        break;
                    case "CreateDate":
                        categories = categories.OrderBy(x => x.CreateDate);
                        break;
                    case "createDate_desc":
                        categories = categories.OrderByDescending(x => x.CreateDate);
                        break;
                    case "UpdateDate":
                        categories = categories.OrderBy(x => x.UpdateDate);
                        break;
                    case "updateDate_desc":
                        categories = categories.OrderByDescending(x => x.UpdateDate);
                        break;
                    default:
                        categories = categories.OrderBy(x => x.Id);
                        break;
                }
                int pageSize = 5;
                return View(PaginatedList<Category>.Create(categories.ToList(), pageNumber ?? 1, pageSize));
            }
            catch (Exception ex)
            {
                _logger.LogError("Kategori sayfasında hata ile karşılaşıldı ->>", ex);
                return null;
            }
        }

        [HttpGet]
        public IActionResult AddCategory()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> AddCategory(CategoryViewModel model)
        {
            Category category = new();
            category.CategoryName = model.CategoryName;
            category.IsActive = model.IsActive;
            category.CreateDate = DateTime.UtcNow;
            CreateCategoryValidation validations = new();
            ValidationResult result = validations.Validate(category);
            if (result.IsValid)
            {
                _toastNotification.AddSuccessToastMessage($"{model.CategoryName} başarıyla eklendi", new NotyOptions()
                {
                    Theme = "metroui"
                });
                await _categoryService.AddAsync(category);
                _logger.LogInformation("Admin yeni kategori ekledi");
                return RedirectToAction("Categories");
            }
            else
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }
                return View(category);
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditCategory(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);
            return View(category);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> EditCategory(UpdateCategoryViewModel model)
        {
            var category = await _categoryService.GetByIdAsync(model.Id);
            category.CategoryName = model.CategoryName;
            category.UpdateDate = DateTime.UtcNow;
            category.IsActive = model.IsActive;
            _categoryService.Update(category);
            UpdateCategoryValidation validation = new();
            ValidationResult result = validation.Validate(category);
            if (result.IsValid)
            {
                _toastNotification.AddSuccessToastMessage($"{model.CategoryName} başarıyla güncellendi", new NotyOptions()
                {
                    Theme = "metroui"
                });
                _logger.LogInformation("Admin kategori güncelledi");
                return RedirectToAction("Categories");
            }
            else
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }
                return View(category);
            }
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveCategory(int id)
        {
            await _categoryService.RemoveAsync(id);
            _logger.LogInformation("Admin kategori sildi");
            _toastNotification.AddErrorToastMessage("Kategori başarıyla silindi!");
            return RedirectToAction("Categories");
        }
    }
}
