using MensaApp.Application.Abstraction.Services;
using MensaApp.Application.Repository.SubCategoryRepo;
using MensaApp.Domain.Entities;
using System.Linq.Expressions;

namespace MensaApp.Infrastructure.Services.Managers
{
    public class SubCategoryManager : ISubCategoryService
    {
        readonly ISubCategoryReadRepository _subCategoryReadRepository;
        readonly ISubCategoryWriteRepository _subCategoryWriteRepository;

        public SubCategoryManager(ISubCategoryReadRepository subCategoryReadRepository, ISubCategoryWriteRepository subCategoryWriteRepository)
        {
            _subCategoryReadRepository = subCategoryReadRepository;
            _subCategoryWriteRepository = subCategoryWriteRepository;
        }

        public async Task<bool> AddAsync(SubCategory model)
        {
            await _subCategoryWriteRepository.AddAsync(model);
            await _subCategoryWriteRepository.SaveAsync();
            return true;
        }

        public async Task<bool> AddRangeAsync(List<SubCategory> datas)
        {
            await _subCategoryWriteRepository.AddRangeAsync(datas);
            await _subCategoryWriteRepository.SaveAsync();
            return true;
        }

        public IQueryable<SubCategory> GetAll(bool tracking = true)
        {
            return _subCategoryReadRepository.GetAll(false);
        }

        public async Task<SubCategory> GetByIdAsync(int id, bool tracking = true)
        {
            return await _subCategoryReadRepository.GetByIdAsync(id, false);
        }

        public async Task<SubCategory> GetSingleAsync(Expression<Func<SubCategory, bool>> method, bool tracing = true)
        {
            return await _subCategoryReadRepository.GetSingleAsync(method, false);
        }

        public async Task<SubCategory> GetSubCategoryIdWithCategoryAsync(int id)
        {
            return await _subCategoryReadRepository.GetSubCategoryIdWithCategoryAsync(id);
        }

        public List<SubCategory> GetSubCategoryWithCategory()
        {
            return _subCategoryReadRepository.GetSubCategoryWithCategory();
        }

        public IQueryable<SubCategory> GetWhere(Expression<Func<SubCategory, bool>> method, bool tracking = true)
        {
            return _subCategoryReadRepository.GetWhere(method, false);
        }

        public void Remove(SubCategory model)
        {
            _subCategoryWriteRepository.Remove(model);
        }

        public async Task<bool> RemoveAsync(int id)
        {
            await _subCategoryWriteRepository.DeleteSubCategoryAsync(id);
            await _subCategoryWriteRepository.SaveAsync();
            return true;
        }

        public void RemoveRange(List<SubCategory> datas)
        {
            _subCategoryWriteRepository.RemoveRange(datas);
        }

        public Task<int> SaveAsync()
            => _subCategoryWriteRepository.SaveAsync();

        public void Update(SubCategory model)
        {
            _subCategoryWriteRepository.Update(model);
        }
    }
}
