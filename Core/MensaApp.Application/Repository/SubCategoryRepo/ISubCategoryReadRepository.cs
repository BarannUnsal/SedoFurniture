using MensaApp.Domain.Entities;

namespace MensaApp.Application.Repository.SubCategoryRepo
{
    public interface ISubCategoryReadRepository : IReadRepository<SubCategory>
    {
        List<SubCategory> GetSubCategoryWithCategory();
        Task<SubCategory> GetSubCategoryIdWithCategoryAsync(int id);
    }
}
