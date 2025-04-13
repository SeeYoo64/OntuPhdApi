using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OntuPhdApi.Data;
using OntuPhdApi.Models.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace OntuPhdApi.Services.Authorization
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthService> _logger;

        public AuthService(AppDbContext context, IConfiguration configuration, ILogger<AuthService> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<(string AccessToken, string RefreshToken)> GenerateTokensAsync(int userId)
        {
            _logger.LogInformation("Generating tokens for user ID: {UserId}", userId);

            // Access Token
            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            new Claim(ClaimTypes.Role, "User")
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var accessToken = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(15),
                signingCredentials: creds
            );

            var accessTokenString = new JwtSecurityTokenHandler().WriteToken(accessToken);
            _logger.LogInformation("Generated Access Token for user ID: {UserId}, Token: {AccessToken}",
                userId, accessTokenString.Length > 10 ? accessTokenString[..10] + "..." : accessTokenString);

            // Refresh Token
            var refreshToken = Guid.NewGuid().ToString();
            _logger.LogInformation("Generated Refresh Token for user ID: {UserId}, Token: {RefreshToken}",
                userId, refreshToken.Length > 10 ? refreshToken[..10] + "..." : refreshToken);

            // Сохранение Refresh Token
            var account = await _context.Accounts.FirstOrDefaultAsync(a => a.UserId == userId);
            if (account == null)
            {
                account = new Account
                {
                    UserId = userId,
                    RefreshToken = refreshToken,
                    ExpiresAt = DateTimeOffset.UtcNow.AddDays(90).ToUnixTimeSeconds()
                };
                _context.Accounts.Add(account);
                _logger.LogInformation("Created new Account for user ID: {UserId}, Account: Id={AccountId}, UserId={UserId}, RefreshToken={RefreshToken}, ExpiresAt={ExpiresAt}",
                    userId, account.Id, account.UserId, account.RefreshToken?.Length > 10 ? account.RefreshToken[..10] + "..." : account.RefreshToken, account.ExpiresAt);
            }
            else
            {
                account.RefreshToken = refreshToken;
                account.ExpiresAt = DateTimeOffset.UtcNow.AddDays(90).ToUnixTimeSeconds();
                _logger.LogInformation("Updated Account for user ID: {UserId}, Account: Id={AccountId}, UserId={UserId}, RefreshToken={RefreshToken}, ExpiresAt={ExpiresAt}",
                    userId, account.Id, account.UserId, account.RefreshToken?.Length > 10 ? account.RefreshToken[..10] + "..." : account.RefreshToken, account.ExpiresAt);
            }

            await _context.SaveChangesAsync();
            _logger.LogInformation("Saved changes to database for user ID: {UserId}", userId);

            return (accessTokenString, refreshToken);
        }

        public async Task<User?> ValidateRefreshTokenAsync(string refreshToken)
        {
            _logger.LogInformation("Validating Refresh Token: {RefreshToken}",
                refreshToken.Length > 10 ? refreshToken[..10] + "..." : refreshToken);

            var account = await _context.Accounts
                .Include(a => a.User)
                .FirstOrDefaultAsync(a => a.RefreshToken == refreshToken);

            if (account == null)
            {
                _logger.LogError("Refresh Token not found: {RefreshToken}",
                    refreshToken.Length > 10 ? refreshToken[..10] + "..." : refreshToken);
                return null;
            }

            if (account.ExpiresAt < DateTimeOffset.UtcNow.ToUnixTimeSeconds())
            {
                _logger.LogError("Refresh Token expired: Id={AccountId}, UserId={UserId}, ExpiresAt={ExpiresAt}",
                    account.Id, account.UserId, account.ExpiresAt);
                return null;
            }

            _logger.LogInformation("Refresh Token validated, User: Id={UserId}, Name={Name}, Email={Email}",
                account.User.Id, account.User.Name, account.User.Email);
            return account.User;
        }

        public async Task RevokeRefreshTokenAsync(string refreshToken)
        {
            _logger.LogInformation("Revoking Refresh Token: {RefreshToken}",
                refreshToken.Length > 10 ? refreshToken[..10] + "..." : refreshToken);

            var account = await _context.Accounts
                .FirstOrDefaultAsync(a => a.RefreshToken == refreshToken);

            if (account != null)
            {
                var accountInfo = new { account.Id, account.UserId, RefreshToken = account.RefreshToken, account.ExpiresAt };
                account.RefreshToken = null;
                account.ExpiresAt = null;
                await _context.SaveChangesAsync();
                _logger.LogInformation("Revoked Refresh Token, Account: Id={AccountId}, UserId={UserId}",
                    account.Id, account.UserId);
            }
            else
            {
                _logger.LogWarning("Refresh Token not found for revocation: {RefreshToken}",
                    refreshToken.Length > 10 ? refreshToken[..10] + "..." : refreshToken);
            }
        }


    }
}
