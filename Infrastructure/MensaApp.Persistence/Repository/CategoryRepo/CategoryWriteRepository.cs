using MensaApp.Application.Repository.CategoryRepo;
using MensaApp.Domain.Entities;
using MensaApp.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace MensaApp.Persistence.Repository.CategoryRepo
{
    public class CategoryWriteRepository : WriteRepository<Category>, ICategoryWriteRepository
    {
        private readonly MensaDbContext context;
        public CategoryWriteRepository(MensaDbContext context) : base(context)
        {
            this.context = context;
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var category = await context.Categories.Include(x => x.SubCategories).ThenInclude(x => x.Products).ThenInclude(x => x.ProductImages).FirstOrDefaultAsync(x => x.Id == id);
            if (category != null)
            {
                return Remove(category);
            }
            await context.SaveChangesAsync();
            return false;
        }
    }
}
