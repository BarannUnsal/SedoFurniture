using MensaApp.Domain.Entities;

namespace MensaApp.Application.Abstraction.Services
{
    public interface ISubCategoryService : IGenericService<SubCategory>
    {
        List<SubCategory> GetSubCategoryWithCategory();
        Task<SubCategory> GetSubCategoryIdWithCategoryAsync(int id);
    }
}
