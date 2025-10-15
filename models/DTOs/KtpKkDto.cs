using System.ComponentModel.DataAnnotations;

namespace sima_bpjs_api.Models.DTOs
{
    public class KtpKkCreateDto
    {
        [Required(ErrorMessage = "NIK is required")]
        [StringLength(16, MinimumLength = 16, ErrorMessage = "NIK must be exactly 16 characters")]
        public string Nik { get; set; } = null!;

        [Required(ErrorMessage = "No KK is required")]
        [StringLength(16, MinimumLength = 16, ErrorMessage = "No KK must be exactly 16 characters")]
        public string NoKk { get; set; } = null!;

        [Required(ErrorMessage = "Nama Lengkap is required")]
        [MaxLength(100, ErrorMessage = "Nama Lengkap cannot exceed 100 characters")]
        public string NamaLengkap { get; set; } = null!;

        [MaxLength(50, ErrorMessage = "Tempat Lahir cannot exceed 50 characters")]
        public string? TempatLahir { get; set; }

        public DateTime? TanggalLahir { get; set; }

        [RegularExpression("^[LP]$", ErrorMessage = "Jenis Kelamin must be L or P")]
        public string? JenisKelamin { get; set; }

        public string? Alamat { get; set; }

        [MaxLength(50, ErrorMessage = "Agama cannot exceed 50 characters")]
        public string? Agama { get; set; }

        [MaxLength(50, ErrorMessage = "Status Perkawinan cannot exceed 50 characters")]
        public string? StatusPerkawinan { get; set; }

        [MaxLength(100, ErrorMessage = "Pekerjaan cannot exceed 100 characters")]
        public string? Pekerjaan { get; set; }

        [MaxLength(50, ErrorMessage = "Kewarganegaraan cannot exceed 50 characters")]
        public string? Kewarganegaraan { get; set; }
    }

    public class KtpKkUpdateDto
    {
        [Required(ErrorMessage = "No KK is required")]
        [StringLength(16, MinimumLength = 16, ErrorMessage = "No KK must be exactly 16 characters")]
        public string NoKk { get; set; } = null!;

        [Required(ErrorMessage = "Nama Lengkap is required")]
        [MaxLength(100, ErrorMessage = "Nama Lengkap cannot exceed 100 characters")]
        public string NamaLengkap { get; set; } = null!;

        [MaxLength(50, ErrorMessage = "Tempat Lahir cannot exceed 50 characters")]
        public string? TempatLahir { get; set; }

        public DateTime? TanggalLahir { get; set; }

        [RegularExpression("^[LP]$", ErrorMessage = "Jenis Kelamin must be L or P")]
        public string? JenisKelamin { get; set; }

        public string? Alamat { get; set; }

        [MaxLength(50, ErrorMessage = "Agama cannot exceed 50 characters")]
        public string? Agama { get; set; }

        [MaxLength(50, ErrorMessage = "Status Perkawinan cannot exceed 50 characters")]
        public string? StatusPerkawinan { get; set; }

        [MaxLength(100, ErrorMessage = "Pekerjaan cannot exceed 100 characters")]
        public string? Pekerjaan { get; set; }

        [MaxLength(50, ErrorMessage = "Kewarganegaraan cannot exceed 50 characters")]
        public string? Kewarganegaraan { get; set; }
    }

    public class KtpKkResponseDto
    {
        public string Nik { get; set; } = null!;
        public string NoKk { get; set; } = null!;
        public string NamaLengkap { get; set; } = null!;
        public string? TempatLahir { get; set; }
        public DateTime? TanggalLahir { get; set; }
        public string? JenisKelamin { get; set; }
        public string? Alamat { get; set; }
        public string? Agama { get; set; }
        public string? StatusPerkawinan { get; set; }
        public string? Pekerjaan { get; set; }
        public string? Kewarganegaraan { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
