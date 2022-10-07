using MensaApp.Application.Repository.ProjectRepo;
using MensaApp.Domain.Entities;
using MensaApp.Persistence.Context;

namespace MensaApp.Persistence.Repository.ProjectRepo
{
    public class ProjectWriteRepository : WriteRepository<Project>, IProjectWriteRepository
    {
        public ProjectWriteRepository(MensaDbContext context) : base(context)
        {
        }
    }
}
