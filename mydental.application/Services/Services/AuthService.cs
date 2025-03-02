using FluentValidation;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using mydental.application.Common;
using mydental.application.DTO.AuthDTO;
using mydental.application.Helpers;
using mydental.application.Services.IServices;
using mydental.domain.Constants;
using mydental.domain.Entities;
using mydental.domain.Helpers;
using mydental.domain.IRepositories;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace mydental.application.Services.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IValidator<RegisterDto> _registerValidator;
        private readonly IValidator<LoginDto> _loginValidator;
        private readonly ILogger<AuthService> _logger;
        private readonly JwtHelper _jwtHelper;

        public AuthService(
            IAuthRepository authRepository,
            IValidator<RegisterDto> registerValidator,
            IValidator<LoginDto> loginValidator,
            ILogger<AuthService> logger,
            JwtHelper jwtHelper)
        {
            _authRepository = authRepository;
            _registerValidator = registerValidator;
            _loginValidator = loginValidator;
            _logger = logger;
            _jwtHelper = jwtHelper;
        }

        /// <summary>
        /// Authenticates a user and returns a JWT token.
        /// </summary>
        public async Task<ServiceResult<AuthTokenDto>> LoginAsync(LoginDto loginDto)
        {
            _logger.LogInformation("Login attempt for email: {Email}", loginDto.Email);

            var user = await _authRepository.FindByEmailAsync(loginDto.Email);
            if (user == null || !user.VerifyPassword(loginDto.Password)) // ✅ Now uses VerifyPassword
            {
                _logger.LogWarning("Unauthorized login attempt for {Email}", loginDto.Email);
                return ServiceResult<AuthTokenDto>.BadRequest("Bad Request", new List<string> { "Invalid email or password." });
            }

            string accessToken = _jwtHelper.GenerateJwtToken(user);
            string refreshToken = _jwtHelper.GenerateRefreshToken();

            user.UpdateRefreshToken(refreshToken, DateTime.UtcNow.AddDays(7));
            await _authRepository.UpdateUserAsync(user);

            _logger.LogInformation("User {Email} logged in successfully", user.Email);

            return ServiceResult<AuthTokenDto>.Success(new AuthTokenDto(accessToken, refreshToken), "Login successful.");
        }



        /// <summary>
        /// Registers a new user in the system.
        /// </summary>
        public async Task<ServiceResult<AuthUserDto>> RegisterAsync(RegisterDto registerDto)
        {
            _logger.LogInformation("Registering new user: {Email}", registerDto.Email); // ✅ Logs registration attempts

            var validationResult = await _registerValidator.ValidateAsync(registerDto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                _logger.LogWarning("User registration failed for {Email}: {Errors}", registerDto.Email, string.Join(", ", errors));
                return ServiceResult<AuthUserDto>.BadRequest("Validation failed", errors);
            }

            // ✅ Step 1: Check if email already exists
            var existingUser = await _authRepository.FindByEmailAsync(registerDto.Email);
            if (existingUser != null)
            {
                _logger.LogWarning("Registration failed. Email already exists: {Email}", registerDto.Email);
                return ServiceResult<AuthUserDto>.Conflict("Email already exists", new List<string> { "An account with this email already exists." });
            }


            var user = new User(registerDto.Email, registerDto.Password, "Patient", registerDto.FullName, registerDto.BirthDate, registerDto.Address, registerDto.Gender, registerDto.ContactNumber);
            var result = await _authRepository.RegisterAsync(user);

            if (result == null)
            {
                _logger.LogError("User registration failed for {Email}", registerDto.Email);
                return ServiceResult<AuthUserDto>.Error("Registration failed", new List<string> { "Could not create user." });
            }

            _logger.LogInformation("User {Email} registered successfully", user.Email); // ✅ Logs successful registration

            return ServiceResult<AuthUserDto>.Success(MapToDto(user), "User registered successfully.");
        }

        /// <summary>
        /// Refreshes a user's authentication token.
        /// </summary>
        public async Task<ServiceResult<AuthTokenDto>> RefreshTokenAsync(string refreshToken)
        {
            _logger.LogInformation("Refreshing token for provided refresh token.");

            var user = await _authRepository.FindByRefreshTokenAsync(refreshToken);
            if (user == null || user.RefreshTokenExpiry < DateTime.UtcNow)
            {
                _logger.LogWarning("Invalid or expired refresh token.");
                return ServiceResult<AuthTokenDto>.BadRequest("Bad request", new List<string> { "Invalid or expired refresh token." });
            }

            string newAccessToken = _jwtHelper.GenerateJwtToken(user);
            string newRefreshToken = _jwtHelper.GenerateRefreshToken();

            user.UpdateRefreshToken(newRefreshToken, DateTime.UtcNow.AddDays(7));
            await _authRepository.UpdateUserAsync(user);

            _logger.LogInformation("User {Email} successfully refreshed token.", user.Email); // ✅ Logs token refresh success

            return ServiceResult<AuthTokenDto>.Success(new AuthTokenDto(newAccessToken, newRefreshToken), "Token refreshed successfully.");
        }

        /// <summary>
        /// Logs out a user by invalidating the refresh token.
        /// </summary>
        public async Task<ServiceResult<bool>> LogoutAsync(string refreshToken)
        {
            var user = await _authRepository.FindByRefreshTokenAsync(refreshToken);
            if (user == null)
            {
                _logger.LogWarning("Logout attempt failed. Invalid refresh token.");
                return ServiceResult<bool>.NotFound("User not found", new List<string> { "Invalid refresh token." });
            }

            user.UpdateRefreshToken(null, DateTime.UtcNow);
            await _authRepository.UpdateUserAsync(user);

            _logger.LogInformation("User {Email} logged out successfully.", user.Email); // ✅ Logs logout events

            return ServiceResult<bool>.Success(true, "User logged out successfully.");
        }

        /// <summary>
        /// Maps a User entity to an AuthUserDto.
        /// </summary>
        private static AuthUserDto MapToDto(User user)
        {
            return new AuthUserDto(
                user.Id,
                user.Email,
                user.Role,
                user.Status,
                user.FullName,
                user.Photo,
                user.BirthDate, // ✅ Convert DateTime? to string
                user.Address,
                user.Gender,
                user.ContactNumber,
                user.EmailVerified,
                user.CreatedAt,
                user.UpdatedAt
            );
        }
    }
}
