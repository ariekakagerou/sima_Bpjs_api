using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sima_bpjs_api.Data;
using sima_bpjs_api.Models;

namespace sima_bpjs_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AduanController : ControllerBase
    {
        private readonly AppDbContext _db;
        public AduanController(AppDbContext db) { _db = db; }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? status)
        {
            var query = _db.Aduan.AsQueryable();
            if (!string.IsNullOrWhiteSpace(status)) query = query.Where(a => a.Status == status);
            var data = await query.OrderByDescending(a => a.CreatedAt).ToListAsync();
            return Ok(new { success = true, data });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _db.Aduan.FindAsync(id);
            if (item == null) return NotFound(new { success = false, message = "Aduan tidak ditemukan" });
            return Ok(new { success = true, data = item });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Aduan dto)
        {
            _db.Aduan.Add(dto);
            await _db.SaveChangesAsync();
            return Ok(new { success = true, data = dto });
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Update(int id, [FromBody] Aduan dto)
        {
            var item = await _db.Aduan.FindAsync(id);
            if (item == null) return NotFound(new { success = false, message = "Aduan tidak ditemukan" });

            item.IdBpjs = dto.IdBpjs;
            item.NamaPengadu = dto.NamaPengadu;
            item.NoKartu = dto.NoKartu;
            item.Kategori = dto.Kategori;
            item.Deskripsi = dto.Deskripsi;
            item.Status = dto.Status;
            item.Prioritas = dto.Prioritas;
            item.AssignedToUserId = dto.AssignedToUserId;
            item.UpdatedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            return Ok(new { success = true, data = item });
        }

        [HttpPost("{id}/status")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> ChangeStatus(int id, [FromQuery] string status)
        {
            var item = await _db.Aduan.FindAsync(id);
            if (item == null) return NotFound(new { success = false, message = "Aduan tidak ditemukan" });
            item.Status = status;
            if (status == "SELESAI") item.TanggalSelesai = DateTime.UtcNow;
            item.UpdatedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            return Ok(new { success = true, data = item });
        }
    }
}


