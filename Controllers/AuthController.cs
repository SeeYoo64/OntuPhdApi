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

            // Установка Refresh Token в httpOnly куки
            Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
            {
                HttpOnly = true, // Недоступно для JavaScript
                Secure = false,   // Только HTTPS
                SameSite = SameSiteMode.Strict, // Защита от CSRF
                Expires = DateTimeOffset.UtcNow.AddDays(90) // 90 дней
            });

            // Установка Access Token в обычную куки
            Response.Cookies.Append("accessToken", accessToken, new CookieOptions
            {
                HttpOnly = false, // Доступно для JavaScript
                Secure = false,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddMinutes(15) // 15 минут
            });

            return Ok(new
            {
                user = new { user.Id, user.Name, user.Email }
            });
        }

        // GET: /api/auth/session
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh()
        {
            // Чтение Refresh Token из куки
            var refreshToken = Request.Cookies["refreshToken"];
            if (string.IsNullOrEmpty(refreshToken))
            {
                return Unauthorized(new { message = "Refresh token not found" });
            }

            // Проверка Refresh Token
            var user = await _authService.ValidateRefreshTokenAsync(refreshToken);
            if (user == null)
            {
                Response.Cookies.Delete("refreshToken");
                Response.Cookies.Delete("accessToken");
                return Unauthorized(new { message = "Invalid or expired refresh token" });
            }

            // Генерация новых токенов
            var (newAccessToken, newRefreshToken) = await _authService.GenerateTokensAsync(user.Id);


            // Обновление кук
            Response.Cookies.Append("refreshToken", newRefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = false,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddDays(90)
            });

            Response.Cookies.Append("accessToken", newAccessToken, new CookieOptions
            {
                HttpOnly = false,
                Secure = false,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddMinutes(15)
            });

            return Ok(new
            {
                user = new { user.Id, user.Name, user.Email }
            });
        }


        [Authorize]
        [HttpPost("signout")]
        public async Task<IActionResult> SignOut()
        {
            // Чтение Refresh Token из куки
            var refreshToken = Request.Cookies["refreshToken"];
            if (!string.IsNullOrEmpty(refreshToken))
            {
                await _authService.RevokeRefreshTokenAsync(refreshToken);
            }

            // Удаление обеих кук
            Response.Cookies.Delete("refreshToken");
            Response.Cookies.Delete("accessToken");

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
