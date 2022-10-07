using MensaApp.Domain.Entities;

namespace MensaApp.Application.Abstraction.Services
{
    public interface ICategoryService : IGenericService<Category>
    {
        List<Category> GetCategoryWithSubCategory();
        IQueryable<Category> CategoryMenu();
    }
}
