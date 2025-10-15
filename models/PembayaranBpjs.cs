using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace sima_bpjs_api.Models
{
    public class PembayaranBpjs
    {
        [Key]
        public int IdPembayaran { get; set; }
        public int IdBpjs { get; set; }
        public string Bulan { get; set; } = null!; // contoh: Januari 2025
        public int Tahun { get; set; }
        public decimal Jumlah { get; set; }
        public string StatusPembayaran { get; set; } = null!; // LUNAS, BELUM LUNAS
        public DateTime? TanggalBayar { get; set; }

        [ForeignKey("IdBpjs")]
        public Bpjs Bpjs { get; set; } = null!;
    }
}
