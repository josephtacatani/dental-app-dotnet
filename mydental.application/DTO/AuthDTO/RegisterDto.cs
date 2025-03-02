using mydental.domain.Helpers;
using System;

namespace mydental.application.DTO.AuthDTO
{
    public class RegisterDto
    {
        public int Id { get; private set; }
        public string Email { get; private set; }
        public string Password { get; private set; } // ✅ Only store password input, not hash
        public string Role { get; private set; } // ✅ Default to "Patient"
        public string Status { get; private set; } // ✅ Default to "Inactive"
        public string FullName { get; private set; }
        public string? Photo { get; private set; } // ✅ Default to null
        public string BirthDate { get; private set; }
        public string Address { get; private set; }
        public string Gender { get; private set; }
        public string ContactNumber { get; private set; }
        public bool EmailVerified { get; private set; } // ✅ Default to false
        public string? RefreshToken { get; private set; } // ✅ Default to null
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }


        public RegisterDto(
            string email,
            string password,
            string fullName,
            string birthDate,
            string address,
            string gender,
            string contactNumber)
        {
            Id = Id;
            Email = email;
            Password = password; // ✅ Store only raw password here (hashed in User entity)
            Role = "Patient"; // ✅ Set default role
            Status = "Inactive"; // ✅ Default status is inactive
            FullName = fullName;
            Photo = null; // ✅ Default to null
            BirthDate = birthDate;
            Address = address;
            Gender = gender;
            ContactNumber = contactNumber;
            EmailVerified = false; // ✅ Default to false
            RefreshToken = null; // ✅ Default to null
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
