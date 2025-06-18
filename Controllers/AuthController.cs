using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OntuPhdApi.Data;
using OntuPhdApi.Services.Authorization;
using BCrypt.Net;
using OntuPhdApi.Models.Authorization;
using Microsoft.Extensions.Logging;
using static System.Net.Mime.MediaTypeNames;


namespace OntuPhdApi.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly AppDbContext _context;
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;
        private readonly IWebHostEnvironment _environment;
        public AuthController(AppDbContext context, IAuthService authService, ILogger<AuthController> logger, IWebHostEnvironment environment)
        {
            _context = context;
            _authService = authService;
            _logger = logger;
            _environment = environment; 
        }

        [Authorize]
        [HttpGet("admins")]
        public async Task<IActionResult> GetAdmins()
        {
            _logger.LogInformation("Starting GetAdmins request for user ID: {UserId}",
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var admins = await _context.Users
                .OrderBy(u => u.Id)
                .Skip(2)
                .Select(u => new
                {
                    u.Id,
                    u.Email,
                    u.Name,
                    u.Image
                })
                .ToListAsync();

            _logger.LogInformation("GetAdmins successful, found {Count} admins", admins.Count);
            return Ok(new { admins });
        }

        [HttpPost("create-admin")]
        public async Task<IActionResult> CreateAdmin([FromBody] CreateAdminRequest request)
        {
            _logger.LogInformation("Starting CreateAdmin with request: Email={Email}", request.Email);

            if (string.IsNullOrEmpty(request.Email))
            {
                _logger.LogError("CreateAdmin failed: Email is required");
                return BadRequest(new { message = "Email is required" });
            }

            // Проверяем, не существует ли пользователь
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == request.Email);
            if (existingUser != null)
            {
                _logger.LogError("CreateAdmin failed: User already exists with email: {Email}", request.Email);
                return Conflict(new { message = "User with this email already exists" });
            }

            // Проверяем, не существует ли пользователь
            existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Name == request.Name);
            if (existingUser != null)
            {
                _logger.LogError("CreateAdmin failed: User already exists with name: {Name}", request.Name);
                return Conflict(new { message = "User with this name already exists" });
            }

            // Создаем нового админа
            var user = new User
            {
                Email = request.Email,
                Name = request.Name ?? "Admin",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin"), // Дефолтный пароль
                MustChangePassword = true, // Требуется смена пароля
                Image = "blank.png"
            };

            // Создаём папку пользователя и копируем blank.png
            if (string.IsNullOrEmpty(user.Name))
            {
                _logger.LogError("CreateAdmin failed: User name is empty for email: {Email}", user.Email);
                return BadRequest(new { message = "User name is required" });
            }

            var safeName = string.Join("_", user.Id);
            var userUploadsDir = Path.Combine(_environment.WebRootPath, "Files", "Uploads", "Users", safeName);
            var imagePath = $"Files/Uploads/Users/{safeName}/blank.png";

            try
            {
                Directory.CreateDirectory(userUploadsDir);
                _logger.LogInformation("Created directory for user: {Directory}", userUploadsDir);

                var sourceBlankPath = Path.Combine(_environment.WebRootPath, "Files", "Uploads", "blank.png");
                var destBlankPath = Path.Combine(userUploadsDir, "blank.png");

                if (System.IO.File.Exists(sourceBlankPath))
                {
                    System.IO.File.Copy(sourceBlankPath, destBlankPath, overwrite: false);
                    _logger.LogInformation("Copied blank.png to: {DestPath}", destBlankPath);
                }
                else
                {
                    _logger.LogWarning("Source blank.png not found at: {SourcePath}, proceeding without copy", sourceBlankPath);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CreateAdmin failed: Error creating directory or copying blank.png for user: {Email}, Directory: {Directory}",
                    user.Email, userUploadsDir);
                return StatusCode(500, new { message = "Error setting up user avatar directory" });
            }

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Created new admin: Id={Id}, Name={Name}, Email={Email}, Role={Role}, MustChangePassword={MustChangePassword}",
                user.Id, user.Name, user.Email, "User", user.MustChangePassword);

            var response = new
            {
                user = new { user.Id, user.Name, user.Email, user.Image }
            };
            return Ok(response);
        }

        // POST: /api/auth/signin
        [HttpPost("signin")]
        public async Task<IActionResult> SignIn([FromBody] SignInRequest request)
        {

            _logger.LogInformation("Starting SignIn with request: Email={Email}", request.Email);
            if (string.IsNullOrEmpty(request.Email))
            {
                _logger.LogError("SignIn failed: Email is required");
                return BadRequest(new { message = "Email is required" });
            }

            if (string.IsNullOrEmpty(request.Password))
            {
                _logger.LogError("SignIn failed: Password is required" + request.Password);
                return BadRequest(new { message = "Password is required" });
            }

            // Поиск или создание пользователя
            _logger.LogInformation("Searching for user with email: {Email}", request.Email);
            var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null)
            {
                _logger.LogInformation("User not found with email: {Email}", request.Email);

                return Unauthorized(new { message = "User not found" });
            }
            else
            {
                _logger.LogInformation("Found existing user: Id={Id}, Name={Name}, Email={Email}", user.Id, user.Name, user.Email);
                // Проверяем пароль

                if (string.IsNullOrEmpty(user.PasswordHash) || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                {
                    _logger.LogError("SignIn failed: Invalid password for email: {Email}", request.Email);
                    return Unauthorized(new { message = "Invalid email or password" });
                }
            }

            // Генерация токенов
            _logger.LogInformation("Generating tokens for user ID: {UserId}", user.Id);
            var (accessToken, refreshToken) = await _authService.GenerateTokensAsync(user.Id);


            // Установка Refresh Token в httpOnly куки
            _logger.LogInformation("Setting refreshToken cookie for user ID: {UserId}, Token: {RefreshToken}",
                        user.Id, refreshToken);
            Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
            {
                HttpOnly = false, // Недоступно для JavaScript
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

            // Установка mustChangePassword Token в обычную куки
            _logger.LogInformation("Setting MustChangePassword cookie for user ID: {UserId}, Value: {Value}",
            user.Id, user.MustChangePassword);
            Response.Cookies.Append("mustChangePassword", user.MustChangePassword ? "true" : "false", new CookieOptions
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
            _logger.LogInformation("SignIn successful, response: Id={Id}, Name={Name}, Email={Email}",
                response.user.Id, response.user.Name, response.user.Email);
            return Ok(response);

        }

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
                Response.Cookies.Delete("mustChangePassword");
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
                HttpOnly = false,
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
                Expires = DateTimeOffset.UtcNow.AddMinutes(1)
            });

            _logger.LogInformation("Setting MustChangePassword cookie for user ID: {UserId}, Value: {Value}",
            user.Id, user.MustChangePassword);

            Response.Cookies.Append("mustChangePassword", user.MustChangePassword ? "true" : "false", new CookieOptions
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
            _logger.LogInformation("Deleting mustChangePassword cookie");
            Response.Cookies.Delete("mustChangePassword");
            return Ok(new { message = "Signed out" });
        }

        [Authorize]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            _logger.LogInformation("Starting ChangePassword for user ID: {UserId}", User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            if (string.IsNullOrEmpty(request.NewPassword))
            {
                _logger.LogError("ChangePassword failed: New password is required");
                return BadRequest(new { message = "New password is required" });
            }

            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out var userId))
            {
                _logger.LogError("ChangePassword failed: Invalid user ID in token");
                return Unauthorized(new { message = "Invalid token" });
            }

            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                _logger.LogError("ChangePassword failed: User not found for ID: {UserId}", userId);
                return NotFound(new { message = "User not found" });
            }

            // Проверяем старый пароль (для обычной смены пароля)
            if (!user.MustChangePassword && string.IsNullOrEmpty(request.OldPassword))
            {
                _logger.LogError("ChangePassword failed: Old password is required for user ID: {UserId}", userId);
                return BadRequest(new { message = "Old password is required" });
            }

            if (!user.MustChangePassword && !BCrypt.Net.BCrypt.Verify(request.OldPassword, user.PasswordHash))
            {
                _logger.LogError("ChangePassword failed: Invalid old password for user ID: {UserId}", userId);
                return Unauthorized(new { message = "Invalid old password" });
            }
            Response.Cookies.Delete("mustChangePassword");
            Response.Cookies.Append("mustChangePassword", user.MustChangePassword ? "true" : "false", new CookieOptions
            {
                HttpOnly = false,
                Secure = false,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddMinutes(15)
            });

            // Обновляем пароль
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
            user.MustChangePassword = false; // Сбрасываем флаг
            await _context.SaveChangesAsync();
            _logger.LogInformation("Password changed for user: Id={Id}, Email={Email}, Role={Role}",
                user.Id, user.Email, "User");

            return Ok(new { message = "Password changed successfully" });
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

            // Формируем путь к аватарке
            string? ImagePath = null;
            if (!string.IsNullOrEmpty(user.Name) && !string.IsNullOrEmpty(user.Image))
            {
                // Экранируем недопустимые символы в имени
                var safeName = user.Id;
                ImagePath = $"Files/Uploads/Users/{safeName}/{user.Image}";
            }

            var response = new
            {
                user.Id,
                user.Name,
                user.Email,
                user.Image
            };
            _logger.LogInformation("GetCurrentUser successful, response: Id={Id}, Name={Name}, Email={Email}, ImagePath={ImagePath}",
                response.Id, response.Name, response.Email, response.Image ?? "null");
            return Ok(response);
        }


        [Authorize]
        [HttpPost("upload-avatar")]
        public async Task<IActionResult> UploadAvatar(IFormFile? file)
        {
            _logger.LogInformation("Starting UploadAvatar request for user ID: {UserId}",
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out var userId))
            {
                _logger.LogError("UploadAvatar failed: Invalid user ID in token");
                return Unauthorized(new { message = "Invalid token" });
            }

            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                _logger.LogError("UploadAvatar failed: User not found for ID: {UserId}", userId);
                return NotFound(new { message = "User not found" });
            }


            var allowedTypes = new[] { "image/png", "image/jpeg", "image/jpg" };
            var safeName = string.Join("_", user.Id);
            var uploadsDir = Path.Combine(_environment.WebRootPath, "Files", "Uploads", "Users", safeName);
            Directory.CreateDirectory(uploadsDir);

            // Удаляем все старые аватарки, кроме blank.png
            try
            {
                foreach (var oldFile in Directory.GetFiles(uploadsDir))
                {
                    var fileName = Path.GetFileName(oldFile);
                    if (fileName != "blank.png")
                    {
                        System.IO.File.Delete(oldFile);
                        _logger.LogInformation("Deleted old avatar for user ID: {UserId}, File: {File}", userId, oldFile);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete old avatars for user ID: {UserId}, Directory: {Directory}", userId, uploadsDir);
                // Продолжаем, даже если удаление не удалось
            }

            string imagePath;
            if (file == null || file.Length == 0)
            {
                // Удаление аватарки: устанавливаем blank.png
                user.Image = "blank.png";
                imagePath = $"Files/Uploads/Users/{safeName}/blank.png";
                _logger.LogInformation("Set blank.png for user ID: {UserId}, ImagePath: {ImagePath}", userId, imagePath);
            }
            else
            {
                // Загрузка новой аватарки
                if (file.Length > 5 * 1024 * 1024)
                {
                    _logger.LogError("UploadAvatar failed: File too large for user ID: {UserId}, Size: {Size}", userId, file.Length);
                    return BadRequest(new { message = "File size exceeds 5 MB" });
                }
                if (!allowedTypes.Contains(file.ContentType))
                {
                    _logger.LogError("UploadAvatar failed: Invalid file type for user ID: {UserId}, Type: {Type}", userId, file.ContentType);
                    return BadRequest(new { message = "Only PNG, JPG, JPEG files are allowed" });
                }

                var extension = Path.GetExtension(file.FileName).ToLower();
                var fileName = $"avatar_{DateTime.UtcNow:yyyyMMddHHmmss}{extension}";
                var filePath = Path.Combine(uploadsDir, fileName);

                try
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "UploadAvatar failed: Error saving file for user ID: {UserId}, Path: {Path}", userId, filePath);
                    return StatusCode(500, new { message = "Error saving file" });
                }

                user.Image = fileName;
                imagePath = $"Files/Uploads/Users/{safeName}/{fileName}";
                _logger.LogInformation("Uploaded new avatar for user ID: {UserId}, Image: {Image}, ImagePath: {ImagePath}", userId, fileName, imagePath);
            }

            await _context.SaveChangesAsync();
            _logger.LogInformation("Updated user image for user ID: {UserId}, Image: {Image}", userId, user.Image);

            return Ok(new { imagePath });

        }


        [Authorize]
        [HttpDelete("delete-admin/{id}")]
        public async Task<IActionResult> DeleteAdmin(int id)
        {
            _logger.LogInformation("Starting DeleteAdmin request for user ID: {CurrentUserId}, Target ID: {TargetId}",
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value, id);

            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out var currentUserId))
            {
                _logger.LogError("DeleteAdmin failed: Invalid current user ID in token");
                return Unauthorized(new { message = "Invalid token" });
            }

            // Нельзя удалить самого себя
            if (currentUserId == id)
            {
                _logger.LogError("DeleteAdmin failed: User ID {UserId} cannot delete self", currentUserId);
                return BadRequest(new { message = "Cannot delete yourself" });
            }

            var user = await _context.Users
                .Include(u => u.Accounts)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                _logger.LogError("DeleteAdmin failed: User not found for ID: {TargetId}", id);
                return NotFound(new { message = "User not found" });
            }

            // Проверяем, что останется хотя бы один админ
            var adminCount = await _context.Users.CountAsync();
            if (adminCount <= 1)
            {
                _logger.LogError("DeleteAdmin failed: Cannot delete the last admin, ID: {TargetId}", id);
                return BadRequest(new { message = "Cannot delete the last admin" });
            }


            // Удаляем папку с аватаркой
            var safeName = string.Join("_", user.Id);
            var userDir = Path.Combine(_environment.WebRootPath, "Files", "Uploads", "Users", safeName);
            if (Directory.Exists(userDir))
            {
                Directory.Delete(userDir, true);
                _logger.LogInformation("Deleted directory for user ID: {TargetId}, Directory: {Directory}", id, userDir);
            }

            // Удаляем пользователя и связанные аккаунты
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            _logger.LogInformation("DeleteAdmin successful: Deleted user ID: {TargetId}, Email: {Email}",
                id, user.Email);

            return Ok(new { message = "Admin deleted successfully" });
        }

    }

    public class CreateAdminRequest
    {
        public string? Email { get; set; }
        public string? Name { get; set; }
    }

    public class ChangePasswordRequest
    {
        public string? OldPassword { get; set; } // Не требуется для MustChangePassword
        public string NewPassword { get; set; }
    }

    public class SignInRequest
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
    }

    public class RefreshTokenRequest
    {
        public string RefreshToken { get; set; } = null!;
    }

}
