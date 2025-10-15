using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sima_bpjs_api.Data;
using sima_bpjs_api.Models;
using sima_bpjs_api.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using System.Security.Cryptography;
using System.Text;

namespace sima_bpjs_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all users
        /// </summary>
        [HttpGet]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<IEnumerable<UserResponseDto>>> GetUsers()
        {
            try
            {
                var users = await _context.Users
                    .Select(u => new UserResponseDto
                    {
                        Id = u.Id,
                        Username = u.Username,
                        Email = u.Email,
                        PhoneNumber = u.PhoneNumber,
                        Role = u.Role,
                        Nik = u.Nik,
                        DateOfBirth = u.DateOfBirth,
                        CreatedAt = u.CreatedAt
                    })
                    .ToListAsync();

                return Ok(new { success = true, data = users, message = "Users retrieved successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Internal server error", error = ex.Message });
            }
        }

        /// <summary>
        /// Get user by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<UserResponseDto>> GetUser(int id)
        {
            try
            {
                var user = await _context.Users
                    .Where(u => u.Id == id)
                    .Select(u => new UserResponseDto
                    {
                        Id = u.Id,
                        Username = u.Username,
                        Email = u.Email,
                        PhoneNumber = u.PhoneNumber,
                        Role = u.Role,
                        Nik = u.Nik,
                        DateOfBirth = u.DateOfBirth,
                        CreatedAt = u.CreatedAt
                    })
                    .FirstOrDefaultAsync();

                if (user == null)
                {
                    return NotFound(new { success = false, message = "User not found" });
                }

                return Ok(new { success = true, data = user, message = "User retrieved successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Internal server error", error = ex.Message });
            }
        }

        /// <summary>
        /// Create new user
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<UserResponseDto>> CreateUser(UserCreateDto userDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { success = false, message = "Invalid data", errors = ModelState });
                }

                // Check if username already exists
                var existingUser = await _context.Users
                    .FirstOrDefaultAsync(u => u.Username == userDto.Username);

                if (existingUser != null)
                {
                    return Conflict(new { success = false, message = "Username already exists" });
                }

                // Check if email already exists (if provided)
                if (!string.IsNullOrEmpty(userDto.Email))
                {
                    var existingEmail = await _context.Users
                        .FirstOrDefaultAsync(u => u.Email == userDto.Email);

                    if (existingEmail != null)
                    {
                        return Conflict(new { success = false, message = "Email already exists" });
                    }
                }

                // Check if phone number already exists (if provided)
                if (!string.IsNullOrEmpty(userDto.PhoneNumber))
                {
                    var existingPhone = await _context.Users
                        .FirstOrDefaultAsync(u => u.PhoneNumber == userDto.PhoneNumber);

                    if (existingPhone != null)
                    {
                        return Conflict(new { success = false, message = "Phone number already exists" });
                    }
                }

                // Check if NIK already exists (if provided)
                if (!string.IsNullOrEmpty(userDto.Nik))
                {
                    var existingNik = await _context.Users
                        .FirstOrDefaultAsync(u => u.Nik == userDto.Nik);

                    if (existingNik != null)
                    {
                        return Conflict(new { success = false, message = "NIK already exists" });
                    }
                }

                var user = new User
                {
                    Username = userDto.Username,
                    PasswordHash = HashPassword(userDto.Password),
                    Email = userDto.Email,
                    PhoneNumber = userDto.PhoneNumber,
                    Role = userDto.Role,
                    Nik = userDto.Nik,
                    DateOfBirth = userDto.DateOfBirth,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                var response = new UserResponseDto
                {
                    Id = user.Id,
                    Username = user.Username,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Role = user.Role,
                    Nik = user.Nik,
                    DateOfBirth = user.DateOfBirth,
                    CreatedAt = user.CreatedAt
                };

                return CreatedAtAction(nameof(GetUser), new { id = user.Id }, 
                    new { success = true, data = response, message = "User created successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Internal server error", error = ex.Message });
            }
        }

        /// <summary>
        /// Update user
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserUpdateDto userDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { success = false, message = "Invalid data", errors = ModelState });
                }

                var user = await _context.Users.FindAsync(id);
                if (user == null)
                {
                    return NotFound(new { success = false, message = "User not found" });
                }

                // Check if username already exists (excluding current user)
                var existingUser = await _context.Users
                    .FirstOrDefaultAsync(u => u.Username == userDto.Username && u.Id != id);

                if (existingUser != null)
                {
                    return Conflict(new { success = false, message = "Username already exists" });
                }

                // Check if email already exists (excluding current user)
                if (!string.IsNullOrEmpty(userDto.Email))
                {
                    var existingEmail = await _context.Users
                        .FirstOrDefaultAsync(u => u.Email == userDto.Email && u.Id != id);

                    if (existingEmail != null)
                    {
                        return Conflict(new { success = false, message = "Email already exists" });
                    }
                }

                // Check if phone number already exists (excluding current user)
                if (!string.IsNullOrEmpty(userDto.PhoneNumber))
                {
                    var existingPhone = await _context.Users
                        .FirstOrDefaultAsync(u => u.PhoneNumber == userDto.PhoneNumber && u.Id != id);

                    if (existingPhone != null)
                    {
                        return Conflict(new { success = false, message = "Phone number already exists" });
                    }
                }

                // Check if NIK already exists (excluding current user)
                if (!string.IsNullOrEmpty(userDto.Nik))
                {
                    var existingNik = await _context.Users
                        .FirstOrDefaultAsync(u => u.Nik == userDto.Nik && u.Id != id);

                    if (existingNik != null)
                    {
                        return Conflict(new { success = false, message = "NIK already exists" });
                    }
                }

                user.Username = userDto.Username;
                user.Email = userDto.Email;
                user.PhoneNumber = userDto.PhoneNumber;
                user.Role = userDto.Role;
                user.Nik = userDto.Nik;
                user.DateOfBirth = userDto.DateOfBirth;

                await _context.SaveChangesAsync();

                var response = new UserResponseDto
                {
                    Id = user.Id,
                    Username = user.Username,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Role = user.Role,
                    Nik = user.Nik,
                    DateOfBirth = user.DateOfBirth,
                    CreatedAt = user.CreatedAt
                };

                return Ok(new { success = true, data = response, message = "User updated successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Internal server error", error = ex.Message });
            }
        }

        /// <summary>
        /// Update user password
        /// </summary>
        [HttpPut("{id}/password")]
        public async Task<IActionResult> UpdatePassword(int id, UserPasswordUpdateDto passwordDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { success = false, message = "Invalid data", errors = ModelState });
                }

                var user = await _context.Users.FindAsync(id);
                if (user == null)
                {
                    return NotFound(new { success = false, message = "User not found" });
                }

                // Verify current password
                if (!VerifyPassword(passwordDto.CurrentPassword, user.PasswordHash))
                {
                    return BadRequest(new { success = false, message = "Current password is incorrect" });
                }

                user.PasswordHash = HashPassword(passwordDto.NewPassword);
                await _context.SaveChangesAsync();

                return Ok(new { success = true, message = "Password updated successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Internal server error", error = ex.Message });
            }
        }

        /// <summary>
        /// Delete user
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);
                if (user == null)
                {
                    return NotFound(new { success = false, message = "User not found" });
                }

                // Check if user has any related BPJS records
                var hasBpjs = await _context.Bpjs
                    .AnyAsync(b => b.Nik == user.Nik);

                if (hasBpjs)
                {
                    return BadRequest(new { success = false, message = "Cannot delete user with existing BPJS records. Please delete BPJS records first." });
                }

                _context.Users.Remove(user);
                await _context.SaveChangesAsync();

                return Ok(new { success = true, message = "User deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Internal server error", error = ex.Message });
            }
        }

        // Helper method untuk hash password
        private static string HashPassword(string password)
        {
            using var rng = RandomNumberGenerator.Create();
            var salt = new byte[16];
            rng.GetBytes(salt);

            var pbkdf2 = new Rfc2898DeriveBytes(
                password, salt, 100000, HashAlgorithmName.SHA256);
            var hash = pbkdf2.GetBytes(32);

            var combined = new byte[1 + salt.Length + hash.Length];
            combined[0] = 0x01; // version
            Buffer.BlockCopy(salt, 0, combined, 1, salt.Length);
            Buffer.BlockCopy(hash, 0, combined, 1 + salt.Length, hash.Length);
            return Convert.ToBase64String(combined);
        }

        // Helper method untuk verify password
        private static bool VerifyPassword(string password, string hash)
        {
            try
            {
                var combined = Convert.FromBase64String(hash);
                if (combined[0] != 0x01) return false;

                var salt = new byte[16];
                Buffer.BlockCopy(combined, 1, salt, 0, salt.Length);

                var storedHash = new byte[32];
                Buffer.BlockCopy(combined, 1 + salt.Length, storedHash, 0, storedHash.Length);

                var pbkdf2 = new Rfc2898DeriveBytes(
                    password, salt, 100000, HashAlgorithmName.SHA256);
                var computedHash = pbkdf2.GetBytes(32);

                return computedHash.SequenceEqual(storedHash);
            }
            catch
            {
                return false;
            }
        }
    }
}
