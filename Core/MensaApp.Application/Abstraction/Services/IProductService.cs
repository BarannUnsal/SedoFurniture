using MensaApp.Domain.Entities;
using System.Linq.Expressions;

namespace MensaApp.Application.Abstraction.Services
{
    public interface IProductService : IGenericService<Product>
    {
        List<Product> GetProductWithCategory();
        List<Product> CarouselProduct();
        Product GetProductIdWithCategory(int id);
        List<Product> GetLast10Product();
        List<Product> GetProductWithCategoryId(Expression<Func<Product, bool>> filter);
        List<Product> GetProductPage();
        List<Product> GetProductWithSubCategoryId(Expression<Func<Product, bool>> filter); 
    }
}
