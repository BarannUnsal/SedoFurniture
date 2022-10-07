using MensaApp.Domain.Entities.Common;
using System.Linq.Expressions;

namespace MensaApp.Application.Abstraction.Services
{
    public interface IGenericService<T> where T : BaseEntity
    {
        Task<bool> AddAsync(T model);
        Task<bool> AddRangeAsync(List<T> datas);
        void Remove(T model);
        void RemoveRange(List<T> datas);
        Task<bool> RemoveAsync(int id);
        void Update(T model);
        Task<int> SaveAsync();
        IQueryable<T> GetAll(bool tracking = true);
        IQueryable<T> GetWhere(Expression<Func<T, bool>> method, bool tracking = true);
        Task<T> GetByIdAsync(int id, bool tracking = true);
        Task<T> GetSingleAsync(Expression<Func<T, bool>> method, bool tracing = true);
    }
}
