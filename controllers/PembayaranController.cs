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
    public class PembayaranController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PembayaranController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Pembayaran
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PembayaranBpjsResponseDto>>> GetAll()
        {
            try
            {
                var pembayaranList = await _context.PembayaranBpjs
                    .Include(p => p.Bpjs)
                    .ThenInclude(b => b.KtpKk)
                    .Select(p => new PembayaranBpjsResponseDto
                    {
                        IdPembayaran = p.IdPembayaran,
                        IdBpjs = p.IdBpjs,
                        Bulan = p.Bulan,
                        Tahun = p.Tahun,
                        Jumlah = p.Jumlah,
                        StatusPembayaran = p.StatusPembayaran,
                        TanggalBayar = p.TanggalBayar,
                        Bpjs = p.Bpjs != null ? new BpjsResponseDto
                        {
                            IdBpjs = p.Bpjs.IdBpjs,
                            Nik = p.Bpjs.Nik,
                            NoBpjs = p.Bpjs.NoBpjs,
                            FaskesUtama = p.Bpjs.FaskesUtama,
                            KelasPerawatan = p.Bpjs.KelasPerawatan,
                            StatusPeserta = p.Bpjs.StatusPeserta,
                            TanggalDaftar = p.Bpjs.TanggalDaftar,
                            KtpKk = p.Bpjs.KtpKk != null ? new KtpKkResponseDto
                            {
                                Nik = p.Bpjs.KtpKk.Nik,
                                NoKk = p.Bpjs.KtpKk.NoKk,
                                NamaLengkap = p.Bpjs.KtpKk.NamaLengkap,
                                TempatLahir = p.Bpjs.KtpKk.TempatLahir,
                                TanggalLahir = p.Bpjs.KtpKk.TanggalLahir,
                                JenisKelamin = p.Bpjs.KtpKk.JenisKelamin,
                                Alamat = p.Bpjs.KtpKk.Alamat,
                                Agama = p.Bpjs.KtpKk.Agama,
                                StatusPerkawinan = p.Bpjs.KtpKk.StatusPerkawinan,
                                Pekerjaan = p.Bpjs.KtpKk.Pekerjaan,
                                Kewarganegaraan = p.Bpjs.KtpKk.Kewarganegaraan,
                                CreatedAt = p.Bpjs.KtpKk.CreatedAt
                            } : null
                        } : null
                    })
                    .ToListAsync();

                return Ok(new { success = true, data = pembayaranList, message = "Payment data retrieved successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Internal server error", error = ex.Message });
            }
        }

        // GET: api/Pembayaran/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PembayaranBpjsResponseDto>> GetById(int id)
        {
            try
            {
                var data = await _context.PembayaranBpjs
                    .Include(p => p.Bpjs)
                    .ThenInclude(b => b.KtpKk)
                    .Where(p => p.IdPembayaran == id)
                    .Select(p => new PembayaranBpjsResponseDto
                    {
                        IdPembayaran = p.IdPembayaran,
                        IdBpjs = p.IdBpjs,
                        Bulan = p.Bulan,
                        Tahun = p.Tahun,
                        Jumlah = p.Jumlah,
                        StatusPembayaran = p.StatusPembayaran,
                        TanggalBayar = p.TanggalBayar,
                        Bpjs = p.Bpjs != null ? new BpjsResponseDto
                        {
                            IdBpjs = p.Bpjs.IdBpjs,
                            Nik = p.Bpjs.Nik,
                            NoBpjs = p.Bpjs.NoBpjs,
                            FaskesUtama = p.Bpjs.FaskesUtama,
                            KelasPerawatan = p.Bpjs.KelasPerawatan,
                            StatusPeserta = p.Bpjs.StatusPeserta,
                            TanggalDaftar = p.Bpjs.TanggalDaftar,
                            KtpKk = p.Bpjs.KtpKk != null ? new KtpKkResponseDto
                            {
                                Nik = p.Bpjs.KtpKk.Nik,
                                NoKk = p.Bpjs.KtpKk.NoKk,
                                NamaLengkap = p.Bpjs.KtpKk.NamaLengkap,
                                TempatLahir = p.Bpjs.KtpKk.TempatLahir,
                                TanggalLahir = p.Bpjs.KtpKk.TanggalLahir,
                                JenisKelamin = p.Bpjs.KtpKk.JenisKelamin,
                                Alamat = p.Bpjs.KtpKk.Alamat,
                                Agama = p.Bpjs.KtpKk.Agama,
                                StatusPerkawinan = p.Bpjs.KtpKk.StatusPerkawinan,
                                Pekerjaan = p.Bpjs.KtpKk.Pekerjaan,
                                Kewarganegaraan = p.Bpjs.KtpKk.Kewarganegaraan,
                                CreatedAt = p.Bpjs.KtpKk.CreatedAt
                            } : null
                        } : null
                    })
                    .FirstOrDefaultAsync();

                if (data == null)
                {
                    return NotFound(new { success = false, message = "Payment data not found" });
                }

                return Ok(new { success = true, data = data, message = "Payment data retrieved successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Internal server error", error = ex.Message });
            }
        }

        // POST: api/Pembayaran
        [HttpPost]
        public async Task<ActionResult<PembayaranBpjsResponseDto>> Create(PembayaranBpjsCreateDto pembayaranDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { success = false, message = "Invalid data", errors = ModelState });
                }

                // Check if BPJS record exists
                var bpjs = await _context.Bpjs.FindAsync(pembayaranDto.IdBpjs);
                if (bpjs == null)
                {
                    return NotFound(new { success = false, message = "BPJS record not found" });
                }

                // Check if payment already exists for the same month and year
                var existingPayment = await _context.PembayaranBpjs
                    .FirstOrDefaultAsync(p => p.IdBpjs == pembayaranDto.IdBpjs && 
                                            p.Bulan == pembayaranDto.Bulan && 
                                            p.Tahun == pembayaranDto.Tahun);

                if (existingPayment != null)
                {
                    return Conflict(new { success = false, message = "Payment already exists for this month and year" });
                }

                var pembayaran = new PembayaranBpjs
                {
                    IdBpjs = pembayaranDto.IdBpjs,
                    Bulan = pembayaranDto.Bulan,
                    Tahun = pembayaranDto.Tahun,
                    Jumlah = pembayaranDto.Jumlah,
                    StatusPembayaran = pembayaranDto.StatusPembayaran,
                    TanggalBayar = pembayaranDto.TanggalBayar
                };

                _context.PembayaranBpjs.Add(pembayaran);
                await _context.SaveChangesAsync();

                var response = new PembayaranBpjsResponseDto
                {
                    IdPembayaran = pembayaran.IdPembayaran,
                    IdBpjs = pembayaran.IdBpjs,
                    Bulan = pembayaran.Bulan,
                    Tahun = pembayaran.Tahun,
                    Jumlah = pembayaran.Jumlah,
                    StatusPembayaran = pembayaran.StatusPembayaran,
                    TanggalBayar = pembayaran.TanggalBayar
                };

                return CreatedAtAction(nameof(GetById), new { id = pembayaran.IdPembayaran }, 
                    new { success = true, data = response, message = "Payment created successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Internal server error", error = ex.Message });
            }
        }

        // PUT: api/Pembayaran/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, PembayaranBpjsUpdateDto pembayaranDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { success = false, message = "Invalid data", errors = ModelState });
                }

                var pembayaran = await _context.PembayaranBpjs.FindAsync(id);
                if (pembayaran == null)
                {
                    return NotFound(new { success = false, message = "Payment data not found" });
                }

                // Check if payment already exists for the same month and year (excluding current record)
                var existingPayment = await _context.PembayaranBpjs
                    .FirstOrDefaultAsync(p => p.IdBpjs == pembayaran.IdBpjs && 
                                            p.Bulan == pembayaranDto.Bulan && 
                                            p.Tahun == pembayaranDto.Tahun &&
                                            p.IdPembayaran != id);

                if (existingPayment != null)
                {
                    return Conflict(new { success = false, message = "Payment already exists for this month and year" });
                }

                pembayaran.Bulan = pembayaranDto.Bulan;
                pembayaran.Tahun = pembayaranDto.Tahun;
                pembayaran.Jumlah = pembayaranDto.Jumlah;
                pembayaran.StatusPembayaran = pembayaranDto.StatusPembayaran;
                pembayaran.TanggalBayar = pembayaranDto.TanggalBayar;

                await _context.SaveChangesAsync();

                var response = new PembayaranBpjsResponseDto
                {
                    IdPembayaran = pembayaran.IdPembayaran,
                    IdBpjs = pembayaran.IdBpjs,
                    Bulan = pembayaran.Bulan,
                    Tahun = pembayaran.Tahun,
                    Jumlah = pembayaran.Jumlah,
                    StatusPembayaran = pembayaran.StatusPembayaran,
                    TanggalBayar = pembayaran.TanggalBayar
                };

                return Ok(new { success = true, data = response, message = "Payment updated successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Internal server error", error = ex.Message });
            }
        }

        // DELETE: api/Pembayaran/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var pembayaran = await _context.PembayaranBpjs.FindAsync(id);
                if (pembayaran == null)
                {
                    return NotFound(new { success = false, message = "Payment data not found" });
                }

                _context.PembayaranBpjs.Remove(pembayaran);
                await _context.SaveChangesAsync();

                return Ok(new { success = true, message = "Payment deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Internal server error", error = ex.Message });
            }
        }
    }
}
