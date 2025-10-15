using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace sima_bpjs_api.Models
{
    public class KtpKk
    {
        [Key]
        public string Nik { get; set; } = null!;
        public string NoKk { get; set; } = null!;
        public string NamaLengkap { get; set; } = null!;
        public string TempatLahir { get; set; } = null!;
        public DateTime? TanggalLahir { get; set; }
        public string JenisKelamin { get; set; } = null!; // L / P
        public string Alamat { get; set; } = null!;
        public string Agama { get; set; } = null!;
        public string StatusPerkawinan { get; set; } = null!;
        public string Pekerjaan { get; set; } = null!;
        public string Kewarganegaraan { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public ICollection<Bpjs> Bpjs { get; set; } = new List<Bpjs>();
    }
}
