using OntuPhdApi.Models.Authorization;

namespace OntuPhdApi.Services.Authorization
{
    public interface IAuthService
    {
        Task<(string AccessToken, string RefreshToken)> GenerateTokensAsync(int userId);
        Task<User?> ValidateRefreshTokenAsync(string refreshToken);
        Task RevokeRefreshTokenAsync(string refreshToken);
    }
}
