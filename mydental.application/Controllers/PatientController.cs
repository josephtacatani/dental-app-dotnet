using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using mydental.application.Services.PatientServices;
using mydental.application.DTO.PatientDTO;
using mydental.application.Common;
using mydental.application.DTO;
using Microsoft.AspNetCore.Authorization;

namespace mydental.application.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")] // ✅ Set JSON response type globally
    public class PatientController : ControllerBase
    {
        private readonly IPatientService _patientService;

        public PatientController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        /// <summary>
        /// Get all patients.
        /// </summary>
        /// <returns>List of patients</returns>
        [HttpGet]
        [Authorize(Roles = "Admin")] // 🔐 ✅ Only Admins can access this endpoint
        [ProducesResponseType(typeof(ServiceResult<IEnumerable<PatientDto>>), 200)] // ✅ Success
        [ProducesResponseType(typeof(UnauthorizedResponseDto), 401)] // ✅ Unauthorized response
        public async Task<IActionResult> GetAllPatients()
        {
            var result = await _patientService.GetAllPatientsAsync();

            if (result.StatusCode == 404)
            {
                return NotFound(new ErrorResponseDto<IEnumerable<PatientDto>>(
                    result.StatusCode, result.Message, result.ErrorMessages));
            }

            return Ok(result);
        }

        /// <summary>
        /// Get a single patient by ID.
        /// </summary>
        /// <param name="id">Patient ID</param>
        /// <returns>Patient details</returns>
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")] // 🔐 ✅ Only Admins can access this endpoint
        [ProducesResponseType(typeof(ServiceResult<PatientDto>), 200)]  // ✅ Success
        [ProducesResponseType(404)]  // ✅ Not Found
        public async Task<IActionResult> GetPatientById(int id)
        {
            var result = await _patientService.GetPatientByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Update a patient's profile.
        /// </summary>
        /// <param name="id">Patient ID</param>
        /// <param name="dto">Updated patient information</param>
        /// <returns>Update status</returns>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")] // 🔐 ✅ Only Admins can access this endpoint
        [ProducesResponseType(typeof(ServiceResult<bool>), 200)]  // ✅ Success
        [ProducesResponseType(400)]  // ✅ Bad Request (Validation Errors)
        [ProducesResponseType(404)]  // ✅ Not Found
        public async Task<IActionResult> UpdatePatientProfile(int id, [FromBody] PatientUpdatePayloadDto dto)
        {
            var result = await _patientService.UpdatePatientProfileAsync(id, dto);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Delete a patient.
        /// </summary>
        /// <param name="id">Patient ID</param>
        /// <returns>Deletion status</returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")] // 🔐 ✅ Only Admins can access this endpoint
        [ProducesResponseType(typeof(ServiceResult<bool>), 200)]  // ✅ Success
        [ProducesResponseType(404)]  // ✅ Not Found
        public async Task<IActionResult> DeletePatient(int id)
        {
            var result = await _patientService.DeletePatientAsync(id);
            return StatusCode(result.StatusCode, result);
        }
    }
}
