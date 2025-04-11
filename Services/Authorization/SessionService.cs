using Microsoft.EntityFrameworkCore;
using OntuPhdApi.Data;
using OntuPhdApi.Models.Authorization;

namespace OntuPhdApi.Services.Authorization
{
    public class SessionService : ISessionService
    {
        private readonly AppDbContext _context;

        public SessionService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<string> CreateSessionAsync(int userId)
        {
            var sessionToken = Guid.NewGuid().ToString();
            var session = new Session
            {
                UserId = userId,
                SessionToken = sessionToken,
                Expires = DateTime.UtcNow.AddDays(30) // Сессия на 30 дней
            };

            _context.Sessions.Add(session);
            await _context.SaveChangesAsync();
            return sessionToken;
        }

        public async Task<User?> ValidateSessionAsync(string sessionToken)
        {
            var session = await _context.Sessions
                .Include(s => s.User)
                .FirstOrDefaultAsync(s => s.SessionToken == sessionToken && s.Expires > DateTime.UtcNow);

            return session?.User;
        }

        public async Task DeleteSessionAsync(string sessionToken)
        {
            var session = await _context.Sessions
                .FirstOrDefaultAsync(s => s.SessionToken == sessionToken);

            if (session != null)
            {
                _context.Sessions.Remove(session);
                await _context.SaveChangesAsync();
            }
        }



    }
}
