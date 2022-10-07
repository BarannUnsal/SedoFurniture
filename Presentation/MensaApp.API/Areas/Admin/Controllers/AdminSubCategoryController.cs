using FluentValidation.Results;
using MensaApp.API.Areas.Admin.Models.SubCategories;
using MensaApp.Application.Abstraction.Services;
using MensaApp.Application.Pagination;
using MensaApp.Domain.Entities;
using MensaApp.Infrastructure.Validations.SubCategoryValidate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NToastNotify;

namespace MensaApp.API.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class AdminSubCategoryController : Controller
    {
        readonly ISubCategoryService _subCategoryService;
        readonly ICategoryService _categoryService;
        readonly IToastNotification _toastNotification;
        readonly ILogger<AdminSubCategoryController> logger;

        public AdminSubCategoryController(ISubCategoryService subCategoryService, ICategoryService categoryService, IToastNotification toastNotification, ILogger<AdminSubCategoryController> logger)
        {
            _subCategoryService = subCategoryService;
            _categoryService = categoryService;
            _toastNotification = toastNotification;
            this.logger = logger;
        }

        [HttpGet]
        public IActionResult GetSubCategories(string? sortOrder, string? currentFilter, string? searchString, int? pageNumber)
        {
            try
            {
                ViewData["IdSortParm"] = String.IsNullOrEmpty(sortOrder) ? "id_desc" : "";
                ViewData["NameSortParm"] = sortOrder == "Name" ? "name_desc" : "Name";
                ViewData["CategorySortParm"] = sortOrder == "Category" ? "cat_desc" : "Category";
                ViewData["CreateDateSortParm"] = sortOrder == "CreateDate" ? "createDate_desc" : "CreateDate";
                ViewData["UpdateDateSortParm"] = sortOrder == "UpdateDate" ? "updateDate_desc" : "UpdateDate";
                ViewData["CurrentFilter"] = searchString;
                ViewData["CurrentSort"] = sortOrder;

                ViewBag.subCatCount = _subCategoryService.GetSubCategoryWithCategory().Count().ToString();

                var subCategories = from x in _subCategoryService.GetSubCategoryWithCategory()
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
                    subCategories = subCategories.Where(x => x.Name.ToUpper().Contains(searchString.ToUpper())
                                          || x.Category.CategoryName.ToUpper().Contains(searchString.ToUpper()));
                }

                switch (sortOrder)
                {
                    case "id_desc":
                        subCategories = subCategories.OrderByDescending(x => x.Id);
                        break;
                    case "Name":
                        subCategories = subCategories.OrderBy(x => x.Name);
                        break;
                    case "name_desc":
                        subCategories = subCategories.OrderByDescending(x => x.Name);
                        break;
                    case "Category":
                        subCategories = subCategories.OrderBy(x => x.Category.CategoryName);
                        break;
                    case "cat_desc":
                        subCategories = subCategories.OrderByDescending(x => x.Category.CategoryName);
                        break;
                    case "CreateDate":
                        subCategories = subCategories.OrderBy(x => x.CreateDate);
                        break;
                    case "createDate_desc":
                        subCategories = subCategories.OrderByDescending(x => x.CreateDate);
                        break;
                    case "UpdateDate":
                        subCategories = subCategories.OrderBy(x => x.UpdateDate);
                        break;
                    case "updateDate_desc":
                        subCategories = subCategories.OrderByDescending(x => x.UpdateDate);
                        break;
                    default:
                        subCategories = subCategories.OrderBy(x => x.Id);
                        break;
                }
                int pageSize = 5;
                return View(PaginatedList<SubCategory>.Create(subCategories.ToList(), pageNumber ?? 1, pageSize));
            }
            catch (Exception ex)
            {
                logger.LogError("Alt kategorilerde hata ile karşılaşıldı ->>", ex);
                return null;
            }
        }

        public IActionResult GetSubCategory(int cid)
        {
            var SubCategory_List = _categoryService.GetAll().Where(s => s.Id == cid).FirstOrDefault().SubCategories.Select(c => new { Id = c.Id, Name = c.Name }).ToList();
            return Json(SubCategory_List);
        }

        [HttpGet]
        public IActionResult CreateSubCategory()
        {
            List<SelectListItem> categoryValue = (from x in _categoryService.GetAll()
                                                  select new SelectListItem
                                                  {
                                                      Text = x.CategoryName,
                                                      Value = x.Id.ToString()
                                                  }).ToList();
            ViewBag.ListOfCategory = categoryValue;
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> CreateSubCategory(SubCategoryCreateViewModel model)
        {
            SubCategory subCategory = new();
            subCategory.Name = model.Name;
            subCategory.CategoryId = model.CategoryId;
            subCategory.CreateDate = DateTime.UtcNow;
            await _subCategoryService.AddAsync(subCategory);
            SubCategoryValidation validation = new();
            ValidationResult result = validation.Validate(subCategory);
            if (result.IsValid)
            {
                _toastNotification.AddSuccessToastMessage($"{model.Name} başarıyla eklendi", new NotyOptions()
                {
                    Theme = "metroui"
                });
                logger.LogInformation("YENİ Alt kategori oluşuturldu");
                return RedirectToAction("GetSubCategories");
            }
            else
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }
                logger.LogError("YENİ Alt kategori oluşturulurken hata!");
                return View(subCategory);
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditSubCategory(int id)
        {
            var subCategory = await _subCategoryService.GetByIdAsync(id);
            List<SelectListItem> categoryValue = (from x in _categoryService.GetAll()
                                                  select new SelectListItem
                                                  {
                                                      Text = x.CategoryName,
                                                      Value = x.Id.ToString()
                                                  }).ToList();
            ViewBag.ListOfCategory = categoryValue;
            return View(subCategory);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> EditSubCategory(SubCategoryUpdateViewModel model)
        {
            var subCategory = await _subCategoryService.GetByIdAsync(model.Id);
            subCategory.Name = model.Name;
            subCategory.UpdateDate = DateTime.UtcNow;
            subCategory.CategoryId = model.CategoryId;
            _subCategoryService.Update(subCategory);
            SubCategoryValidation validations = new();
            ValidationResult result = validations.Validate(subCategory);
            if (result.IsValid)
            {
                _toastNotification.AddSuccessToastMessage($"{model.Name} başarıyla güncellendi", new NotyOptions()
                {
                    Theme = "metroui"
                });
                return RedirectToAction("GetSubCategories");
            }
            else
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }
                return View(subCategory);
            }
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveSubCategory(int id)
        {
            await _subCategoryService.RemoveAsync(id);
            _toastNotification.AddErrorToastMessage("Alt Kategori başarıyla silindi");
            return RedirectToAction("GetSubCategories");
        }
    }
}
