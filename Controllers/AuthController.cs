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
        private readonly ILogger<AuthController> _logger;

        public AuthController(AppDbContext context, IAuthService authService, ILogger<AuthController> logger)
        {
            _context = context;
            _authService = authService;
            _logger = logger;
        }

        // POST: /api/auth/signin
        [HttpPost("signin")]
        public async Task<IActionResult> SignIn([FromBody] SignInRequest request)
        {
            _logger.LogInformation("Starting SignIn with request: Email={Email}, Name={Name}", request.Email, request.Name);

            if (string.IsNullOrEmpty(request.Email))
            {
                _logger.LogError("SignIn failed: Email is required");
                return BadRequest(new { message = "Email is required" });
            }

            // Поиск или создание пользователя
            _logger.LogInformation("Searching for user with email: {Email}", request.Email);
            var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null)
            {
                _logger.LogInformation("User not found, creating new user with email: {Email}", request.Email);
                user = new User
                {
                    Email = request.Email,
                    Name = request.Name ?? "User",
                    EmailVerified = null
                };
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Created new user: Id={Id}, Name={Name}, Email={Email}", user.Id, user.Name, user.Email);
            }
            else
            {
                _logger.LogInformation("Found existing user: Id={Id}, Name={Name}, Email={Email}", user.Id, user.Name, user.Email);
            }

            // Генерация токенов
            _logger.LogInformation("Generating tokens for user ID: {UserId}", user.Id);
            var (accessToken, refreshToken) = await _authService.GenerateTokensAsync(user.Id);


            // Установка Refresh Token в httpOnly куки
            _logger.LogInformation("Setting refreshToken cookie for user ID: {UserId}, Token: {RefreshToken}",
                        user.Id, refreshToken);
            Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
            {
                HttpOnly = true, // Недоступно для JavaScript
                Secure = false,   // Только HTTPS
                SameSite = SameSiteMode.Strict, // Защита от CSRF
                Expires = DateTimeOffset.UtcNow.AddDays(90) // 90 дней
            });

            // Установка Access Token в обычную куки
            _logger.LogInformation("Setting accessToken cookie for user ID: {UserId}, Token: {AccessToken}",
                        user.Id, accessToken);
            Response.Cookies.Append("accessToken", accessToken, new CookieOptions
            {
                HttpOnly = false, // Доступно для JavaScript
                Secure = false,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddMinutes(15) // 15 минут
            });

            var response = new
            {
                user = new { user.Id, user.Name, user.Email }
            };
            _logger.LogInformation("SignIn successful, response: Id={Id}, Name={Name}, Email={Email}",
                response.user.Id, response.user.Name, response.user.Email);
            return Ok(response);

        }

        // GET: /api/auth/session
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh()
        {
            _logger.LogInformation("Starting Refresh request");

            // Чтение Refresh Token
            var refreshToken = Request.Cookies["refreshToken"];
            _logger.LogInformation("Read refreshToken from cookie: {RefreshToken}",
                refreshToken);


            if (string.IsNullOrEmpty(refreshToken))
            {
                _logger.LogError("Refresh failed: Refresh token not found");
                return Unauthorized(new { message = "Refresh token not found" });
            }

            // Проверка Refresh Token
            _logger.LogInformation("Validating refreshToken");
            var user = await _authService.ValidateRefreshTokenAsync(refreshToken);
            if (user == null)
            {
                _logger.LogError("Refresh failed: Invalid or expired refresh token");
                Response.Cookies.Delete("refreshToken");
                Response.Cookies.Delete("accessToken");
                return Unauthorized(new { message = "Invalid or expired refresh token" });
            }
            _logger.LogInformation("Refresh token validated, user: Id={Id}, Name={Name}, Email={Email}",
                user.Id, user.Name, user.Email);

            // Генерация новых токенов
            _logger.LogInformation("Generating new tokens for user ID: {UserId}", user.Id);
            var (newAccessToken, newRefreshToken) = await _authService.GenerateTokensAsync(user.Id);

            // Обновление кук
            _logger.LogInformation("Setting new refreshToken cookie for user ID: {UserId}, Token: {RefreshToken}",
                        user.Id, newRefreshToken);
            Response.Cookies.Append("refreshToken", newRefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = false,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddDays(90)
            });

            _logger.LogInformation("Setting new accessToken cookie for user ID: {UserId}, Token: {AccessToken}",
                        user.Id, newAccessToken);
            Response.Cookies.Append("accessToken", newAccessToken, new CookieOptions
            {
                HttpOnly = false,
                Secure = false,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddMinutes(15)
            });

            var response = new
            {
                user = new { user.Id, user.Name, user.Email }
            };
            _logger.LogInformation("Refresh successful, response: Id={Id}, Name={Name}, Email={Email}",
                response.user.Id, response.user.Name, response.user.Email);
            return Ok(response);
        }


        [Authorize]
        [HttpPost("signout")]
        public async Task<IActionResult> SignOut()
        {
            _logger.LogInformation("Starting SignOut request");

            // Чтение Refresh Token
            var refreshToken = Request.Cookies["refreshToken"];
            _logger.LogInformation("Read refreshToken from cookie: {RefreshToken}",
                refreshToken);


            if (!string.IsNullOrEmpty(refreshToken))
            {
                _logger.LogInformation("Revoking refreshToken");
                await _authService.RevokeRefreshTokenAsync(refreshToken);
            }
            else
            {
                _logger.LogWarning("No refreshToken found for SignOut");
            }

            // Удаление обеих кук
            _logger.LogInformation("Deleting refreshToken cookie");
            Response.Cookies.Delete("refreshToken");
            _logger.LogInformation("Deleting accessToken cookie");
            Response.Cookies.Delete("accessToken");

            return Ok(new { message = "Signed out" });
        }


        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> GetCurrentUser()
        {
            _logger.LogInformation("Starting GetCurrentUser request");
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            _logger.LogInformation("Extracted user ID from token: {UserId}", userId);


            _logger.LogInformation("Searching for user with ID: {UserId}", userId);
            var user = await _context.Users.FindAsync(userId);



            if (user == null)
            {
                _logger.LogError("GetCurrentUser failed: User not found for ID: {UserId}", userId);
                return NotFound(new { message = "User not found" });
            }

            var response = new
            {
                user.Id,
                user.Name,
                user.Email
            };
            _logger.LogInformation("GetCurrentUser successful, response: Id={Id}, Name={Name}, Email={Email}",
                response.Id, response.Name, response.Email);
            return Ok(response);
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
