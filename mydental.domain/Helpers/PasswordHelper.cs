using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Security.Cryptography;

public static class PasswordHelper
{
    /// <summary>
    /// Hashes a password using PBKDF2 and the provided salt.
    /// </summary>
    public static string HashPassword(string password, byte[] salt)
    {
        return Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 10000,
            numBytesRequested: 32));
    }

    /// <summary>
    /// Generates a secure hashed password with a random salt.
    /// </summary>
    public static (string HashedPassword, string Salt) CreateHashedPassword(string password)
    {
        byte[] salt = new byte[16];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }

        string hashedPassword = HashPassword(password, salt);
        return (hashedPassword, Convert.ToBase64String(salt));
    }

    /// <summary>
    /// ✅ Verifies a password by hashing the input and comparing it to the stored hash.
    /// </summary>
    public static bool VerifyPassword(string password, string storedHash, string storedSalt)
    {
        byte[] saltBytes = Convert.FromBase64String(storedSalt); // Convert stored salt back to bytes
        string hashedInput = HashPassword(password, saltBytes);  // Hash the input password with the same salt
        return hashedInput == storedHash; // Compare hashes
    }
}
