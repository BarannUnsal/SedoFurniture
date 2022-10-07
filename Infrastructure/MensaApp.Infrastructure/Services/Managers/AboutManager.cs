using MensaApp.Application.Abstraction.Services;
using MensaApp.Application.Repository.AboutRepo;
using MensaApp.Domain.Entities;
using System.Linq.Expressions;

namespace MensaApp.Infrastructure.Services.Managers
{
    public class AboutManager : IAboutService
    {
        readonly IAboutWriteRepository _aboutWriteRepository;
        readonly IAboutReadRepository _aboutReadRepository;
        public AboutManager(IAboutWriteRepository aboutWriteRepository, IAboutReadRepository aboutReadRepository)
        {
            _aboutWriteRepository = aboutWriteRepository;
            _aboutReadRepository = aboutReadRepository;
        }

        public async Task<bool> AddAsync(About model)
        {
            await _aboutWriteRepository.AddAsync(model);
            await _aboutWriteRepository.SaveAsync();
            return true;
        }

        public async Task<bool> AddRangeAsync(List<About> datas)
        {
            await _aboutWriteRepository.AddRangeAsync(datas);
            await _aboutWriteRepository.SaveAsync();
            return true;
        }

        public IQueryable<About> GetAll(bool tracking = true)
        {
            return _aboutReadRepository.GetAll(false);
        }

        public async Task<About> GetByIdAsync(int id, bool tracking = true)
        {
            return await _aboutReadRepository.GetByIdAsync(id);
        }

        public async Task<About> GetSingleAsync(Expression<Func<About, bool>> method, bool tracing = true)
        {
            return await _aboutReadRepository.GetSingleAsync(method);
        }

        public IQueryable<About> GetWhere(Expression<Func<About, bool>> method, bool tracking = true)
        {
            return _aboutReadRepository.GetWhere(method);
        }

        public void Remove(About model)
        {
            _aboutWriteRepository.Remove(model);
        }

        public async Task<bool> RemoveAsync(int id)
        {
            await _aboutWriteRepository.RemoveAsync(id);
            await _aboutWriteRepository.SaveAsync();
            return true;
        }

        public void RemoveRange(List<About> datas)
        {
            _aboutWriteRepository.RemoveRange(datas);
            _aboutWriteRepository.SaveAsync();
        }

        public async Task<int> SaveAsync()
        {
            return await _aboutWriteRepository.SaveAsync();
        }

        public void Update(About model)
        {
            _aboutWriteRepository.Update(model);
            _aboutWriteRepository.SaveAsync();
        }
    }
}
