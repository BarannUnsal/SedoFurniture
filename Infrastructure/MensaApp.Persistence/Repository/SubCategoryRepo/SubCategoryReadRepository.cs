using MensaApp.Application.Repository.SubCategoryRepo;
using MensaApp.Domain.Entities;
using MensaApp.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace MensaApp.Persistence.Repository.SubCategoryRepo
{
    public class SubCategoryReadRepository : ReadRepository<SubCategory>, ISubCategoryReadRepository
    {
        private readonly MensaDbContext _context;

        public SubCategoryReadRepository(MensaDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<SubCategory> GetSubCategoryIdWithCategoryAsync(int id)
        {
            return await _context.SubCategories.Include(x => x.Category).Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public List<SubCategory> GetSubCategoryWithCategory()
        {
            return _context.SubCategories.Include(x => x.Products).Include(x => x.Category).ToList();
        }
    }
}
