using MensaApp.Application.Repository.SubCategoryRepo;
using MensaApp.Domain.Entities;
using MensaApp.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace MensaApp.Persistence.Repository.SubCategoryRepo
{
    public class SubCategoryWriteRepository : WriteRepository<SubCategory>, ISubCategoryWriteRepository
    {
        private readonly MensaDbContext context;
        public SubCategoryWriteRepository(MensaDbContext context) : base(context)
        {
            this.context = context;
        }

        public async Task<bool> DeleteSubCategoryAsync(int id)
        {
            var subCategory = await context.SubCategories.Include(x => x.Products).ThenInclude(x => x.ProductImages).FirstOrDefaultAsync(x => x.Id == id);
            if (subCategory != null)
            {
                await context.SaveChangesAsync();
                return Remove(subCategory);
            }
            return false;
        }
    }
}
