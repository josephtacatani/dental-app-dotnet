using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using mydental.application.Services.Services;
using mydental.domain.Entities;
using mydental.domain.Helpers;
using mydental.domain.IRepositories;
using mydental.infrastructure.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace mydental.infrastructure.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly MyDentalDbContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthRepository> _logger;

        public AuthRepository(
            MyDentalDbContext dbContext, 
            IConfiguration configuration,
            ILogger<AuthRepository> logger
            )
        {
            _dbContext = dbContext;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<User?> RegisterAsync(User patientUser)
        {
            patientUser.SetEmail(patientUser.Email.Trim().ToLower()); // ✅ Remove spaces before storing

            await _dbContext.Users.AddAsync(patientUser);
            var result = await _dbContext.SaveChangesAsync();
            return result > 0 ? patientUser : null;
        }



        public async Task<string?> LoginAsync(string email, string password)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null || !user.VerifyPassword(password)) { return null; }

            return GenerateJwtToken(user);
        }

        public async Task<(string AccessToken, string RefreshToken)?> RefreshTokenAsync(string refreshToken)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
            if (user == null || user.RefreshTokenExpiry < DateTime.UtcNow) // ✅ Expiry check
            {
                return null;
            }

            string newAccessToken = GenerateJwtToken(user);
            string newRefreshToken = GenerateRefreshToken();

            // ✅ Now updates both token and expiry
            user.UpdateRefreshToken(newRefreshToken, DateTime.UtcNow.AddDays(7)); // Expires in 7 days
            await _dbContext.SaveChangesAsync();

            return (newAccessToken, newRefreshToken);
        }


        private string GenerateJwtToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateRefreshToken()
        {
            return Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        }

        public async Task<User?> FindByRefreshTokenAsync(string refreshToken)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            _dbContext.Users.Update(user);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<User?> FindByEmailAsync(string email)
        {
            _logger.LogInformation("Searching user by email: {Email}", email);

            var user = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Email.Trim().ToLower() == email.Trim().ToLower()); // ✅ Fixes issue

            if (user == null)
            {
                _logger.LogWarning("No user found in the database with email: {Email}", email);
            }

            return user;
        }






    }
}
