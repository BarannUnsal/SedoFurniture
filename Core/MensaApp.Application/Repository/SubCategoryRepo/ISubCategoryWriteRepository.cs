using MensaApp.Domain.Entities;

namespace MensaApp.Application.Repository.SubCategoryRepo
{
    public interface ISubCategoryWriteRepository : IWriteRepository<SubCategory>
    {
        Task<bool> DeleteSubCategoryAsync(int id);
    }
}
