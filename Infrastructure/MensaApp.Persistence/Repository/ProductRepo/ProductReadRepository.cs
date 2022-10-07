using MensaApp.Application.Repository.ProductRepo;
using MensaApp.Domain.Entities;
using MensaApp.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MensaApp.Persistence.Repository.ProductRepo
{
    public class ProductReadRepository : ReadRepository<Product>, IProductReadRepository
    {
        private readonly MensaDbContext context;

        public ProductReadRepository(MensaDbContext context) : base(context)
        {
            this.context = context;
        }

        public List<Product> GetProductWithCategory()
        {
            return context.Products.Include(x => x.Category).Include(x => x.SubCategory).Include(x => x.ProductImages).ToList();
        }

        public Product GetProductIdWithCategory(int id)
        {
            return context.Products.Where(x => x.Id == id).Include(x => x.Category).Include(x => x.SubCategory).Include(x => x.ProductImages).FirstOrDefault();
        }

        public List<Product> GetLast10Product()
        {
            return context.Products.Include(x => x.Category).Include(x => x.SubCategory).OrderByDescending(x => x.Id).Take(10).ToList();
        }

        public List<Product> List(Expression<Func<Product, bool>> filter)
        {
            return context.Products.Where(filter).Include(x => x.Category).Include(x => x.SubCategory).ToList();
        }
    }
}
