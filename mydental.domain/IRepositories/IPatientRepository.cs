using System.Collections.Generic;
using System.Threading.Tasks;
using mydental.domain.Entities;

namespace mydental.domain.IRepositories
{
    public interface IPatientRepository
    {
        Task<IEnumerable<User>> GetAllPatientsAsync();
        Task<User> GetPatientByIdAsync(int id);
        Task<bool> AddPatientAsync(User patientUser);
        Task<bool> UpdatePatientAsync(User patientUser);
        Task<bool> DeletePatientAsync(int id);
    }
}
