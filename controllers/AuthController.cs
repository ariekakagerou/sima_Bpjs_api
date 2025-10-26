using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using sima_bpjs_api.Data;
using sima_bpjs_api.Models;
using sima_bpjs_api.Validators;
using Microsoft.EntityFrameworkCore;

namespace sima_bpjs_api.Controllers
{
    /// <summary>
    /// Enhanced Authentication Controller with Security Best Practices
    /// Includes: JWT Auth, Account Lockout, Password Validation, Refresh Tokens
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly AppDbContext _context;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IConfiguration config, AppDbContext context, ILogger<AuthController> logger)
        {
            _config = config;
            _context = context;
            _logger = logger;
        }

        [HttpGet("test")]
        [AllowAnonymous]
        public IActionResult Test()
        {
            return Ok(new { message = "API is working", timestamp = DateTime.Now });
        }

        /// <summary>
        /// Register new user with password strength validation
        /// </summary>
        [HttpPost("register")]
        [Consumes("application/json")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterModel body)
        {
            try
            {
                if (body == null || string.IsNullOrWhiteSpace(body.Username) || string.IsNullOrWhiteSpace(body.Password))
                {
                    return BadRequest(new { message = "Username dan password wajib diisi" });
                }

                if (string.IsNullOrWhiteSpace(body.Email) && string.IsNullOrWhiteSpace(body.PhoneNumber))
                {
                    return BadRequest(new { message = "Email atau No. Telepon wajib diisi" });
                }

                // ✅ SECURITY: Prevent privilege escalation
                if (body.Role?.ToUpperInvariant() == "ADMIN")
                {
                    _logger.LogWarning("Attempted admin registration from username: {Username}", body.Username);
                    return Forbid("Tidak bisa mendaftar sebagai admin. Hubungi administrator untuk akses admin.");
                }

                // ✅ SECURITY: Password strength validation
                var (isValid, message) = PasswordValidator.ValidateStrength(body.Password);
                if (!isValid)
                {
                    return BadRequest(new { message });
                }

                // Check duplicates
                var exists = await _context.Users.AnyAsync(u => u.Username == body.Username);
                if (exists)
                {
                    return Conflict(new { message = "Username sudah digunakan" });
                }

                if (!string.IsNullOrWhiteSpace(body.Email))
                {
                    var emailExists = await _context.Users.AnyAsync(u => u.Email == body.Email);
                    if (emailExists)
                    {
                        return Conflict(new { message = "Email sudah digunakan" });
                    }
                }

                if (!string.IsNullOrWhiteSpace(body.PhoneNumber))
                {
                    var phoneExists = await _context.Users.AnyAsync(u => u.PhoneNumber == body.PhoneNumber);
                    if (phoneExists)
                    {
                        return Conflict(new { message = "No. Telepon sudah digunakan" });
                    }
                }

                var passwordHash = HashPassword(body.Password);
                var user = new User
                {
                    Username = body.Username,
                    PasswordHash = passwordHash,
                    Role = string.IsNullOrWhiteSpace(body.Role) ? "USER" : body.Role.ToUpperInvariant(),
                    Nik = string.IsNullOrWhiteSpace(body.Nik) ? null : body.Nik,
                    Email = body.Email,
                    PhoneNumber = body.PhoneNumber,
                    DateOfBirth = body.DateOfBirth,
                    FailedLoginAttempts = 0,
                    LockoutEnd = null
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                _logger.LogInformation("User registered successfully: {Username}", user.Username);

                return Created(string.Empty, new
                {
                    id = user.Id,
                    user.Username,
                    user.Role,
                    user.Nik,
                    user.Email,
                    user.PhoneNumber,
                    user.DateOfBirth
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during registration");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        /// <summary>
        /// Login with account lockout protection and refresh token
        /// </summary>
        [HttpPost("login")]
        [Consumes("application/json")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginModel login)
        {
            try
            {
                if (login == null || string.IsNullOrEmpty(login.EmailOrPhone) || string.IsNullOrEmpty(login.Password))
                {
                    return BadRequest(new { message = "Email/No. Telepon dan password wajib diisi" });
                }

                var user = await _context.Users.FirstOrDefaultAsync(u =>
                    u.Username == login.EmailOrPhone ||
                    u.Email == login.EmailOrPhone ||
                    u.PhoneNumber == login.EmailOrPhone
                );

                // Get client info
                var ipAddress = GetClientIpAddress();
                var userAgent = Request.Headers["User-Agent"].ToString();
                var device = ParseDeviceInfo(userAgent);
                var browser = ParseBrowserInfo(userAgent);

                // ✅ SECURITY: Check account lockout
                if (user != null && user.IsLockedOut)
                {
                    var remainingMinutes = (int)(user.LockoutEnd!.Value - DateTime.UtcNow).TotalMinutes + 1;
                    await RecordLoginActivity(user.Id, user.Username, ipAddress, device, browser, "GAGAL - LOCKED");

                    _logger.LogWarning("Login attempt for locked account: {Username} from IP: {IpAddress}", user.Username, ipAddress);

                    return StatusCode(423, new
                    {
                        message = $"Akun terkunci karena terlalu banyak percobaan login gagal. Coba lagi dalam {remainingMinutes} menit.",
                        lockoutEnd = user.LockoutEnd,
                        remainingMinutes
                    });
                }

                // Verify credentials
                if (user == null || !VerifyPassword(login.Password, user.PasswordHash))
                {
                    // ✅ SECURITY: Increment failed attempts and potentially lock account
                    if (user != null)
                    {
                        user.FailedLoginAttempts++;

                        // Lock account after 5 failed attempts for 15 minutes
                        if (user.FailedLoginAttempts >= 5)
                        {
                            user.LockoutEnd = DateTime.UtcNow.AddMinutes(15);
                            await _context.SaveChangesAsync();

                            await RecordLoginActivity(user.Id, user.Username, ipAddress, device, browser, "GAGAL - LOCKED");

                            _logger.LogWarning("Account locked due to failed attempts: {Username} from IP: {IpAddress}", user.Username, ipAddress);

                            return StatusCode(423, new
                            {
                                message = "Akun dikunci selama 15 menit karena terlalu banyak percobaan login gagal.",
                                lockoutEnd = user.LockoutEnd
                            });
                        }

                        await _context.SaveChangesAsync();
                    }

                    await RecordLoginActivity(null, login.EmailOrPhone, ipAddress, device, browser, "GAGAL");

                    _logger.LogWarning("Failed login attempt for: {EmailOrPhone} from IP: {IpAddress}", login.EmailOrPhone, ipAddress);

                    // Generic error message to prevent user enumeration
                    return Unauthorized(new { message = "Kredensial tidak valid" });
                }

                // ✅ SECURITY: Reset failed attempts on successful login
                user.FailedLoginAttempts = 0;
                user.LockoutEnd = null;
                await _context.SaveChangesAsync();

                // Record successful login
                await RecordLoginActivity(user.Id, user.Username, ipAddress, device, browser, "BERHASIL");

                // ✅ SECURITY: Generate access token and refresh token
                var accessToken = GenerateJwtToken(user.Username, user.Role);
                var refreshToken = GenerateRefreshToken();

                // Store refresh token in database
                var refreshTokenEntity = new RefreshToken
                {
                    UserId = user.Id,
                    Token = refreshToken,
                    ExpiresAt = DateTime.UtcNow.AddDays(7), // Valid for 7 days
                    CreatedAt = DateTime.UtcNow,
                    CreatedByIp = ipAddress,
                    CreatedByDevice = device
                };

                _context.RefreshTokens.Add(refreshTokenEntity);
                await _context.SaveChangesAsync();

                _logger.LogInformation("User logged in successfully: {Username} from IP: {IpAddress}", user.Username, ipAddress);

                return Ok(new
                {
                    accessToken,
                    refreshToken,
                    username = user.Username,
                    role = user.Role,
                    email = user.Email,
                    phoneNumber = user.PhoneNumber,
                    expiresIn = 3600 // 60 minutes in seconds
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        /// <summary>
        /// Refresh access token using refresh token
        /// </summary>
        [HttpPost("refresh")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.RefreshToken))
                {
                    return BadRequest(new { message = "Refresh token wajib diisi" });
                }

                var storedToken = await _context.RefreshTokens
                    .Include(rt => rt.User)
                    .FirstOrDefaultAsync(rt => rt.Token == request.RefreshToken);

                if (storedToken == null)
                {
                    _logger.LogWarning("Invalid refresh token attempt");
                    return Unauthorized(new { message = "Refresh token tidak valid" });
                }

                if (storedToken.IsRevoked)
                {
                    _logger.LogWarning("Revoked refresh token used for user: {UserId}", storedToken.UserId);
                    return Unauthorized(new { message = "Refresh token telah dicabut" });
                }

                if (storedToken.IsExpired)
                {
                    _logger.LogInformation("Expired refresh token for user: {UserId}", storedToken.UserId);
                    return Unauthorized(new { message = "Refresh token telah expired" });
                }

                // Generate new access token
                var newAccessToken = GenerateJwtToken(storedToken.User.Username, storedToken.User.Role);

                _logger.LogInformation("Access token refreshed for user: {Username}", storedToken.User.Username);

                return Ok(new
                {
                    accessToken = newAccessToken,
                    expiresIn = 3600,
                    message = "Token berhasil diperbarui"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during token refresh");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        /// <summary>
        /// Logout - revoke refresh token
        /// </summary>
        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout([FromBody] RefreshTokenRequest request)
        {
            try
            {
                var username = User.FindFirst(ClaimTypes.Name)?.Value;

                if (!string.IsNullOrWhiteSpace(request.RefreshToken))
                {
                    var storedToken = await _context.RefreshTokens
                        .FirstOrDefaultAsync(rt => rt.Token == request.RefreshToken);

                    if (storedToken != null && !storedToken.IsRevoked)
                    {
                        storedToken.IsRevoked = true;
                        storedToken.RevokedAt = DateTime.UtcNow;
                        await _context.SaveChangesAsync();

                        _logger.LogInformation("User logged out: {Username}", username);
                    }
                }

                return Ok(new { message = "Logout berhasil" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during logout");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        /// <summary>
        /// Create admin account (development only)
        /// </summary>
        [HttpPost("create-admin")]
        [AllowAnonymous]
        public async Task<IActionResult> CreateAdmin([FromBody] CreateAdminModel model)
        {
            try
            {
                // ✅ SECURITY: Only available in development
                if (!_config.GetValue<bool>("IsDevelopment", false))
                {
                    return Forbid("Endpoint hanya tersedia saat development");
                }

                if (string.IsNullOrWhiteSpace(model.Username) || string.IsNullOrWhiteSpace(model.Password))
                {
                    return BadRequest(new { message = "Username dan password wajib diisi" });
                }

                // ✅ SECURITY: Password strength validation
                var (isValid, message) = PasswordValidator.ValidateStrength(model.Password);
                if (!isValid)
                {
                    return BadRequest(new { message });
                }

                var exists = await _context.Users.AnyAsync(u => u.Username == model.Username);
                if (exists)
                {
                    return Conflict(new { message = "Username sudah digunakan" });
                }

                var passwordHash = HashPassword(model.Password);
                var admin = new User
                {
                    Username = model.Username,
                    PasswordHash = passwordHash,
                    Role = "ADMIN",
                    Nik = string.IsNullOrWhiteSpace(model.Nik) ? null : model.Nik
                };

                _context.Users.Add(admin);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Admin created: {Username}", admin.Username);

                return Created(string.Empty, new
                {
                    message = "Admin berhasil dibuat",
                    username = admin.Username,
                    role = admin.Role,
                    nik = admin.Nik
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating admin");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        // ==================== PRIVATE HELPER METHODS ====================

        private async Task RecordLoginActivity(int? userId, string username, string? ipAddress, string? device, string? browser, string status)
        {
            try
            {
                var loginActivity = new LoginActivity
                {
                    UserId = userId,
                    Username = username,
                    IpAddress = ipAddress,
                    Device = device,
                    Browser = browser,
                    Status = status,
                    LoginTime = DateTime.Now,
                    CreatedAt = DateTime.Now
                };

                _context.LoginActivities.Add(loginActivity);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error recording login activity");
            }
        }

        private string? GetClientIpAddress()
        {
            try
            {
                var ipAddress = Request.Headers["X-Forwarded-For"].FirstOrDefault();
                if (string.IsNullOrEmpty(ipAddress))
                {
                    ipAddress = Request.Headers["X-Real-IP"].FirstOrDefault();
                }
                if (string.IsNullOrEmpty(ipAddress))
                {
                    ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
                }
                return ipAddress;
            }
            catch
            {
                return null;
            }
        }

        private string? ParseDeviceInfo(string userAgent)
        {
            if (string.IsNullOrEmpty(userAgent)) return "Unknown";

            try
            {
                if (userAgent.Contains("Windows NT 10.0")) return "Windows 10";
                if (userAgent.Contains("Windows NT 11.0")) return "Windows 11";
                if (userAgent.Contains("Windows")) return "Windows";
                if (userAgent.Contains("Mac OS")) return "MacOS";
                if (userAgent.Contains("Linux")) return "Linux";
                if (userAgent.Contains("Android")) return "Android";
                if (userAgent.Contains("iPhone") || userAgent.Contains("iPad")) return "iOS";
                return "Unknown";
            }
            catch
            {
                return "Unknown";
            }
        }

        private string? ParseBrowserInfo(string userAgent)
        {
            if (string.IsNullOrEmpty(userAgent)) return "Unknown";

            try
            {
                if (userAgent.Contains("Edg")) return "Edge";
                if (userAgent.Contains("Chrome")) return "Chrome";
                if (userAgent.Contains("Firefox")) return "Firefox";
                if (userAgent.Contains("Safari") && !userAgent.Contains("Chrome")) return "Safari";
                if (userAgent.Contains("Opera") || userAgent.Contains("OPR")) return "Opera";
                return "Unknown";
            }
            catch
            {
                return "Unknown";
            }
        }

        private string GenerateJwtToken(string username, string role)
        {
            var jwtSettings = _config.GetSection("Jwt");
            var key = Environment.GetEnvironmentVariable("SIMA_JWT_SECRET")
                ?? jwtSettings["Key"]
                ?? throw new InvalidOperationException("JWT Key not configured");

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, string.IsNullOrWhiteSpace(role) ? "USER" : role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["ExpireMinutes"] ?? "60")),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private static string GenerateRefreshToken()
        {
            var randomBytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }

        private static string HashPassword(string password)
        {
            using var rng = RandomNumberGenerator.Create();
            var salt = new byte[16];
            rng.GetBytes(salt);

            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000, HashAlgorithmName.SHA256);
            var hash = pbkdf2.GetBytes(32);

            var combined = new byte[1 + salt.Length + hash.Length];
            combined[0] = 0x01; // version
            Buffer.BlockCopy(salt, 0, combined, 1, salt.Length);
            Buffer.BlockCopy(hash, 0, combined, 1 + salt.Length, hash.Length);
            return Convert.ToBase64String(combined);
        }

        private static bool VerifyPassword(string password, string stored)
        {
            try
            {
                var bytes = Convert.FromBase64String(stored);
                if (bytes.Length < 1 + 16 + 32) return false;
                var version = bytes[0];
                if (version != 0x01) return false;

                var salt = new byte[16];
                Buffer.BlockCopy(bytes, 1, salt, 0, 16);
                var storedHash = new byte[32];
                Buffer.BlockCopy(bytes, 1 + 16, storedHash, 0, 32);

                var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000, HashAlgorithmName.SHA256);
                var computed = pbkdf2.GetBytes(32);

                // ✅ SECURITY: Constant-time comparison
                var diff = 0;
                for (int i = 0; i < 32; i++) diff |= computed[i] ^ storedHash[i];
                return diff == 0;
            }
            catch
            {
                return false;
            }
        }
    }

    // ==================== REQUEST MODELS ====================

    public class LoginModel
    {
        public string EmailOrPhone { get; set; } = null!;
        public string Password { get; set; } = null!;
    }

    public class RegisterModel
    {
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? Nik { get; set; }
        public string? Role { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }
    }

    public class CreateAdminModel
    {
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? Nik { get; set; }
    }

    public class RefreshTokenRequest
    {
        public string RefreshToken { get; set; } = null!;
    }
}

