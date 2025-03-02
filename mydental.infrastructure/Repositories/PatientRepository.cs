using Microsoft.EntityFrameworkCore;
using mydental.domain.Common;
using mydental.domain.Entities;
using mydental.domain.IRepositories;
using mydental.infrastructure.Data;

namespace mydental.infrastructure.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        private readonly MyDentalDbContext _dbContext;

        public PatientRepository(MyDentalDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<User>> GetAllPatientsAsync()
        {
            return await _dbContext.Users
                .Where(u => u.Role == UserRoles.Patient)
                .ToListAsync();
        }

        public async Task<User?> GetPatientByIdAsync(int id)
        {
            return await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Id == id && u.Role == UserRoles.Patient);
        }

        public async Task<bool> AddPatientAsync(User patientUser)
        {
            patientUser.ChangeRole(UserRoles.Patient); // ✅ Ensure the role is "Patient"
            await _dbContext.Users.AddAsync(patientUser);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdatePatientAsync(User patientUser)
        {
            var existingUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == patientUser.Id && u.Role == UserRoles.Patient);
            if (existingUser == null) return false;

            existingUser.UpdateProfile(patientUser.FullName, patientUser.Address, patientUser.BirthDate, patientUser.Photo, patientUser.Gender, patientUser.ContactNumber);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeletePatientAsync(int id)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id && u.Role == UserRoles.Patient);
            if (user == null) return false;

            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync();
            return true;
        }



    }

}
