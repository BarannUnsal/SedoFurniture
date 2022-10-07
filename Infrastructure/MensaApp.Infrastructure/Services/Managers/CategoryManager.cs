using MensaApp.Application.Abstraction.Services;
using MensaApp.Application.Repository.CategoryRepo;
using MensaApp.Domain.Entities;
using System.Linq.Expressions;

namespace MensaApp.Infrastructure.Services.Managers
{
    public class CategoryManager : ICategoryService
    {
        readonly ICategoryReadRepository _categoryReadRepository;
        readonly ICategoryWriteRepository _categoryWriteRepository;
        public CategoryManager(ICategoryReadRepository categoryReadRepository, ICategoryWriteRepository categoryWriteRepository)
        {
            _categoryReadRepository = categoryReadRepository;
            _categoryWriteRepository = categoryWriteRepository;
        }
        public async Task<bool> AddAsync(Category model)
        {
            await _categoryWriteRepository.AddAsync(model);
            await _categoryWriteRepository.SaveAsync();
            return true;
        }

        public async Task<bool> AddRangeAsync(List<Category> datas)
        {
            await _categoryWriteRepository.AddRangeAsync(datas);
            await _categoryWriteRepository.SaveAsync();
            return true;
        }

        public IQueryable<Category> GetAll(bool tracking = true)
        {
            return _categoryReadRepository.GetAll(false);
        }

        public async Task<Category> GetByIdAsync(int id, bool tracking = true)
        {
            return await _categoryReadRepository.GetByIdAsync(id);
        }

        public async Task<Category> GetSingleAsync(Expression<Func<Category, bool>> method, bool tracing = true)
        {
            return await _categoryReadRepository.GetSingleAsync(method);
        }

        public IQueryable<Category> GetWhere(Expression<Func<Category, bool>> method, bool tracking = true)
        {
            return _categoryReadRepository.GetWhere(method);
        }

        public void Remove(Category model)
        {
            _categoryWriteRepository.Remove(model);
        }

        public async Task<bool> RemoveAsync(int id)
        {
            await _categoryWriteRepository.DeleteCategoryAsync(id);
            await _categoryWriteRepository.SaveAsync();
            return true;
        }

        public async void RemoveRange(List<Category> datas)
        {
            _categoryWriteRepository.RemoveRange(datas);
            await _categoryWriteRepository.SaveAsync();
        }

        public void Update(Category model)
        {
            _categoryWriteRepository.Update(model);
            _categoryWriteRepository.SaveAsync();
        }

        public async Task<int> SaveAsync()
        {
            return await _categoryWriteRepository.SaveAsync();
        }

        public List<Category> GetCategoryWithSubCategory()
        {
            return _categoryReadRepository.GetCategoryWithSubCategory().OrderBy(x => x.CategoryName).ToList();
        }

        public IQueryable<Category> CategoryMenu()
        {
            return _categoryReadRepository.CategoryMenu().OrderBy(x => x.CategoryName);
        }
    }
}
