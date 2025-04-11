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
        private readonly ISessionService _sessionService;

        public AuthController(AppDbContext context, ISessionService sessionService)
        {
            _context = context;
            _sessionService = sessionService;
        }

        // POST: /api/auth/signin
        [HttpPost("signin")]
        public async Task<IActionResult> SignIn([FromBody] SignInRequest request)
        {
            // Example: login throw email (can be extended for OAuth)
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null)
            {
                // Creating short User, if he doesnt exist
                user = new User
                {
                    Email = request.Email,
                    Name = request.Name ?? "User",
                    EmailVerified = null // for OAuth or verification
                };
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }

            // Creating session
            var sessionToken = await _sessionService.CreateSessionAsync(user.Id);

            // Installing cookies
            Response.Cookies.Append("auth.session-token", sessionToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = false, // true = only HTTPS
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddDays(30)
            });

            return Ok(new { user = new { user.Id, user.Name, user.Email } });
        }

        // GET: /api/auth/session
        [HttpGet("session")]
        public IActionResult GetSession()
        {
            if (HttpContext.Items["User"] is User user)
            {
                return Ok(new
                {
                    user = new { user.Id, user.Name, user.Email },
                    expires = DateTime.UtcNow.AddDays(30) // Example
                });
            }

            return Ok(new { });
        }

        // POST: /api/auth/signout
        [HttpPost("signout")]
        public async Task<IActionResult> SignOut()
        {
            var sessionToken = HttpContext.Request.Cookies["auth.session-token"];
            if (!string.IsNullOrEmpty(sessionToken))
            {
                await _sessionService.DeleteSessionAsync(sessionToken);
                Response.Cookies.Delete("auth.session-token");
            }

            return Ok(new { message = "Signed out" });
        }
    }

    public class SignInRequest
    {
        public string? Email { get; set; }
        public string? Name { get; set; }
        // Can be extended for password or OAuth-token if needed
    }

}
