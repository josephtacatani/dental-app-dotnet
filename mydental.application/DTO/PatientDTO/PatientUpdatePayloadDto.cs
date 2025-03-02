namespace mydental.application.DTO.PatientDTO
{
    public class PatientUpdatePayloadDto
    {
        public string FullName { get; private set; }
        public string Address { get; private set; }
        public string BirthDate { get; private set; }
        public string Email { get; private set; }
        public string Status { get; private set; }
        public string Photo { get; private set; }
        public string Gender { get; private set; }
        public string ContactNumber { get; private set; }
    }
}
