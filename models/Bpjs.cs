using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace sima_bpjs_api.Models
{
    public class Bpjs
    {
        [Key]
        public int IdBpjs { get; set; }
        
        [Required(ErrorMessage = "NIK is required")]
        public string Nik { get; set; } = null!;
        
        [Required(ErrorMessage = "No BPJS is required")]
        public string NoBpjs { get; set; } = null!;
        
        [Required(ErrorMessage = "Faskes Utama is required")]
        public string FaskesUtama { get; set; } = null!;
        
        [Required(ErrorMessage = "Kelas Perawatan is required")]
        public string KelasPerawatan { get; set; } = null!; // KELAS I, II, III
        
        [Required(ErrorMessage = "Status Peserta is required")]
        public string StatusPeserta { get; set; } = null!; // AKTIF, NONAKTIF
        
        public DateTime? TanggalDaftar { get; set; }

        [ForeignKey("Nik")]
        [JsonIgnore]
        public KtpKk? KtpKk { get; set; }

        public ICollection<PembayaranBpjs> Pembayaran { get; set; } = new List<PembayaranBpjs>();
    }
}
