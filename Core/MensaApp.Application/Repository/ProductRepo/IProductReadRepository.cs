using MensaApp.Domain.Entities;
using System.Linq.Expressions;

namespace MensaApp.Application.Repository.ProductRepo
{
    public interface IProductReadRepository : IReadRepository<Product>
    {
        List<Product> GetProductWithCategory();
        Product GetProductIdWithCategory(int id);
        List<Product> GetLast10Product();
        List<Product> List(Expression<Func<Product, bool>> filter);
    }
}
