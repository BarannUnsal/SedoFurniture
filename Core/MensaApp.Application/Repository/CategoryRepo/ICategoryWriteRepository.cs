using MensaApp.Domain.Entities;

namespace MensaApp.Application.Repository.CategoryRepo
{
    public interface ICategoryWriteRepository : IWriteRepository<Category>
    {
        Task<bool> DeleteCategoryAsync(int id);
    }
}
