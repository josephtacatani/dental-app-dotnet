namespace mydental.application.DTO.PatientDTO
{
    public class PatientDto
    {
        public int Id { get; private set; }
        public string FullName { get; private set; }
        public string Address { get; private set; }
        public string BirthDate { get; private set; }
        public string Email { get; private set; }
        public string Status { get; private set; }
        public string Photo { get; private set; }
        public string Gender { get; private set; }
        public string ContactNumber { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        // ✅ Constructor-based assignment (Immutable DTO)
        public PatientDto(int id, string fullName, string address, string birthDate, string email,
                          string status, string photo, string gender, string contactNumber,
                          DateTime createdAt, DateTime updatedAt)
        {
            Id = id;
            FullName = fullName;
            Address = address;
            BirthDate = birthDate;
            Email = email;
            Status = status;
            Photo = photo;
            Gender = gender;
            ContactNumber = contactNumber;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }

        
    }

}
