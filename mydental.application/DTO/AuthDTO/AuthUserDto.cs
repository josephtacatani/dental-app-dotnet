namespace mydental.application.DTO.AuthDTO
{
    public class AuthUserDto
    {
        public int Id { get; private set; }
        public string Email { get; private set; }
        public string Role { get; private set; }
        public string Status { get; private set; }
        public string FullName { get; private set; }
        public string? Photo { get; private set; }
        public string BirthDate { get; private set; } // ✅ Use DateTime?
        public string Address { get; private set; }
        public string Gender { get; private set; }
        public string ContactNumber { get; private set; }
        public bool EmailVerified { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        public AuthUserDto(
            int id,
            string email,
            string role,
            string status,
            string fullName,
            string? photo,
            string birthDate, // ✅ Use DateTime? (JSON converter will format it)
            string address,
            string gender,
            string contactNumber,
            bool emailVerified,
            DateTime createdAt,
            DateTime updatedAt)
        {
            Id = id;
            Email = email;
            Role = role;
            Status = status;
            FullName = fullName;
            Photo = photo;
            BirthDate = birthDate; // ✅ No manual formatting needed
            Address = address;
            Gender = gender;
            ContactNumber = contactNumber;
            EmailVerified = emailVerified;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }
    }
}
