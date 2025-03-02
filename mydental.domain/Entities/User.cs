using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using mydental.domain.Helpers;
using mydental.domain.Constants;
using System.Security.Cryptography;

namespace mydental.domain.Entities
{
    public class User
    {
        public int Id { get; private set; }
        public string Email { get; private set; }
        public string PasswordHash { get; private set; }
        public string PasswordSalt { get; private set; } // ✅ New Column for salting passwords
        public string Role { get; private set; }
        public string Status { get; private set; }
        public string FullName { get; private set; }
        public string? Photo { get; private set; }
        public string Address { get; private set; }
        public string Gender { get; private set; }
        public string ContactNumber { get; private set; }
        public bool EmailVerified { get; private set; }
        public string? RefreshToken { get; private set; }

        private DateTime? _refreshTokenExpiry;
        public DateTime? RefreshTokenExpiry => _refreshTokenExpiry; // ✅ Proper encapsulation

        private DateTime? _birthDate;
        public string? BirthDate => _birthDate?.ToString("yyyy-MM-dd"); // ✅ Formats for API responses

        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        // ✅ EF Core requires a parameterless constructor
        private User() { }

        public User(
            string email,
            string password,
            string role,
            string fullName,
            string birthDate,
            string address,
            string gender,
            string contactNumber)
        {
            Email = !string.IsNullOrWhiteSpace(email) ? email.ToLower().Trim() : throw new ArgumentException(UserValidationConstants.EmailRequired);

            // ✅ Secure password hashing with salt
            (PasswordHash, PasswordSalt) = PasswordHelper.CreateHashedPassword(password);

            Role = !string.IsNullOrWhiteSpace(role) ? role : throw new ArgumentException(UserValidationConstants.RoleRequired);
            FullName = !string.IsNullOrWhiteSpace(fullName) ? fullName : throw new ArgumentException(UserValidationConstants.FullNameRequired);

            _birthDate = DateHelper.ParseDate(birthDate) ?? throw new ArgumentException("Invalid date format. Use yyyy-MM-dd.");
            if (_birthDate >= DateTime.UtcNow)
                throw new ArgumentException(UserValidationConstants.BirthDateInvalid);

            Address = address;
            Gender = gender;
            ContactNumber = contactNumber;
            Status = "inactive";
            EmailVerified = false;
            RefreshToken = null;
            _refreshTokenExpiry = null;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = CreatedAt;
        }

        /// <summary>
        /// ✅ Verifies the password using the stored hash & salt.
        /// </summary>
        public bool VerifyPassword(string password)
        {
            return PasswordHelper.VerifyPassword(password, PasswordHash, PasswordSalt);
        }

        private void EnsureValidState()
        {
            if (string.IsNullOrWhiteSpace(Email))
                throw new ArgumentException(UserValidationConstants.EmailRequired);

            if (string.IsNullOrWhiteSpace(FullName))
                throw new ArgumentException(UserValidationConstants.FullNameRequired);

            if (!_birthDate.HasValue || _birthDate >= DateTime.UtcNow)
                throw new ArgumentException(UserValidationConstants.BirthDateInvalid);
        }

        public void ChangeRole(string newRole)
        {
            if (string.IsNullOrWhiteSpace(newRole))
                throw new ArgumentException(UserValidationConstants.RoleInvalid);

            Role = newRole;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateProfile(string fullName, string address, string birthDate, string photo, string gender, string contactNumber)
        {
            if (string.IsNullOrWhiteSpace(fullName))
                throw new ArgumentException(UserValidationConstants.FullNameRequired);

            DateTime parsedBirthDate = DateHelper.ParseDate(birthDate) ?? throw new ArgumentException(UserValidationConstants.BirthDateInvalid);
            if (parsedBirthDate >= DateTime.UtcNow)
                throw new ArgumentException(UserValidationConstants.BirthDateInvalid);

            FullName = fullName;
            Address = address;
            _birthDate = parsedBirthDate;
            Photo = photo;
            Gender = gender;
            ContactNumber = contactNumber;
            UpdatedAt = DateTime.UtcNow;

            EnsureValidState();
        }

        public void VerifyEmail()
        {
            EmailVerified = true;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdatePassword(string newPassword)
        {
            if (string.IsNullOrWhiteSpace(newPassword))
                throw new ArgumentException(UserValidationConstants.NewPasswordRequired);

            (PasswordHash, PasswordSalt) = PasswordHelper.CreateHashedPassword(newPassword);
            UpdatedAt = DateTime.UtcNow;
        }

        public void ChangeStatus(string status)
        {
            if (string.IsNullOrWhiteSpace(status))
                throw new ArgumentException(UserValidationConstants.StatusRequired);

            Status = status;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateRefreshToken(string newToken, DateTime? expiry)
        {
            RefreshToken = newToken;
            _refreshTokenExpiry = expiry;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password cannot be empty.");

            (PasswordHash, PasswordSalt) = PasswordHelper.CreateHashedPassword(password);
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email cannot be empty.");

            Email = email.ToLower().Trim();
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
