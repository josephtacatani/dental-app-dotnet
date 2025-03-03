using System.Collections.Generic;
using System.Threading.Tasks;
using mydental.domain.Entities;

namespace mydental.domain.Interfaces
{
    public interface IServiceListRepository
    {
        Task<IEnumerable<ServiceList>> GetAllAsync();
        Task<ServiceList?> GetByIdAsync(int id);
        Task<bool> AddAsync(ServiceList serviceList);
        Task<bool> UpdateAsync(ServiceList serviceList);
        Task<bool> DeleteAsync(int id);
    }
}
