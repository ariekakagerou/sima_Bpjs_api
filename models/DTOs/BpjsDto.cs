using System.ComponentModel.DataAnnotations;

namespace sima_bpjs_api.Models.DTOs
{
    public class BpjsCreateDto
    {
        [Required(ErrorMessage = "NIK is required")]
        [StringLength(16, MinimumLength = 16, ErrorMessage = "NIK must be exactly 16 characters")]
        public string Nik { get; set; } = null!;

        [Required(ErrorMessage = "No BPJS is required")]
        [MaxLength(20, ErrorMessage = "No BPJS cannot exceed 20 characters")]
        public string NoBpjs { get; set; } = null!;

        [Required(ErrorMessage = "Faskes Utama is required")]
        [MaxLength(100, ErrorMessage = "Faskes Utama cannot exceed 100 characters")]
        public string FaskesUtama { get; set; } = null!;

        [Required(ErrorMessage = "Kelas Perawatan is required")]
        [RegularExpression("^(KELAS I|KELAS II|KELAS III)$", ErrorMessage = "Kelas Perawatan must be KELAS I, KELAS II, or KELAS III")]
        public string KelasPerawatan { get; set; } = null!;

        [Required(ErrorMessage = "Status Peserta is required")]
        [RegularExpression("^(PENDING|AKTIF|NONAKTIF)$", ErrorMessage = "Status Peserta must be PENDING, AKTIF, or NONAKTIF")]
        public string StatusPeserta { get; set; } = "PENDING";

        public DateTime? TanggalDaftar { get; set; }
    }

    public class BpjsUpdateDto
    {
        [Required(ErrorMessage = "No BPJS is required")]
        [MaxLength(20, ErrorMessage = "No BPJS cannot exceed 20 characters")]
        public string NoBpjs { get; set; } = null!;

        [Required(ErrorMessage = "Faskes Utama is required")]
        [MaxLength(100, ErrorMessage = "Faskes Utama cannot exceed 100 characters")]
        public string FaskesUtama { get; set; } = null!;

        [Required(ErrorMessage = "Kelas Perawatan is required")]
        [RegularExpression("^(KELAS I|KELAS II|KELAS III)$", ErrorMessage = "Kelas Perawatan must be KELAS I, KELAS II, or KELAS III")]
        public string KelasPerawatan { get; set; } = null!;

        [Required(ErrorMessage = "Status Peserta is required")]
        [RegularExpression("^(PENDING|AKTIF|NONAKTIF)$", ErrorMessage = "Status Peserta must be PENDING, AKTIF, or NONAKTIF")]
        public string StatusPeserta { get; set; } = null!;

        public DateTime? TanggalDaftar { get; set; }
    }

    public class BpjsResponseDto
    {
        public int IdBpjs { get; set; }
        public string Nik { get; set; } = null!;
        public string NoBpjs { get; set; } = null!;
        public string FaskesUtama { get; set; } = null!;
        public string KelasPerawatan { get; set; } = null!;
        public string StatusPeserta { get; set; } = null!;
        public DateTime? TanggalDaftar { get; set; }
        public KtpKkResponseDto? KtpKk { get; set; }
    }
}
