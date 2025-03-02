using Microsoft.AspNetCore.Mvc;
using mydental.application.DTO.AuthDTO;
using mydental.application.Services.IServices;
using mydental.application.Common;
using Microsoft.AspNetCore.Authorization;

namespace mydental.application.Controllers
{
    [ApiController]
    [Route("api/auth")]
    [Produces("application/json")] // ✅ Ensures JSON response type
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Register a new user.
        /// </summary>
        /// <param name="registerDto">User registration details.</param>
        /// <returns>Registration result.</returns>
        [HttpPost("register")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ServiceResult<AuthUserDto>), 201)] // ✅ Created
        [ProducesResponseType(typeof(ServiceResult<AuthUserDto>), 400)] // ❌ Bad Request
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            var result = await _authService.RegisterAsync(registerDto);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Login and get JWT access & refresh tokens.
        /// </summary>
        /// <param name="loginDto">User login credentials.</param>
        /// <returns>JWT tokens.</returns>
        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ServiceResult<AuthTokenDto>), 200)] // ✅ Success
        [ProducesResponseType(typeof(ServiceResult<AuthTokenDto>), 400)]       // Unauthorized
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var result = await _authService.LoginAsync(loginDto);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Refresh an expired access token using a refresh token.
        /// </summary>
        /// <param name="refreshToken">The refresh token.</param>
        /// <returns>New access & refresh tokens.</returns>
        [HttpPost("refresh-token")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ServiceResult<AuthTokenDto>), 200)] // ✅ Success
        [ProducesResponseType(typeof(ServiceResult<AuthTokenDto>), 401)] // ❌ Unauthorized
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            var result = await _authService.RefreshTokenAsync(request.RefreshToken);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Logout user and invalidate refresh token.
        /// </summary>
        /// <param name="refreshToken">Refresh token to revoke.</param>
        /// <returns>Logout success.</returns>
        [HttpPost("logout")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ServiceResult<bool>), 200)] // ✅ Success
        [ProducesResponseType(typeof(ServiceResult<bool>), 404)] // ❌ Not Found
        public async Task<IActionResult> Logout([FromBody] LogoutRequest logout)
        {
            var result = await _authService.LogoutAsync(logout.RefreshToken);
            return StatusCode(result.StatusCode, result);
        }
    }
}
