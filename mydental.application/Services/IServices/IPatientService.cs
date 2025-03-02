using mydental.application.Common;
using mydental.application.DTO.PatientDTO;

namespace mydental.application.Services.PatientServices
{
    public interface IPatientService
    {
        Task<ServiceResult<IEnumerable<PatientDto>>> GetAllPatientsAsync();
        Task<ServiceResult<PatientDto>> GetPatientByIdAsync(int id);
        Task<ServiceResult<bool>> UpdatePatientProfileAsync(int id, PatientUpdatePayloadDto patientUpdatePayload);
        Task<ServiceResult<bool>> DeletePatientAsync(int id);
    }
}
