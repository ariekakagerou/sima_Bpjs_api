using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sima_bpjs_api.Data;
using sima_bpjs_api.Models;
using sima_bpjs_api.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace sima_bpjs_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginActivityController : ControllerBase
    {
        private readonly AppDbContext _context;

        public LoginActivityController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all login activities (Admin only)
        /// </summary>
        [HttpGet]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<IEnumerable<LoginActivityDto>>> GetAllLoginActivities(
            [FromQuery] int page = 1,
            [FromQuery] int limit = 10)
        {
            try
            {
                var query = _context.LoginActivities
                    .OrderByDescending(l => l.LoginTime)
                    .AsQueryable();

                var total = await query.CountAsync();
                var activities = await query
                    .Skip((page - 1) * limit)
                    .Take(limit)
                    .Select(l => new LoginActivityDto
                    {
                        Id = l.Id,
                        Username = l.Username,
                        IpAddress = l.IpAddress,
                        Device = l.Device,
                        Browser = l.Browser,
                        Status = l.Status,
                        LoginTime = l.LoginTime
                    })
                    .ToListAsync();

                return Ok(new
                {
                    success = true,
                    data = activities,
                    total = total,
                    page = page,
                    limit = limit,
                    message = "Login activities retrieved successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Internal server error", error = ex.Message });
            }
        }

        /// <summary>
        /// Get login activities for current user
        /// </summary>
        [HttpGet("my-activities")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<LoginActivityDto>>> GetMyLoginActivities(
            [FromQuery] int limit = 10)
        {
            try
            {
                var username = User.FindFirst(ClaimTypes.Name)?.Value;
                if (string.IsNullOrEmpty(username))
                {
                    return Unauthorized(new { success = false, message = "User not authenticated" });
                }

                var activities = await _context.LoginActivities
                    .Where(l => l.Username == username)
                    .OrderByDescending(l => l.LoginTime)
                    .Take(limit)
                    .Select(l => new LoginActivityDto
                    {
                        Id = l.Id,
                        Username = l.Username,
                        IpAddress = l.IpAddress,
                        Device = l.Device,
                        Browser = l.Browser,
                        Status = l.Status,
                        LoginTime = l.LoginTime
                    })
                    .ToListAsync();

                return Ok(new
                {
                    success = true,
                    data = activities,
                    message = "Login activities retrieved successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Internal server error", error = ex.Message });
            }
        }

        /// <summary>
        /// Get login activities by username (Admin only)
        /// </summary>
        [HttpGet("user/{username}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<IEnumerable<LoginActivityDto>>> GetLoginActivitiesByUsername(
            string username,
            [FromQuery] int limit = 10)
        {
            try
            {
                var activities = await _context.LoginActivities
                    .Where(l => l.Username == username)
                    .OrderByDescending(l => l.LoginTime)
                    .Take(limit)
                    .Select(l => new LoginActivityDto
                    {
                        Id = l.Id,
                        Username = l.Username,
                        IpAddress = l.IpAddress,
                        Device = l.Device,
                        Browser = l.Browser,
                        Status = l.Status,
                        LoginTime = l.LoginTime
                    })
                    .ToListAsync();

                return Ok(new
                {
                    success = true,
                    data = activities,
                    message = "Login activities retrieved successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Internal server error", error = ex.Message });
            }
        }

        /// <summary>
        /// Create login activity (Internal use by AuthController)
        /// </summary>
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<LoginActivityDto>> CreateLoginActivity([FromBody] LoginActivityCreateDto activityDto)
        {
            try
            {
                var activity = new LoginActivity
                {
                    UserId = activityDto.UserId,
                    Username = activityDto.Username,
                    IpAddress = activityDto.IpAddress,
                    Device = activityDto.Device,
                    Browser = activityDto.Browser,
                    Status = activityDto.Status,
                    LoginTime = DateTime.Now,
                    CreatedAt = DateTime.Now
                };

                _context.LoginActivities.Add(activity);
                await _context.SaveChangesAsync();

                var response = new LoginActivityDto
                {
                    Id = activity.Id,
                    Username = activity.Username,
                    IpAddress = activity.IpAddress,
                    Device = activity.Device,
                    Browser = activity.Browser,
                    Status = activity.Status,
                    LoginTime = activity.LoginTime
                };

                return Ok(new { success = true, data = response, message = "Login activity recorded" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Internal server error", error = ex.Message });
            }
        }

        /// <summary>
        /// Delete login activity (Admin only)
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> DeleteLoginActivity(int id)
        {
            try
            {
                var activity = await _context.LoginActivities.FindAsync(id);
                if (activity == null)
                {
                    return NotFound(new { success = false, message = "Login activity not found" });
                }

                _context.LoginActivities.Remove(activity);
                await _context.SaveChangesAsync();

                return Ok(new { success = true, message = "Login activity deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Internal server error", error = ex.Message });
            }
        }

        /// <summary>
        /// Clear old login activities (Admin only)
        /// </summary>
        [HttpDelete("clear-old")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> ClearOldActivities([FromQuery] int daysOld = 30)
        {
            try
            {
                var cutoffDate = DateTime.Now.AddDays(-daysOld);
                var oldActivities = await _context.LoginActivities
                    .Where(l => l.LoginTime < cutoffDate)
                    .ToListAsync();

                _context.LoginActivities.RemoveRange(oldActivities);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    success = true,
                    message = $"Deleted {oldActivities.Count} old login activities",
                    deletedCount = oldActivities.Count
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Internal server error", error = ex.Message });
            }
        }
    }
}

