namespace mydental.application.DTO.PatientDTO
{
    public class ErrorResponseDto<T>
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public List<string>? ErrorMessages { get; set; }
        public T? Data { get; set; } // ✅ Ensure data is always present (null for errors)

        public ErrorResponseDto(int statusCode, string message, List<string>? errorMessages = null)
        {
            StatusCode = statusCode;
            Message = message;
            ErrorMessages = errorMessages ?? new List<string>();
            Data = default; // ✅ Ensure `data` is always included as null in error responses
        }
    }
}
