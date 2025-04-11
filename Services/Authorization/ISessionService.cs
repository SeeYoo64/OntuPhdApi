using OntuPhdApi.Models.Authorization;

namespace OntuPhdApi.Services.Authorization
{
    public interface ISessionService
    {
        Task<string> CreateSessionAsync(int userId);
        Task<User?> ValidateSessionAsync(string sessionToken);
        Task DeleteSessionAsync(string sessionToken);
    }
}
