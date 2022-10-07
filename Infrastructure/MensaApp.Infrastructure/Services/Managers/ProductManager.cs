using MensaApp.Application.Abstraction.Services;
using MensaApp.Application.Repository.ProductRepo;
using MensaApp.Domain.Entities;
using System.Linq.Expressions;

namespace MensaApp.Infrastructure.Services.Managers
{
    public class ProductManager : IProductService
    {
        readonly IProductReadRepository _productReadRepository;
        readonly IProductWriteRepository _productWriteRepository;
        public ProductManager(IProductReadRepository productReadRepository, IProductWriteRepository productWriteRepository)
        {
            _productReadRepository = productReadRepository;
            _productWriteRepository = productWriteRepository;
        }

        public async Task<bool> AddAsync(Product model)
        {
            await _productWriteRepository.AddAsync(model);
            await _productWriteRepository.SaveAsync();
            return true;
        }

        public async Task<bool> AddRangeAsync(List<Product> datas)
        {
            await _productWriteRepository.AddRangeAsync(datas);
            await _productWriteRepository.SaveAsync();
            return true;
        }

        public List<Product> CarouselProduct()
        {
            return _productReadRepository.GetAll().OrderByDescending(x => x.Id).Take(2).ToList();
        }

        public List<Product> GetProductPage()
        {
            return _productReadRepository.GetProductWithCategory().OrderByDescending(x => x.Id).Take(6).ToList();
        }

        public List<Product> GetLast10Product()
        {
            return _productReadRepository.GetLast10Product();
        }

        public IQueryable<Product> GetAll(bool tracking = true)
        {
            return _productReadRepository.GetAll(false);
        }

        public async Task<Product> GetByIdAsync(int id, bool tracking = true)
        {
            return await _productReadRepository.GetByIdAsync(id, false);
        }

        public Product GetProductIdWithCategory(int id)
        {
            return _productReadRepository.GetProductIdWithCategory(id);
        }

        public List<Product> GetProductWithCategory()
        {
            return _productReadRepository.GetProductWithCategory();
        }

        public async Task<Product> GetSingleAsync(Expression<Func<Product, bool>> method, bool tracing = true)
        {
            return await _productReadRepository.GetSingleAsync(method);
        }

        public IQueryable<Product> GetWhere(Expression<Func<Product, bool>> method, bool tracking = true)
        {
            return _productReadRepository.GetWhere(method);
        }

        public void Remove(Product model)
        {
            _productWriteRepository.Remove(model);
        }

        public async Task<bool> RemoveAsync(int id)
        {
            await _productWriteRepository.RemoveAsync(id);
            await _productWriteRepository.SaveAsync();
            return true;
        }

        public void RemoveRange(List<Product> datas)
        {
            _productWriteRepository.RemoveRange(datas);
            _productWriteRepository.SaveAsync();
        }

        public async Task<int> SaveAsync()
        {
            return await _productWriteRepository.SaveAsync();
        }

        public void Update(Product model)
        {
            _productWriteRepository.Update(model);
            _productWriteRepository.SaveAsync();
        }

        public List<Product> GetProductWithCategoryId(Expression<Func<Product, bool>> filter)
        {
            return _productReadRepository.List(filter).ToList();
        }

        public List<Product> GetProductWithSubCategoryId(Expression<Func<Product, bool>> filter)
        {
            return _productReadRepository.List(filter).ToList();
        }
    }
}
