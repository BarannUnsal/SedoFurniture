using MensaApp.Application.Abstraction.Services;
using MensaApp.Application.Repository.ContactRepo;
using MensaApp.Domain.Entities;
using System.Linq.Expressions;

namespace MensaApp.Infrastructure.Services.Managers
{
    public class ContactManager : IContactService
    {
        readonly IContactReadRepository _contactReadRepository;
        readonly IContactWriteRepository _contactWriteRepository;
        public ContactManager(IContactReadRepository contactReadRepository, IContactWriteRepository contactWriteRepository)
        {
            _contactReadRepository = contactReadRepository;
            _contactWriteRepository = contactWriteRepository;
        }

        public async Task<bool> AddAsync(Contact model)
        {
            await _contactWriteRepository.AddAsync(model);
            await _contactWriteRepository.SaveAsync();
            return true;
        }

        public async Task<bool> AddRangeAsync(List<Contact> datas)
        {
            await _contactWriteRepository.AddRangeAsync(datas);
            await _contactWriteRepository.SaveAsync();
            return true;
        }

        public IQueryable<Contact> GetAll(bool tracking = true)
        {
            return _contactReadRepository.GetAll(false);
        }

        public Task<Contact> GetByIdAsync(int id, bool tracking = true)
        {
            return _contactReadRepository.GetByIdAsync(id);
        }

        public async Task<Contact> GetSingleAsync(Expression<Func<Contact, bool>> method, bool tracing = true)
        {
            return await _contactReadRepository.GetSingleAsync(method);
        }

        public IQueryable<Contact> GetWhere(Expression<Func<Contact, bool>> method, bool tracking = true)
        {
            return _contactReadRepository.GetWhere(method);
        }

        public void Remove(Contact model)
        {
            _contactWriteRepository.Remove(model);
        }

        public async Task<bool> RemoveAsync(int id)
        {
            await _contactWriteRepository.RemoveAsync(id);
            await _contactWriteRepository.SaveAsync();
            return true;
        }

        public void RemoveRange(List<Contact> datas)
        {
            _contactWriteRepository.RemoveRange(datas);
            _contactWriteRepository.SaveAsync();
        }

        public async Task<int> SaveAsync()
        {
            return await _contactWriteRepository.SaveAsync();
        }

        public void Update(Contact model)
        {
            _contactWriteRepository.Update(model);
            _contactWriteRepository.SaveAsync();
        }
    }
}
