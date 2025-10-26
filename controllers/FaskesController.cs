using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sima_bpjs_api.Data;
using sima_bpjs_api.Models;

namespace sima_bpjs_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FaskesController : ControllerBase
    {
        private readonly AppDbContext _db;
        public FaskesController(AppDbContext db) { _db = db; }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? q, [FromQuery] string? status)
        {
            var query = _db.Faskes.AsQueryable();
            if (!string.IsNullOrWhiteSpace(q))
            {
                var lower = q.ToLower();
                query = query.Where(f => (f.Nama ?? "").ToLower().Contains(lower) || (f.Kota ?? "").ToLower().Contains(lower) || (f.Tipe ?? "").ToLower().Contains(lower));
            }
            if (!string.IsNullOrWhiteSpace(status))
            {
                query = query.Where(f => f.Status == status);
            }
            var data = await query.OrderBy(f => f.Nama).ToListAsync();
            return Ok(new { success = true, data });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _db.Faskes.FindAsync(id);
            if (item == null) return NotFound(new { success = false, message = "Faskes tidak ditemukan" });
            return Ok(new { success = true, data = item });
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Create([FromBody] Faskes dto)
        {
            _db.Faskes.Add(dto);
            await _db.SaveChangesAsync();
            return Ok(new { success = true, data = dto });
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Update(int id, [FromBody] Faskes dto)
        {
            var item = await _db.Faskes.FindAsync(id);
            if (item == null) return NotFound(new { success = false, message = "Faskes tidak ditemukan" });

            item.Nama = dto.Nama;
            item.Tipe = dto.Tipe;
            item.Alamat = dto.Alamat;
            item.Kota = dto.Kota;
            item.Provinsi = dto.Provinsi;
            item.Status = dto.Status;
            await _db.SaveChangesAsync();
            return Ok(new { success = true, data = item });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _db.Faskes.FindAsync(id);
            if (item == null) return NotFound(new { success = false, message = "Faskes tidak ditemukan" });
            _db.Faskes.Remove(item);
            await _db.SaveChangesAsync();
            return Ok(new { success = true });
        }

        [HttpPost("{id}/status")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> ChangeStatus(int id, [FromQuery] string status)
        {
            var item = await _db.Faskes.FindAsync(id);
            if (item == null) return NotFound(new { success = false, message = "Faskes tidak ditemukan" });
            item.Status = status;
            await _db.SaveChangesAsync();
            return Ok(new { success = true, data = item });
        }
    }
}


