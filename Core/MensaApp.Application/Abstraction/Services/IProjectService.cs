using MensaApp.Domain.Entities;

namespace MensaApp.Application.Abstraction.Services
{
    public interface IProjectService : IGenericService<Project>
    {
        Task<Project> GetByIdProjectAsync(int id, bool tracking = true);
    }
}
