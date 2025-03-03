using mydental.application.Common;
using mydental.application.DTO;
using mydental.application.DTO.ServiceListDTO;
using mydental.domain.Entities;

namespace mydental.application.Services.ServiceListServices
{
    public interface IServiceListService
    {
        Task<ServiceResult<IEnumerable<ServiceListDto>>> GetAllServicesAsync();
        Task<ServiceResult<ServiceListDto>> GetServiceByIdAsync(int id);
        Task<ServiceResult<bool>> AddServiceAsync(CreateServiceListDto createService);
        Task<ServiceResult<bool>> UpdateServiceAsync(int id, ServiceListDto serviceListDto);
        Task<ServiceResult<bool>> DeleteServiceAsync(int id);
    }
}
