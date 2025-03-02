using mydental.application.DTO.AuthDTO;
using mydental.application.Common;

namespace mydental.application.Services.IServices
{
    public interface IAuthService
    {
        Task<ServiceResult<AuthUserDto>> RegisterAsync(RegisterDto registerDto);
        Task<ServiceResult<AuthTokenDto>> LoginAsync(LoginDto loginDto); // ✅ Ensure this is correct
        Task<ServiceResult<AuthTokenDto>> RefreshTokenAsync(string refreshToken);
        Task<ServiceResult<bool>> LogoutAsync(string refreshToken); // ✅ Added logout
    }
}
