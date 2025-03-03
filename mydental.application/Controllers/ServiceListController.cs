using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using mydental.application.Services.ServiceListServices;
using mydental.application.DTO;
using mydental.application.Common;
using Microsoft.AspNetCore.Authorization;
using mydental.application.DTO.PatientDTO;
using mydental.application.DTO.ServiceListDTO;

namespace mydental.application.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")] // ✅ Set JSON response type globally
    public class ServiceListController : ControllerBase
    {
        private readonly IServiceListService _serviceListService;

        public ServiceListController(IServiceListService serviceListService)
        {
            _serviceListService = serviceListService;
        }

        /// <summary>
        /// Get all services.
        /// </summary>
        /// <returns>List of services</returns>
        [HttpGet]
        [Authorize(Roles = "Admin")] // 🔐 ✅ Only Admins can create services
        [ProducesResponseType(typeof(ServiceResult<IEnumerable<ServiceListDto>>), 200)] // ✅ Success
        [ProducesResponseType(404)]  // ✅ Not Found
        public async Task<IActionResult> GetAllServices()
        {
            var result = await _serviceListService.GetAllServicesAsync();

            if (result.StatusCode == 404)
            {
                return NotFound(new ErrorResponseDto<IEnumerable<ServiceListDto>>(
                    result.StatusCode, result.Message, result.ErrorMessages));
            }

            return Ok(result);
        }

        /// <summary>
        /// Get a single service by ID.
        /// </summary>
        /// <param name="id">Service ID</param>
        /// <returns>Service details</returns>
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")] // 🔐 ✅ Only Admins can create services
        [ProducesResponseType(typeof(ServiceResult<ServiceListDto>), 200)]  // ✅ Success
        [ProducesResponseType(404)]  // ✅ Not Found
        public async Task<IActionResult> GetServiceById(int id)
        {
            var result = await _serviceListService.GetServiceByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Add a new service.
        /// </summary>
        /// <param name="createService">Service data</param>
        /// <returns>Creation status</returns>
        [HttpPost]
        [Authorize(Roles = "Admin")] // 🔐 ✅ Only Admins can create services
        [ProducesResponseType(typeof(ServiceResult<CreateServiceListDto>), 200)]
        [ProducesResponseType(400)]  // ✅ Bad Request (Validation Errors)

        public async Task<IActionResult> AddService([FromBody] CreateServiceListDto createService)
        {
            var result = await _serviceListService.AddServiceAsync(createService);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Update a service.
        /// </summary>
        /// <param name="id">Service ID</param>
        /// <param name="dto">Updated service information</param>
        /// <returns>Update status</returns>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")] // 🔐 ✅ Only Admins can update services
        [ProducesResponseType(typeof(ServiceResult<bool>), 200)]  // ✅ Success
        [ProducesResponseType(400)]  // ✅ Bad Request (Validation Errors)
        [ProducesResponseType(404)]  // ✅ Not Found
        public async Task<IActionResult> UpdateService(int id, [FromBody] ServiceListDto dto)
        {
            var result = await _serviceListService.UpdateServiceAsync(id, dto);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Delete a service.
        /// </summary>
        /// <param name="id">Service ID</param>
        /// <returns>Deletion status</returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")] // 🔐 ✅ Only Admins can delete services
        [ProducesResponseType(typeof(ServiceResult<bool>), 200)]  // ✅ Success
        [ProducesResponseType(404)]  // ✅ Not Found
        public async Task<IActionResult> DeleteService(int id)
        {
            var result = await _serviceListService.DeleteServiceAsync(id);
            return StatusCode(result.StatusCode, result);
        }
    }
}
