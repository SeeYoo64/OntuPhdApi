using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OntuPhdApi.Data;
using OntuPhdApi.Models.Authorization;
using OntuPhdApi.Services.Authorization;

namespace OntuPhdApi.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly AppDbContext _context;
        private readonly IAuthService _authService;

        public AuthController(AppDbContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        // POST: /api/auth/signin
        [HttpPost("signin")]
        public async Task<IActionResult> SignIn([FromBody] SignInRequest request)
        {
            if (string.IsNullOrEmpty(request.Email))
            {
                return BadRequest(new { message = "Email is required" });
            }

            // Поиск или создание пользователя
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null)
            {
                user = new User
                {
                    Email = request.Email,
                    Name = request.Name ?? "User",
                    EmailVerified = null
                };
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }

            // Генерация токенов
            var (accessToken, refreshToken) = await _authService.GenerateTokensAsync(user.Id);

            return Ok(new
            {
                accessToken,
                refreshToken,
                user = new { user.Id, user.Name, user.Email },
                expiresIn = 15 * 60 // В секундах (15 минут)
            });
        }

        // GET: /api/auth/session
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request)
        {
            var user = await _authService.ValidateRefreshTokenAsync(request.RefreshToken);
            if (user == null)
            {
                return Unauthorized(new { message = "Invalid or expired refresh token" });
            }

            // Генерация новых токенов
            var (accessToken, newRefreshToken) = await _authService.GenerateTokensAsync(user.Id);

            return Ok(new
            {
                accessToken,
                refreshToken = newRefreshToken,
                user = new { user.Id, user.Name, user.Email },
                expiresIn = 15 * 60
            });
        }


        [Authorize]
        [HttpPost("signout")]
        public async Task<IActionResult> SignOut()
        {
            var refreshToken = Request.Headers["Refresh-Token"].ToString();
            if (!string.IsNullOrEmpty(refreshToken))
            {
                await _authService.RevokeRefreshTokenAsync(refreshToken);
            }

            return Ok(new { message = "Signed out" });
        }


        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var user = await _context.Users.FindAsync(userId);

            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }

            return Ok(new
            {
                user.Id,
                user.Name,
                user.Email
            });
        }






    }

    public class SignInRequest
    {
        public string? Email { get; set; }
        public string? Name { get; set; }
    }

    public class RefreshTokenRequest
    {
        public string RefreshToken { get; set; } = null!;
    }

}
