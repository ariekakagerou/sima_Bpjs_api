using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sima_bpjs_api.Data;
using sima_bpjs_api.Models;
using sima_bpjs_api.Models.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace sima_bpjs_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class KtpKkController : ControllerBase
    {
        private readonly AppDbContext _context;

        public KtpKkController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/KtpKk
        [HttpGet]
        public async Task<ActionResult<IEnumerable<KtpKkResponseDto>>> GetAll()
        {
            try
            {
                var ktpKkList = await _context.KtpKk
                    .Select(k => new KtpKkResponseDto
                    {
                        Nik = k.Nik!,
                        NoKk = k.NoKk!,
                        NamaLengkap = k.NamaLengkap!,
                        TempatLahir = k.TempatLahir!,
                        TanggalLahir = k.TanggalLahir,
                        JenisKelamin = k.JenisKelamin!,
                        Alamat = k.Alamat!,
                        Agama = k.Agama!,
                        StatusPerkawinan = k.StatusPerkawinan!,
                        Pekerjaan = k.Pekerjaan!,
                        Kewarganegaraan = k.Kewarganegaraan!,
                        CreatedAt = k.CreatedAt
                    })
                    .ToListAsync();

                return Ok(new { success = true, data = ktpKkList, message = "KTP/KK data retrieved successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Internal server error", error = ex.Message });
            }
        }

        // GET: api/KtpKk/1234567890123456
        [HttpGet("{nik}")]
        public async Task<ActionResult<KtpKkResponseDto>> GetByNik(string nik)
        {
            try
            {
                var data = await _context.KtpKk
                    .Where(k => k.Nik == nik)
                    .Select(k => new KtpKkResponseDto
                    {
                        Nik = k.Nik!,
                        NoKk = k.NoKk!,
                        NamaLengkap = k.NamaLengkap!,
                        TempatLahir = k.TempatLahir!,
                        TanggalLahir = k.TanggalLahir,
                        JenisKelamin = k.JenisKelamin!,
                        Alamat = k.Alamat!,
                        Agama = k.Agama!,
                        StatusPerkawinan = k.StatusPerkawinan!,
                        Pekerjaan = k.Pekerjaan!,
                        Kewarganegaraan = k.Kewarganegaraan!,
                        CreatedAt = k.CreatedAt
                    })
                    .FirstOrDefaultAsync();

                if (data == null)
                {
                    return NotFound(new { success = false, message = "KTP/KK data not found" });
                }

                return Ok(new { success = true, data = data, message = "KTP/KK data retrieved successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Internal server error", error = ex.Message });
            }
        }

        // POST: api/KtpKk
        [HttpPost]
        public async Task<ActionResult<KtpKkResponseDto>> Create(KtpKkCreateDto ktpKkDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { success = false, message = "Invalid data", errors = ModelState });
                }

                // Check if NIK already exists
                var existingKtp = await _context.KtpKk
                    .FirstOrDefaultAsync(k => k.Nik == ktpKkDto.Nik);

                if (existingKtp != null)
                {
                    return Conflict(new { success = false, message = "NIK already exists" });
                }

                var ktpKk = new KtpKk
                {
                    Nik = ktpKkDto.Nik!,
                    NoKk = ktpKkDto.NoKk!,
                    NamaLengkap = ktpKkDto.NamaLengkap!,
                    TempatLahir = ktpKkDto.TempatLahir!,
                    TanggalLahir = ktpKkDto.TanggalLahir,
                    JenisKelamin = ktpKkDto.JenisKelamin!,
                    Alamat = ktpKkDto.Alamat!,
                    Agama = ktpKkDto.Agama!,
                    StatusPerkawinan = ktpKkDto.StatusPerkawinan!,
                    Pekerjaan = ktpKkDto.Pekerjaan!,
                    Kewarganegaraan = ktpKkDto.Kewarganegaraan!,
                    CreatedAt = DateTime.UtcNow
                };

                _context.KtpKk.Add(ktpKk);
                await _context.SaveChangesAsync();

                var response = new KtpKkResponseDto
                {
                    Nik = ktpKk.Nik!,
                    NoKk = ktpKk.NoKk!,
                    NamaLengkap = ktpKk.NamaLengkap!,
                    TempatLahir = ktpKk.TempatLahir!,
                    TanggalLahir = ktpKk.TanggalLahir,
                    JenisKelamin = ktpKk.JenisKelamin!,
                    Alamat = ktpKk.Alamat!,
                    Agama = ktpKk.Agama!,
                    StatusPerkawinan = ktpKk.StatusPerkawinan!,
                    Pekerjaan = ktpKk.Pekerjaan!,
                    Kewarganegaraan = ktpKk.Kewarganegaraan!,
                    CreatedAt = ktpKk.CreatedAt
                };

                return CreatedAtAction(nameof(GetByNik), new { nik = ktpKk.Nik }, 
                    new { success = true, data = response, message = "KTP/KK data created successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Internal server error", error = ex.Message });
            }
        }

        // PUT: api/KtpKk/1234567890123456
        [HttpPut("{nik}")]
        public async Task<IActionResult> Update(string nik, KtpKkUpdateDto ktpKkDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { success = false, message = "Invalid data", errors = ModelState });
                }

                var ktpKk = await _context.KtpKk.FindAsync(nik);
                if (ktpKk == null)
                {
                    return NotFound(new { success = false, message = "KTP/KK data not found" });
                }

                ktpKk.NoKk = ktpKkDto.NoKk;
                ktpKk.NamaLengkap = ktpKkDto.NamaLengkap;
                ktpKk.TempatLahir = ktpKkDto.TempatLahir;
                ktpKk.TanggalLahir = ktpKkDto.TanggalLahir;
                ktpKk.JenisKelamin = ktpKkDto.JenisKelamin;
                ktpKk.Alamat = ktpKkDto.Alamat;
                ktpKk.Agama = ktpKkDto.Agama;
                ktpKk.StatusPerkawinan = ktpKkDto.StatusPerkawinan;
                ktpKk.Pekerjaan = ktpKkDto.Pekerjaan;
                ktpKk.Kewarganegaraan = ktpKkDto.Kewarganegaraan;

                await _context.SaveChangesAsync();

                var response = new KtpKkResponseDto
                {
                    Nik = ktpKk.Nik!,
                    NoKk = ktpKk.NoKk!,
                    NamaLengkap = ktpKk.NamaLengkap!,
                    TempatLahir = ktpKk.TempatLahir!,
                    TanggalLahir = ktpKk.TanggalLahir,
                    JenisKelamin = ktpKk.JenisKelamin!,
                    Alamat = ktpKk.Alamat!,
                    Agama = ktpKk.Agama!,
                    StatusPerkawinan = ktpKk.StatusPerkawinan!,
                    Pekerjaan = ktpKk.Pekerjaan!,
                    Kewarganegaraan = ktpKk.Kewarganegaraan!,
                    CreatedAt = ktpKk.CreatedAt
                };

                return Ok(new { success = true, data = response, message = "KTP/KK data updated successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Internal server error", error = ex.Message });
            }
        }

        // DELETE: api/KtpKk/1234567890123456
        [HttpDelete("{nik}")]
        public async Task<IActionResult> Delete(string nik)
        {
            try
            {
                var ktpKk = await _context.KtpKk.FindAsync(nik);
                if (ktpKk == null)
                {
                    return NotFound(new { success = false, message = "KTP/KK data not found" });
                }

                // Check if there are any related BPJS records
                var hasBpjs = await _context.Bpjs
                    .AnyAsync(b => b.Nik == nik);

                if (hasBpjs)
                {
                    return BadRequest(new { success = false, message = "Cannot delete KTP/KK data with existing BPJS records. Please delete BPJS records first." });
                }

                _context.KtpKk.Remove(ktpKk);
                await _context.SaveChangesAsync();

                return Ok(new { success = true, message = "KTP/KK data deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Internal server error", error = ex.Message });
            }
        }
    }
}
