namespace mydental.application.DTO
{
    public class UnauthorizedResponseDto
    {
        public int StatusCode { get; set; } = 401;
        public string Message { get; set; } = "Unauthorized";
        public List<string>? ErrorMessages { get; set; }

        public UnauthorizedResponseDto()
        {
            ErrorMessages = new List<string> { "Access token is missing or invalid." };
        }
    }
}
