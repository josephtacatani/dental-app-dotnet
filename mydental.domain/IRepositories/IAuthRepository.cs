using mydental.domain.Entities;

namespace mydental.domain.IRepositories
{
    public interface IAuthRepository
    {

        Task<User?> RegisterAsync(User patientUser);
        Task<string?> LoginAsync(string email, string password);
        Task<(string AccessToken, string RefreshToken)?> RefreshTokenAsync(string refreshToken);

        Task<User?> FindByRefreshTokenAsync(string refreshToken);
        Task<bool> UpdateUserAsync(User user);

        Task<User?> FindByEmailAsync(string email);



    }
}
