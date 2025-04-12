using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OntuPhdApi.Data;
using OntuPhdApi.Models.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OntuPhdApi.Services.Authorization
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<(string AccessToken, string RefreshToken)> GenerateTokensAsync(int userId)
        {
            // Генерация Access Token
            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            new Claim(ClaimTypes.Role, "User") // Можно расширить для ролей
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var accessToken = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(15), // Access Token на 15 минут
                signingCredentials: creds
            );

            var accessTokenString = new JwtSecurityTokenHandler().WriteToken(accessToken);

            // Генерация Refresh Token
            var refreshToken = Guid.NewGuid().ToString();

            // Сохранение Refresh Token в базе
            var account = await _context.Accounts
                .FirstOrDefaultAsync(a => a.UserId == userId && a.Type == "jwt");

            if (account == null)
            {
                account = new Account
                {
                    UserId = userId,
                    Type = "jwt",
                    Provider = "local",
                    ProviderAccountId = userId.ToString(),
                    RefreshToken = refreshToken,
                    ExpiresAt = DateTimeOffset.UtcNow.AddDays(90).ToUnixTimeSeconds()
                };
                _context.Accounts.Add(account);
            }
            else
            {
                account.RefreshToken = refreshToken;
                account.ExpiresAt = DateTimeOffset.UtcNow.AddDays(90).ToUnixTimeSeconds();
            }

            await _context.SaveChangesAsync();

            return (accessTokenString, refreshToken);
        }

        public async Task<User?> ValidateRefreshTokenAsync(string refreshToken)
        {
            var account = await _context.Accounts
                .Include(a => a.User)
                .FirstOrDefaultAsync(a => a.RefreshToken == refreshToken && a.Type == "jwt");

            if (account == null || account.ExpiresAt < DateTimeOffset.UtcNow.ToUnixTimeSeconds())
            {
                return null;
            }

            return account.User;
        }

        public async Task RevokeRefreshTokenAsync(string refreshToken)
        {
            var account = await _context.Accounts
                .FirstOrDefaultAsync(a => a.RefreshToken == refreshToken && a.Type == "jwt");

            if (account != null)
            {
                account.RefreshToken = null;
                account.ExpiresAt = null;
                await _context.SaveChangesAsync();
            }
        }
    }
}
