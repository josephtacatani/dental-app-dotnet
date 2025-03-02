namespace mydental.application.Common
{
    public static class ErrorMessages
    {
        // ❌ 400 Bad Request Errors
        public const string InvalidRequest = "Invalid request format.";
        public const string InvalidPatientData = "Invalid patient data.";
        public const string RequiredFieldsMissing = "Required fields are missing.";

        // ❌ 404 Not Found Errors
        public const string PatientNotFound = "No patient found with the given ID.";
        public const string PatientsNotFound = "No patients found.";

        // ❌ 409 Conflict Errors
        public const string PatientAlreadyExists = "A patient with this email already exists.";
        public const string EmailAlreadyTaken = "Email is already in use.";

        // ❌ 500 Internal Server Errors
        public const string GeneralServerError = "An unexpected error occurred.";
        public const string DatabaseError = "An unexpected database error occurred.";
    }
}
