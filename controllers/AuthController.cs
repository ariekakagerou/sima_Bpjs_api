using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using sima_bpjs_api.Data;
using sima_bpjs_api.Models;
using Microsoft.EntityFrameworkCore;

namespace sima_bpjs_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly AppDbContext _context;
        public AuthController(IConfiguration config, AppDbContext context)
        {
            _config = config;
            _context = context;
        }

        [HttpGet("test")]
        [AllowAnonymous]
        public IActionResult Test()
        {
            return Ok(new { message = "API is working", timestamp = DateTime.Now });
        }

        [HttpPost("register")]
        [Consumes("application/json")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterModel body)
        {
            if (body == null || string.IsNullOrWhiteSpace(body.Username) || string.IsNullOrWhiteSpace(body.Password))
            {
                return BadRequest(new { message = "Username dan password wajib diisi" });
            }

            if (string.IsNullOrWhiteSpace(body.Email) && string.IsNullOrWhiteSpace(body.PhoneNumber))
            {
                return BadRequest(new { message = "Email atau No. Telepon wajib diisi" });
            }

            // Cek apakah user mencoba register sebagai admin
            if (body.Role?.ToUpperInvariant() == "ADMIN")
            {
                return Forbid("Tidak bisa mendaftar sebagai admin. Hubungi administrator untuk akses admin.");
            }

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
                DateOfBirth = body.DateOfBirth
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Created(string.Empty, new { id = user.Id, user.Username, user.Role, user.Nik, user.Email, user.PhoneNumber, user.DateOfBirth });
        }

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
                if (user == null || !VerifyPassword(login.Password, user.PasswordHash))
                {
                    return Unauthorized(new { message = "Kredensial tidak valid" });
                }

                var token = GenerateJwtToken(user.Username, user.Role);
                return Ok(new { token, username = user.Username, role = user.Role, email = user.Email, phoneNumber = user.PhoneNumber });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        private string GenerateJwtToken(string username, string role)
        {
            var jwtSettings = _config.GetSection("Jwt");
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, string.IsNullOrWhiteSpace(role) ? "USER" : role)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"] ?? throw new InvalidOperationException("JWT Key not configured")));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings["ExpireMinutes"])),
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private static string HashPassword(string password)
        {
            // PBKDF2 simple helper
            using var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
            var salt = new byte[16];
            rng.GetBytes(salt);

            var pbkdf2 = new System.Security.Cryptography.Rfc2898DeriveBytes(password, salt, 100000, System.Security.Cryptography.HashAlgorithmName.SHA256);
            var hash = pbkdf2.GetBytes(32);

            var combined = new byte[1 + salt.Length + hash.Length];
            combined[0] = 0x01; // version
            Buffer.BlockCopy(salt, 0, combined, 1, salt.Length);
            Buffer.BlockCopy(hash, 0, combined, 1 + salt.Length, hash.Length);
            return Convert.ToBase64String(combined);
        }

        [HttpPost("create-admin")]
        [AllowAnonymous]
        public async Task<IActionResult> CreateAdmin([FromBody] CreateAdminModel model)
        {
            // Hanya bisa diakses saat development
            if (!_config.GetValue<bool>("IsDevelopment", false))
            {
                return Forbid("Endpoint hanya tersedia saat development");
            }

            // Hapus validasi secret key

            if (string.IsNullOrWhiteSpace(model.Username) || string.IsNullOrWhiteSpace(model.Password))
            {
                return BadRequest(new { message = "Username dan password wajib diisi" });
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

            return Created(string.Empty, new {
                message = "Admin berhasil dibuat",
                username = admin.Username,
                role = admin.Role,
                nik = admin.Nik
            });
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

                var pbkdf2 = new System.Security.Cryptography.Rfc2898DeriveBytes(password, salt, 100000, System.Security.Cryptography.HashAlgorithmName.SHA256);
                var computed = pbkdf2.GetBytes(32);

                // constant-time compare
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
        public string? Role { get; set; } // default USER
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }
    }

    public class CreateAdminModel
    {
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? Nik { get; set; }
        // SecretKey dihapus
    }
} 