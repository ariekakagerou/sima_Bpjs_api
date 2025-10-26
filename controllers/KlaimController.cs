using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sima_bpjs_api.Data;
using sima_bpjs_api.Models;

namespace sima_bpjs_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class KlaimController : ControllerBase
    {
        private readonly AppDbContext _db;
        public KlaimController(AppDbContext db) { _db = db; }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? status, [FromQuery] int? idFaskes, [FromQuery] int? idBpjs)
        {
            var query = _db.Klaim
                .Include(k => k.Faskes)
                .Include(k => k.Bpjs)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(status)) query = query.Where(k => k.Status == status);
            if (idFaskes.HasValue) query = query.Where(k => k.IdFaskes == idFaskes);
            if (idBpjs.HasValue) query = query.Where(k => k.IdBpjs == idBpjs);

            var data = await query.OrderByDescending(k => k.TanggalPengajuan).ToListAsync();
            return Ok(new { success = true, data });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _db.Klaim
                .Include(k => k.Faskes)
                .Include(k => k.Bpjs)
                .FirstOrDefaultAsync(k => k.IdKlaim == id);
            if (item == null) return NotFound(new { success = false, message = "Klaim tidak ditemukan" });
            return Ok(new { success = true, data = item });
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Create([FromBody] Klaim dto)
        {
            _db.Klaim.Add(dto);
            await _db.SaveChangesAsync();
            return Ok(new { success = true, data = dto });
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Update(int id, [FromBody] Klaim dto)
        {
            var item = await _db.Klaim.FindAsync(id);
            if (item == null) return NotFound(new { success = false, message = "Klaim tidak ditemukan" });

            item.IdBpjs = dto.IdBpjs;
            item.IdFaskes = dto.IdFaskes;
            item.TanggalPengajuan = dto.TanggalPengajuan;
            item.Diagnosis = dto.Diagnosis;
            item.Tindakan = dto.Tindakan;
            item.BiayaDiajukan = dto.BiayaDiajukan;
            item.BiayaDisetujui = dto.BiayaDisetujui;
            item.Status = dto.Status;
            item.Catatan = dto.Catatan;
            item.UpdatedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            return Ok(new { success = true, data = item });
        }

        [HttpPost("{id}/approve")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Approve(int id, [FromBody] decimal? biayaDisetujui)
        {
            var item = await _db.Klaim.FindAsync(id);
            if (item == null) return NotFound(new { success = false, message = "Klaim tidak ditemukan" });
            item.Status = "DISETUJUI";
            item.BiayaDisetujui = biayaDisetujui ?? item.BiayaDiajukan;
            item.UpdatedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            return Ok(new { success = true, data = item });
        }

        [HttpPost("{id}/deny")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Deny(int id, [FromBody] string? catatan)
        {
            var item = await _db.Klaim.FindAsync(id);
            if (item == null) return NotFound(new { success = false, message = "Klaim tidak ditemukan" });
            item.Status = "DITOLAK";
            item.Catatan = catatan;
            item.UpdatedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            return Ok(new { success = true, data = item });
        }
    }
}


