using MensaApp.Application.Repository.CategoryRepo;
using MensaApp.Domain.Entities;
using MensaApp.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace MensaApp.Persistence.Repository.CategoryRepo
{
    public class CategoryReadRepository : ReadRepository<Category>, ICategoryReadRepository
    {
        private readonly MensaDbContext context;
        public CategoryReadRepository(MensaDbContext context) : base(context)
        {
            this.context = context;
        }

        public List<Category> GetCategoryWithSubCategory()
        {
            return context.Categories.Include(x => x.SubCategories).ToList();
        }

        public IQueryable<Category> CategoryMenu()
        {
            var category = context.Categories.Include(x => x.SubCategories).ToList();
            var query = category.AsQueryable();
            return query;
        }
    }
}
