using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sima_bpjs_api.Data;
using sima_bpjs_api.Models;
using sima_bpjs_api.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;

namespace sima_bpjs_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BpjsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IHttpClientFactory _httpClientFactory;

        public BpjsController(AppDbContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BpjsResponseDto>>> GetAll()
        {
            try
            {
                var bpjsList = await _context.Bpjs
                    .Include(b => b.KtpKk)
                    .Select(b => new BpjsResponseDto
                    {
                        IdBpjs = b.IdBpjs,
                        Nik = b.Nik,
                        NoBpjs = b.NoBpjs,
                        FaskesUtama = b.FaskesUtama,
                        KelasPerawatan = b.KelasPerawatan,
                        StatusPeserta = b.StatusPeserta,
                        TanggalDaftar = b.TanggalDaftar,
                        KtpKk = b.KtpKk != null ? new KtpKkResponseDto
                        {
                            Nik = b.KtpKk.Nik,
                            NoKk = b.KtpKk.NoKk,
                            NamaLengkap = b.KtpKk.NamaLengkap,
                            TempatLahir = b.KtpKk.TempatLahir,
                            TanggalLahir = b.KtpKk.TanggalLahir,
                            JenisKelamin = b.KtpKk.JenisKelamin,
                            Alamat = b.KtpKk.Alamat,
                            Agama = b.KtpKk.Agama,
                            StatusPerkawinan = b.KtpKk.StatusPerkawinan,
                            Pekerjaan = b.KtpKk.Pekerjaan,
                            Kewarganegaraan = b.KtpKk.Kewarganegaraan,
                            CreatedAt = b.KtpKk.CreatedAt
                        } : null
                    })
                    .ToListAsync();

                return Ok(new { success = true, data = bpjsList, message = "BPJS data retrieved successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Internal server error", error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BpjsResponseDto>> GetById(int id)
        {
            try
            {
                var bpjs = await _context.Bpjs
                    .Include(b => b.KtpKk)
                    .Include(b => b.Pembayaran)
                    .Where(b => b.IdBpjs == id)
                    .Select(b => new BpjsResponseDto
                    {
                        IdBpjs = b.IdBpjs,
                        Nik = b.Nik,
                        NoBpjs = b.NoBpjs,
                        FaskesUtama = b.FaskesUtama,
                        KelasPerawatan = b.KelasPerawatan,
                        StatusPeserta = b.StatusPeserta,
                        TanggalDaftar = b.TanggalDaftar,
                        KtpKk = b.KtpKk != null ? new KtpKkResponseDto
                        {
                            Nik = b.KtpKk.Nik,
                            NoKk = b.KtpKk.NoKk,
                            NamaLengkap = b.KtpKk.NamaLengkap,
                            TempatLahir = b.KtpKk.TempatLahir,
                            TanggalLahir = b.KtpKk.TanggalLahir,
                            JenisKelamin = b.KtpKk.JenisKelamin,
                            Alamat = b.KtpKk.Alamat,
                            Agama = b.KtpKk.Agama,
                            StatusPerkawinan = b.KtpKk.StatusPerkawinan,
                            Pekerjaan = b.KtpKk.Pekerjaan,
                            Kewarganegaraan = b.KtpKk.Kewarganegaraan,
                            CreatedAt = b.KtpKk.CreatedAt
                        } : null
                    })
                    .FirstOrDefaultAsync();

                if (bpjs == null)
                {
                    return NotFound(new { success = false, message = "BPJS data not found" });
                }

                return Ok(new { success = true, data = bpjs, message = "BPJS data retrieved successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Internal server error", error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<BpjsResponseDto>> Create(sima_bpjs_api.Models.DTOs.BpjsCreateDto bpjsDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { success = false, message = "Invalid data", errors = ModelState });
                }

                if (!IsValidKelas(bpjsDto.KelasPerawatan))
                {
                    return BadRequest(new { success = false, message = "Kelas Perawatan harus KELAS I / KELAS II / KELAS III" });
                }

            // Ambil data KTP dari lokal atau eksternal, dan validasi field penting
            KtpKk? ktp = await _context.KtpKk.FirstOrDefaultAsync(k => k.Nik == bpjsDto.Nik);
            JsonElement? externalMatch = null;

            if (ktp == null)
            {
                var client = _httpClientFactory.CreateClient("KtpApi");
                var httpResponse = await client.GetAsync("api/ktp/all");
                if (!httpResponse.IsSuccessStatusCode)
                {
                    return BadRequest(new { message = "Tidak bisa mengakses layanan KTP eksternal" });
                }

                using var stream = await httpResponse.Content.ReadAsStreamAsync();
                using var doc = await JsonDocument.ParseAsync(stream);
                var data = doc.RootElement.GetProperty("data");
                var match = data.EnumerateArray().FirstOrDefault(x => x.GetProperty("nik").GetString() == bpjsDto.Nik);
                if (match.ValueKind == JsonValueKind.Undefined)
                {
                    return BadRequest(new { message = $"NIK {bpjsDto.Nik} tidak ditemukan pada layanan KTP eksternal" });
                }
                externalMatch = match;

                // Sinkronkan minimal data KTP ke database lokal agar FK terpenuhi
                ktp = new KtpKk
                {
                    Nik = match.GetProperty("nik").GetString() ?? bpjsDto.Nik,
                    NoKk = "0000000000000000",
                    NamaLengkap = match.TryGetProperty("nama_lengkap", out var nama) ? (nama.GetString() ?? "-") : "-",
                    TempatLahir = match.TryGetProperty("tempat_lahir", out var tempat) ? (tempat.GetString() ?? "-") : "-",
                    TanggalLahir = match.TryGetProperty("tanggal_lahir", out var tgl) && DateTime.TryParse(tgl.GetString(), out var dt) ? dt : null,
                    JenisKelamin = match.TryGetProperty("jenis_kelamin", out var jk) ? (jk.GetString() ?? "-") : "-",
                    Alamat = match.TryGetProperty("alamat", out var alamat) ? (alamat.GetString() ?? "-") : "-",
                    Agama = match.TryGetProperty("agama", out var agama) ? (agama.GetString() ?? "-") : "-",
                    StatusPerkawinan = match.TryGetProperty("status_perkawinan", out var sp) ? (sp.GetString() ?? "-") : "-",
                    Pekerjaan = match.TryGetProperty("pekerjaan", out var pk) ? (pk.GetString() ?? "-") : "-",
                    Kewarganegaraan = match.TryGetProperty("kewarganegaraan", out var kew) ? (kew.GetString() ?? "-") : "-"
                };

                _context.KtpKk.Add(ktp);
                await _context.SaveChangesAsync();
            }

            // Validasi kolom penting terhadap sumber data (lokal atau eksternal)
            // Nama, Alamat, Tanggal Lahir
            string? refNama = ktp.NamaLengkap;
            string? refAlamat = ktp.Alamat;
            DateTime? refTanggalLahir = ktp.TanggalLahir;

            // Jika ingin ketat ke data eksternal terbaru, boleh override saat externalMatch != null
            if (externalMatch.HasValue)
            {
                var m = externalMatch.Value;
                if (m.TryGetProperty("nama_lengkap", out var nn)) refNama = nn.GetString();
                if (m.TryGetProperty("alamat", out var aa)) refAlamat = aa.GetString();
                if (m.TryGetProperty("tanggal_lahir", out var tt) && DateTime.TryParse(tt.GetString(), out var dt2)) refTanggalLahir = dt2;
            }

            // Nomor BPJS otomatis dan status awal PENDING
            var bpjs = new Bpjs
            {
                Nik = bpjsDto.Nik,
                NoBpjs = GenerateNoBpjs(bpjsDto.Nik),
                FaskesUtama = bpjsDto.FaskesUtama,
                KelasPerawatan = bpjsDto.KelasPerawatan,
                StatusPeserta = "PENDING",
                TanggalDaftar = bpjsDto.TanggalDaftar
            };

                _context.Bpjs.Add(bpjs);
                await _context.SaveChangesAsync();

                var response = new BpjsResponseDto
                {
                    IdBpjs = bpjs.IdBpjs,
                    Nik = bpjs.Nik,
                    NoBpjs = bpjs.NoBpjs,
                    FaskesUtama = bpjs.FaskesUtama,
                    KelasPerawatan = bpjs.KelasPerawatan,
                    StatusPeserta = bpjs.StatusPeserta,
                    TanggalDaftar = bpjs.TanggalDaftar,
                    KtpKk = ktp != null ? new KtpKkResponseDto
                    {
                        Nik = ktp.Nik,
                        NoKk = ktp.NoKk,
                        NamaLengkap = ktp.NamaLengkap,
                        TempatLahir = ktp.TempatLahir,
                        TanggalLahir = ktp.TanggalLahir,
                        JenisKelamin = ktp.JenisKelamin,
                        Alamat = ktp.Alamat,
                        Agama = ktp.Agama,
                        StatusPerkawinan = ktp.StatusPerkawinan,
                        Pekerjaan = ktp.Pekerjaan,
                        Kewarganegaraan = ktp.Kewarganegaraan,
                        CreatedAt = ktp.CreatedAt
                    } : null
                };

                return CreatedAtAction(nameof(GetById), new { id = bpjs.IdBpjs }, 
                    new { success = true, data = response, message = "BPJS data created successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Internal server error", error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, BpjsUpdateDto bpjsDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { success = false, message = "Invalid data", errors = ModelState });
                }

                if (!IsValidKelas(bpjsDto.KelasPerawatan))
                {
                    return BadRequest(new { success = false, message = "Kelas Perawatan harus KELAS I / KELAS II / KELAS III" });
                }

                var bpjs = await _context.Bpjs.FindAsync(id);
                if (bpjs == null)
                {
                    return NotFound(new { success = false, message = "BPJS data not found" });
                }

                // Check if NoBpjs already exists (excluding current record)
                var existingBpjs = await _context.Bpjs
                    .FirstOrDefaultAsync(b => b.NoBpjs == bpjsDto.NoBpjs && b.IdBpjs != id);

                if (existingBpjs != null)
                {
                    return Conflict(new { success = false, message = "No BPJS already exists" });
                }

                bpjs.NoBpjs = bpjsDto.NoBpjs;
                bpjs.FaskesUtama = bpjsDto.FaskesUtama;
                bpjs.KelasPerawatan = bpjsDto.KelasPerawatan;
                bpjs.StatusPeserta = bpjsDto.StatusPeserta;
                bpjs.TanggalDaftar = bpjsDto.TanggalDaftar;

                await _context.SaveChangesAsync();

                var response = new BpjsResponseDto
                {
                    IdBpjs = bpjs.IdBpjs,
                    Nik = bpjs.Nik,
                    NoBpjs = bpjs.NoBpjs,
                    FaskesUtama = bpjs.FaskesUtama,
                    KelasPerawatan = bpjs.KelasPerawatan,
                    StatusPeserta = bpjs.StatusPeserta,
                    TanggalDaftar = bpjs.TanggalDaftar
                };

                return Ok(new { success = true, data = response, message = "BPJS data updated successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Internal server error", error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var bpjs = await _context.Bpjs.FindAsync(id);
                if (bpjs == null)
                {
                    return NotFound(new { success = false, message = "BPJS data not found" });
                }

                // Check if there are any related payments
                var hasPayments = await _context.PembayaranBpjs
                    .AnyAsync(p => p.IdBpjs == id);

                if (hasPayments)
                {
                    return BadRequest(new { success = false, message = "Cannot delete BPJS data with existing payments. Please delete payments first." });
                }

                _context.Bpjs.Remove(bpjs);
                await _context.SaveChangesAsync();

                return Ok(new { success = true, message = "BPJS data deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Internal server error", error = ex.Message });
            }
        }

        // Admin approves participant -> AKTIF
        [HttpPost("{id}/approve")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Approve(int id)
        {
            var bpjs = await _context.Bpjs.FindAsync(id);
            if (bpjs == null) return NotFound();
            bpjs.StatusPeserta = "AKTIF";
            await _context.SaveChangesAsync();
            return Ok(new { message = "Peserta disetujui", id = id, status = bpjs.StatusPeserta });
        }

        // Admin deactivates participant -> NONAKTIF
        [HttpPost("{id}/deactivate")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Deactivate(int id)
        {
            var bpjs = await _context.Bpjs.FindAsync(id);
            if (bpjs == null) return NotFound();
            bpjs.StatusPeserta = "NONAKTIF";
            await _context.SaveChangesAsync();
            return Ok(new { message = "Peserta dinonaktifkan", id = id, status = bpjs.StatusPeserta });
        }

        private static bool IsValidKelas(string kelas)
        {
            return kelas == "KELAS I" || kelas == "KELAS II" || kelas == "KELAS III";
        }

        private static string GenerateNoBpjs(string nik)
        {
            // Contoh sederhana: 13 digit -> 6 digit prefix waktu + 7 digit checksum nik
            var ts = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
            var prefix = ts.Substring(ts.Length - 6);
            var last7 = nik.Length >= 7 ? nik.Substring(nik.Length - 7) : nik.PadLeft(7, '0');
            return prefix + last7;
        }
    }
}
