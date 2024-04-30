using Monitor.SPA.Models.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Monitor.SPA.Repository.Interface
{
    public interface IGenericRepository<T> where T : IBaseEntity
    {
        Task<T> CreateAsync(T entity);
        Task<T> ReadAsync(string id, long subscriptionId);
        Task<IEnumerable<T>> ReadListAsync(long subscriptionId);
        Task<T> UpdateAsync(T entity);
        Task DeleteAsync(T entity);
    }
}
