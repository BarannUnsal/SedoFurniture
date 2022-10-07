using MensaApp.Domain.Entities;

namespace MensaApp.Application.Repository.CategoryRepo
{
    public interface ICategoryReadRepository : IReadRepository<Category>
    {
        List<Category> GetCategoryWithSubCategory();
        IQueryable<Category> CategoryMenu();
    }
}
