using MensaApp.Application.Abstraction.Services;
using MensaApp.Application.Repository.ProjectRepo;
using MensaApp.Domain.Entities;
using System.Linq.Expressions;

namespace MensaApp.Infrastructure.Services.Managers
{
    public class ProjectManager : IProjectService
    {
        readonly IProjectReadRepository _projectReadRepository;
        readonly IProjectWriteRepository _projectWriteRepository;

        public ProjectManager(IProjectReadRepository projectReadRepository, IProjectWriteRepository projectWriteRepository)
        {
            _projectReadRepository = projectReadRepository;
            _projectWriteRepository = projectWriteRepository;
        }

        public async Task<bool> AddAsync(Project model)
        {
            await _projectWriteRepository.AddAsync(model);
            await _projectWriteRepository.SaveAsync();
            return true;
        }

        public async Task<bool> AddRangeAsync(List<Project> datas)
        {
            await _projectWriteRepository.AddRangeAsync(datas);
            await _projectWriteRepository.SaveAsync();
            return true;
        }

        public IQueryable<Project> GetAll(bool tracking = true)
        {
            return _projectReadRepository.GetAll(false);
        }

        public async Task<Project> GetByIdAsync(int id, bool tracking = true)
        {
            return await _projectReadRepository.GetByIdAsync(id, false);
        }

        public async Task<Project> GetByIdProjectAsync(int id, bool tracking = true)
        {
            return await _projectReadRepository.GetProjectIdAsync(id);
        }

        public async Task<Project> GetSingleAsync(Expression<Func<Project, bool>> method, bool tracing = true)
        {
            return await _projectReadRepository.GetSingleAsync(method, false);
        }

        public IQueryable<Project> GetWhere(Expression<Func<Project, bool>> method, bool tracking = true)
        {
            return _projectReadRepository.GetWhere(method, false);
        }

        public void Remove(Project model)
        {
            _projectWriteRepository.Remove(model);
        }

        public async Task<bool> RemoveAsync(int id)
        {
            await _projectWriteRepository.RemoveAsync(id);
            await _projectWriteRepository.SaveAsync();
            return true;
        }

        public void RemoveRange(List<Project> datas)
        {
            _projectWriteRepository.RemoveRange(datas);
            _projectWriteRepository.SaveAsync();
        }

        public async Task<int> SaveAsync()
            => await _projectWriteRepository.SaveAsync();

        public void Update(Project model)
        {
            _projectWriteRepository.Update(model);
            _projectWriteRepository.SaveAsync();
        }
    }
}
