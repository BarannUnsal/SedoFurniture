using MensaApp.Domain.Entities;

namespace MensaApp.Application.Repository.ProjectRepo
{
    public interface IProjectReadRepository : IReadRepository<Project>
    {
        Task<Project> GetProjectIdAsync(int id);
    }
}
