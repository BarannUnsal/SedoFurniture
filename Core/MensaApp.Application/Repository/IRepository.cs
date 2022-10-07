using MensaApp.Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;

namespace MensaApp.Application.Repository
{
    public interface IRepository<T> where T : BaseEntity
    {
        DbSet<T> Table { get; }
    }
}
