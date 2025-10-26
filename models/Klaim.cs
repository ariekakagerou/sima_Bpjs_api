using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace sima_bpjs_api.Models
{
    public class Klaim
    {
        [Key]
        public int IdKlaim { get; set; }

        [Required]
        public int IdBpjs { get; set; }
        [ForeignKey("IdBpjs")]
        public Bpjs? Bpjs { get; set; }

        [Required]
        public int IdFaskes { get; set; }
        [ForeignKey("IdFaskes")]
        public Faskes? Faskes { get; set; }

        [Required]
        public DateTime TanggalPengajuan { get; set; } = DateTime.UtcNow;
        public string? Diagnosis { get; set; }
        public string? Tindakan { get; set; }
        public decimal BiayaDiajukan { get; set; }
        public decimal? BiayaDisetujui { get; set; }
        [Required]
        public string Status { get; set; } = "PENDING"; // PENDING / DISETUJUI / DITOLAK
        public string? Catatan { get; set; }

        public int? CreatedByUserId { get; set; }
        [ForeignKey("CreatedByUserId")]
        public User? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}


