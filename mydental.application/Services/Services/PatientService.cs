using FluentValidation;
using Microsoft.Extensions.Logging;
using mydental.application.Common;
using mydental.application.DTO.PatientDTO;
using mydental.domain.Entities;
using mydental.domain.IRepositories;



namespace mydental.application.Services.PatientServices
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _patientRepository;
        private readonly IValidator<PatientDto> _patientValidator;
        private readonly IValidator<PatientUpdatePayloadDto> _updateValidator;
        private readonly ILogger<PatientService> _logger;

        public PatientService(
            IPatientRepository patientRepository,
            IValidator<PatientDto> patientValidator,
            IValidator<PatientUpdatePayloadDto> updateValidator,
            ILogger<PatientService> logger)
        {
            _patientRepository = patientRepository;
            _patientValidator = patientValidator;
            _updateValidator = updateValidator;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves all patients from the database.
        /// </summary>
        public async Task<ServiceResult<IEnumerable<PatientDto>>> GetAllPatientsAsync()
        {
            var patients = await _patientRepository.GetAllPatientsAsync();

            if (patients == null || !patients.Any())
            {
                _logger.LogWarning("No patients found.");
                return ServiceResult<IEnumerable<PatientDto>>.NotFound(ErrorMessages.PatientsNotFound, new List<string> { "No patients found." });
            }

            var patientDtos = patients.Select(MapToDto).ToList();
            return ServiceResult<IEnumerable<PatientDto>>.Success(patientDtos, SuccessMessages.PatientsRetrieved);
        }

        /// <summary>
        /// Retrieves a single patient by ID.
        /// </summary>
        public async Task<ServiceResult<PatientDto>> GetPatientByIdAsync(int id)
        {
            var patient = await _patientRepository.GetPatientByIdAsync(id);
            if (patient == null)
            {
                _logger.LogWarning("Patient not found: {PatientId}", id);
                return ServiceResult<PatientDto>.NotFound(ErrorMessages.PatientNotFound, new List<string> { $"No patient found with ID {id}." });
            }

            return ServiceResult<PatientDto>.Success(MapToDto(patient), SuccessMessages.PatientRetrieved);
        }

        /// <summary>
        /// Updates the profile of an existing patient.
        /// </summary>
        public async Task<ServiceResult<bool>> UpdatePatientProfileAsync(int id, PatientUpdatePayloadDto patientUpdatePayload)
        {
            var validationResult = await _updateValidator.ValidateAsync(patientUpdatePayload);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                _logger.LogWarning("Validation failed for patient update: {Errors}", string.Join("; ", errors));
                return ServiceResult<bool>.BadRequest("Validation failed", errors);
            }

            var existingPatient = await _patientRepository.GetPatientByIdAsync(id);
            if (existingPatient == null)
            {
                _logger.LogWarning("Patient not found for update: {PatientId}", id);
                return ServiceResult<bool>.NotFound(ErrorMessages.PatientNotFound, new List<string> { $"No patient found with ID {id}." });
            }

            // Update patient details
            existingPatient.UpdateProfile(
                patientUpdatePayload.FullName,
                patientUpdatePayload.Address,
                patientUpdatePayload.BirthDate,
                patientUpdatePayload.Photo,
                patientUpdatePayload.Gender,
                patientUpdatePayload.ContactNumber
            );

            var updateResult = await _patientRepository.UpdatePatientAsync(existingPatient);
            return updateResult
                ? ServiceResult<bool>.Success(true, SuccessMessages.PatientUpdated)
                : ServiceResult<bool>.Error("Update failed", new List<string> { "Unable to update the patient profile." });
        }

        /// <summary>
        /// Deletes a patient by ID.
        /// </summary>
        public async Task<ServiceResult<bool>> DeletePatientAsync(int id)
        {
            var result = await _patientRepository.DeletePatientAsync(id);
            if (!result)
            {
                _logger.LogWarning("Failed to delete patient: {PatientId}", id);
                return ServiceResult<bool>.NotFound(ErrorMessages.PatientNotFound, new List<string> { "Deletion failed. Patient not found." });
            }

            return ServiceResult<bool>.Success(true, SuccessMessages.PatientDeleted);
        }

        /// <summary>
        /// Maps a User entity to a User.
        /// </summary>
        private static PatientDto MapToDto(User patient)
        {
            return new PatientDto(
                patient.Id,
                patient.FullName,
                patient.Address,
                patient.BirthDate,
                patient.Email,
                patient.Status,
                patient.Photo,
                patient.Gender,
                patient.ContactNumber,
                patient.CreatedAt,
                patient.UpdatedAt
            );
        }
    }
}
