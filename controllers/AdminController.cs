using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sima_bpjs_api.Data;
using sima_bpjs_api.Models;
using sima_bpjs_api.Models.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace sima_bpjs_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "ADMIN")]
    public class AdminController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all admins
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AdminResponseDto>>> GetAdmins()
        {
            try
            {
                var admins = await _context.Admin
                    .Select(a => new AdminResponseDto
                    {
                        Id = a.Id,
                        Username = a.Username
                    })
                    .ToListAsync();

                return Ok(new { success = true, data = admins, message = "Admins retrieved successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Internal server error", error = ex.Message });
            }
        }

        /// <summary>
        /// Get admin by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<AdminResponseDto>> GetAdmin(int id)
        {
            try
            {
                var admin = await _context.Admin
                    .Where(a => a.Id == id)
                    .Select(a => new AdminResponseDto
                    {
                        Id = a.Id,
                        Username = a.Username
                    })
                    .FirstOrDefaultAsync();

                if (admin == null)
                {
                    return NotFound(new { success = false, message = "Admin not found" });
                }

                return Ok(new { success = true, data = admin, message = "Admin retrieved successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Internal server error", error = ex.Message });
            }
        }

        /// <summary>
        /// Create new admin
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<AdminResponseDto>> CreateAdmin(AdminCreateDto adminDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { success = false, message = "Invalid data", errors = ModelState });
                }

                // Check if username already exists
                var existingAdmin = await _context.Admin
                    .FirstOrDefaultAsync(a => a.Username == adminDto.Username);

                if (existingAdmin != null)
                {
                    return Conflict(new { success = false, message = "Username already exists" });
                }

                var admin = new Admin
                {
                    Username = adminDto.Username,
                    Password = adminDto.Password
                };

                _context.Admin.Add(admin);
                await _context.SaveChangesAsync();

                var response = new AdminResponseDto
                {
                    Id = admin.Id,
                    Username = admin.Username
                };

                return CreatedAtAction(nameof(GetAdmin), new { id = admin.Id }, 
                    new { success = true, data = response, message = "Admin created successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Internal server error", error = ex.Message });
            }
        }

        /// <summary>
        /// Update admin
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAdmin(int id, AdminUpdateDto adminDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { success = false, message = "Invalid data", errors = ModelState });
                }

                var admin = await _context.Admin.FindAsync(id);
                if (admin == null)
                {
                    return NotFound(new { success = false, message = "Admin not found" });
                }

                // Check if username already exists (excluding current admin)
                var existingAdmin = await _context.Admin
                    .FirstOrDefaultAsync(a => a.Username == adminDto.Username && a.Id != id);

                if (existingAdmin != null)
                {
                    return Conflict(new { success = false, message = "Username already exists" });
                }

                admin.Username = adminDto.Username;
                admin.Password = adminDto.Password;

                await _context.SaveChangesAsync();

                var response = new AdminResponseDto
                {
                    Id = admin.Id,
                    Username = admin.Username
                };

                return Ok(new { success = true, data = response, message = "Admin updated successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Internal server error", error = ex.Message });
            }
        }

        /// <summary>
        /// Delete admin
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAdmin(int id)
        {
            try
            {
                var admin = await _context.Admin.FindAsync(id);
                if (admin == null)
                {
                    return NotFound(new { success = false, message = "Admin not found" });
                }

                _context.Admin.Remove(admin);
                await _context.SaveChangesAsync();

                return Ok(new { success = true, message = "Admin deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Internal server error", error = ex.Message });
            }
        }
    }
}
