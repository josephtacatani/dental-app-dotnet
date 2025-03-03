using FluentValidation;
using Microsoft.Extensions.Logging;
using mydental.application.Common;
using mydental.application.DTO.ServiceListDTO;
using mydental.domain.Entities;
using mydental.domain.Interfaces;

namespace mydental.application.Services.ServiceListServices
{
    public class ServiceListService : IServiceListService
    {
        private readonly IServiceListRepository _serviceListRepository;
        private readonly IValidator<CreateServiceListDto> _createServiceListValidator;
        private readonly IValidator<ServiceListDto> _serviceListValidator;
        private readonly ILogger<ServiceListService> _logger;

        public ServiceListService(
            IServiceListRepository serviceListRepository,
            IValidator<CreateServiceListDto> createServiceListValidator,
            IValidator<ServiceListDto> serviceListValidator,
            ILogger<ServiceListService> logger)
        {
            _serviceListRepository = serviceListRepository;
            _createServiceListValidator = createServiceListValidator;
            _serviceListValidator = serviceListValidator;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves all services from the database.
        /// </summary>
        public async Task<ServiceResult<IEnumerable<ServiceListDto>>> GetAllServicesAsync()
        {
            var services = await _serviceListRepository.GetAllAsync();

            if (services == null || !services.Any())
            {
                _logger.LogWarning("No services found.");
                return ServiceResult<IEnumerable<ServiceListDto>>.NotFound("No services found.", new List<string> { "No services found." });
            }

            var serviceDtos = services.Select(MapToDto).ToList();
            return ServiceResult<IEnumerable<ServiceListDto>>.Success(serviceDtos, "Services retrieved successfully.");
        }

        /// <summary>
        /// Retrieves a single service by ID.
        /// </summary>
        public async Task<ServiceResult<ServiceListDto>> GetServiceByIdAsync(int id)
        {
            var service = await _serviceListRepository.GetByIdAsync(id);
            if (service == null)
            {
                _logger.LogWarning("Service not found: {ServiceId}", id);
                return ServiceResult<ServiceListDto>.NotFound("Service not found.", new List<string> { $"No service found with ID {id}." });
            }

            return ServiceResult<ServiceListDto>.Success(MapToDto(service), "Service retrieved successfully.");
        }

        /// <summary>
        /// Adds a new service.
        /// </summary>
        public async Task<ServiceResult<bool>> AddServiceAsync(CreateServiceListDto createService)
        {
            _logger.LogInformation("Adding new service: {ServiceName}", createService.ServiceName);

            // ✅ Step 1: Validate the DTO
            var validationResult = await _createServiceListValidator.ValidateAsync(createService);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                _logger.LogWarning("Validation failed for new service: {Errors}", string.Join("; ", errors));
                return ServiceResult<bool>.BadRequest("Validation failed", errors);
            }

            // ✅ Step 2: Check for duplicate service names (Optional)
            var existingService = await _serviceListRepository.GetAllAsync();
            if (existingService.Any(s => s.ServiceName == createService.ServiceName))
            {
                _logger.LogWarning("Service creation failed. Service name already exists: {ServiceName}", createService.ServiceName);
                return ServiceResult<bool>.Conflict("Service name already exists", new List<string> { "A service with this name already exists." });
            }

            // ✅ Step 3: Create the new service entity using constructor
            var service = new ServiceList(
                createService.ServiceName,
                createService.Title,
                createService.Content,
                createService.Photo
            );

            // ✅ Step 4: Save to repository
            var result = await _serviceListRepository.AddAsync(service);
            if (!result)
            {
                _logger.LogError("Service creation failed for {ServiceName}", createService.ServiceName);
                return ServiceResult<bool>.Error("Creation failed", new List<string> { "Could not create the service." });
            }

            _logger.LogInformation("Service {ServiceName} created successfully", service.ServiceName);
            return ServiceResult<bool>.Success(true, "Service added successfully.");
        }



        /// <summary>
        /// Updates an existing service.
        /// </summary>
        public async Task<ServiceResult<bool>> UpdateServiceAsync(int id, ServiceListDto serviceListDto)
        {
            _logger.LogInformation("Updating service: {ServiceId}", id);

            var validationResult = await _serviceListValidator.ValidateAsync(serviceListDto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                _logger.LogWarning("Validation failed for service update: {Errors}", string.Join("; ", errors));
                return ServiceResult<bool>.BadRequest("Validation failed", errors);
            }

            var existingService = await _serviceListRepository.GetByIdAsync(id);
            if (existingService == null)
            {
                _logger.LogWarning("Service not found for update: {ServiceId}", id);
                return ServiceResult<bool>.NotFound("Service not found.", new List<string> { $"No service found with ID {id}." });
            }

            // Update service details
            existingService.Update(serviceListDto.ServiceName, serviceListDto.Title, serviceListDto.Content, serviceListDto.Photo);

            var updateResult = await _serviceListRepository.UpdateAsync(existingService);
            if (!updateResult)
            {
                _logger.LogError("Failed to update service: {ServiceId}", id);
                return ServiceResult<bool>.Error("Update failed", new List<string> { "Unable to update the service." });
            }

            _logger.LogInformation("Service updated successfully: {ServiceId}", id);
            return ServiceResult<bool>.Success(true, "Service updated successfully.");
        }

        /// <summary>
        /// Deletes a service by ID.
        /// </summary>
        public async Task<ServiceResult<bool>> DeleteServiceAsync(int id)
        {
            _logger.LogInformation("Deleting service: {ServiceId}", id);

            var result = await _serviceListRepository.DeleteAsync(id);
            if (!result)
            {
                _logger.LogWarning("Failed to delete service: {ServiceId}", id);
                return ServiceResult<bool>.NotFound("Service not found.", new List<string> { "Deletion failed. Service not found." });
            }

            _logger.LogInformation("Service deleted successfully: {ServiceId}", id);
            return ServiceResult<bool>.Success(true, "Service deleted successfully.");
        }

        /// <summary>
        /// Maps a ServiceList entity to a ServiceListDto.
        /// </summary>
        private static ServiceListDto MapToDto(ServiceList service)
        {
            return new ServiceListDto(
                id: service.Id,
                serviceName: service.ServiceName,
                title: service.Title,
                content: service.Content,
                photo: service.Photo,
                createdAt: service.CreatedAt,
                updatedAt: service.UpdatedAt
            );
        }
    }
}
