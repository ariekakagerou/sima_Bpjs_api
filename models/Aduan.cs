using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace sima_bpjs_api.Models
{
    public class Aduan
    {
        [Key]
        public int IdAduan { get; set; }

        public int? IdBpjs { get; set; }
        [ForeignKey("IdBpjs")]
        public Bpjs? Bpjs { get; set; }

        [Required]
        public string NamaPengadu { get; set; } = null!;
        public string? NoKartu { get; set; }
        public string? Kategori { get; set; } // Pelayanan, Pembayaran, Klaim, dll
        public string? Deskripsi { get; set; }
        [Required]
        public string Status { get; set; } = "BARU"; // BARU / DIPROSES / SELESAI
        public string? Prioritas { get; set; } // RENDAH / NORMAL / TINGGI

        public int? AssignedToUserId { get; set; }
        [ForeignKey("AssignedToUserId")]
        public User? AssignedTo { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public DateTime? TanggalSelesai { get; set; }
    }
}


