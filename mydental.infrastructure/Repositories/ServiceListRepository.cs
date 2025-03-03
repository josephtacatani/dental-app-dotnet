using Microsoft.EntityFrameworkCore;
using mydental.domain.Entities;
using mydental.domain.Interfaces;
using mydental.infrastructure.Data;

namespace mydental.infrastructure.Repositories
{
    public class ServiceListRepository : IServiceListRepository
    {
        private readonly MyDentalDbContext _dbContext;

        public ServiceListRepository(MyDentalDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<ServiceList>> GetAllAsync()
        {
            return await _dbContext.ServiceLists.ToListAsync(); // ✅ Fixed DbSet name
        }

        public async Task<ServiceList?> GetByIdAsync(int id)
        {
            return await _dbContext.ServiceLists.FindAsync(id); // ✅ Fixed DbSet name
        }

        public async Task<bool> AddAsync(ServiceList serviceList)
        {
            await _dbContext.ServiceLists.AddAsync(serviceList); // ✅ Fixed DbSet name
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAsync(ServiceList serviceList)
        {
            var existingService = await _dbContext.ServiceLists.FirstOrDefaultAsync(s => s.Id == serviceList.Id);
            if (existingService == null) return false;

            // ✅ Correctly update existing entity properties instead of replacing the object
            existingService.Update(serviceList.ServiceName, serviceList.Title, serviceList.Content, serviceList.Photo);

            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var serviceList = await _dbContext.ServiceLists.FindAsync(id); // ✅ Fixed DbSet name
            if (serviceList == null) return false;

            _dbContext.ServiceLists.Remove(serviceList);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
