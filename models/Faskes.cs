using System.ComponentModel.DataAnnotations;

namespace sima_bpjs_api.Models
{
    public class Faskes
    {
        [Key]
        public int IdFaskes { get; set; }
        [Required]
        public string Nama { get; set; } = null!;
        [Required]
        public string Tipe { get; set; } = null!; // Rumah Sakit / Klinik / Puskesmas
        public string? Alamat { get; set; }
        public string? Kota { get; set; }
        public string? Provinsi { get; set; }
        [Required]
        public string Status { get; set; } = "AKTIF"; // AKTIF / NONAKTIF
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}


