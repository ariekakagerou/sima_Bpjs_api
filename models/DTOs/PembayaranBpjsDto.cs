using System.ComponentModel.DataAnnotations;

namespace sima_bpjs_api.Models.DTOs
{
    public class PembayaranBpjsCreateDto
    {
        [Required(ErrorMessage = "ID BPJS is required")]
        public int IdBpjs { get; set; }

        [Required(ErrorMessage = "Bulan is required")]
        [MaxLength(20, ErrorMessage = "Bulan cannot exceed 20 characters")]
        public string Bulan { get; set; } = null!;

        [Required(ErrorMessage = "Tahun is required")]
        [Range(2000, 2100, ErrorMessage = "Tahun must be between 2000 and 2100")]
        public int Tahun { get; set; }

        [Required(ErrorMessage = "Jumlah is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Jumlah must be greater than 0")]
        public decimal Jumlah { get; set; }

        [Required(ErrorMessage = "Status Pembayaran is required")]
        [RegularExpression("^(LUNAS|BELUM LUNAS)$", ErrorMessage = "Status Pembayaran must be LUNAS or BELUM LUNAS")]
        public string StatusPembayaran { get; set; } = "BELUM LUNAS";

        public DateTime? TanggalBayar { get; set; }
    }

    public class PembayaranBpjsUpdateDto
    {
        [Required(ErrorMessage = "Bulan is required")]
        [MaxLength(20, ErrorMessage = "Bulan cannot exceed 20 characters")]
        public string Bulan { get; set; } = null!;

        [Required(ErrorMessage = "Tahun is required")]
        [Range(2000, 2100, ErrorMessage = "Tahun must be between 2000 and 2100")]
        public int Tahun { get; set; }

        [Required(ErrorMessage = "Jumlah is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Jumlah must be greater than 0")]
        public decimal Jumlah { get; set; }

        [Required(ErrorMessage = "Status Pembayaran is required")]
        [RegularExpression("^(LUNAS|BELUM LUNAS)$", ErrorMessage = "Status Pembayaran must be LUNAS or BELUM LUNAS")]
        public string StatusPembayaran { get; set; } = null!;

        public DateTime? TanggalBayar { get; set; }
    }

    public class PembayaranBpjsResponseDto
    {
        public int IdPembayaran { get; set; }
        public int IdBpjs { get; set; }
        public string Bulan { get; set; } = null!;
        public int Tahun { get; set; }
        public decimal Jumlah { get; set; }
        public string StatusPembayaran { get; set; } = null!;
        public DateTime? TanggalBayar { get; set; }
        public BpjsResponseDto? Bpjs { get; set; }
    }
}
