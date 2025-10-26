using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace sima_bpjs_api.Models
{
    [Table("login_activities")]
    public class LoginActivity
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("user_id")]
        public int? UserId { get; set; }

        [Column("username")]
        [MaxLength(100)]
        public string Username { get; set; } = null!;

        [Column("ip_address")]
        [MaxLength(50)]
        public string? IpAddress { get; set; }

        [Column("device")]
        [MaxLength(200)]
        public string? Device { get; set; }

        [Column("browser")]
        [MaxLength(100)]
        public string? Browser { get; set; }

        [Column("status")]
        [MaxLength(20)]
        public string Status { get; set; } = "BERHASIL"; // BERHASIL, GAGAL

        [Column("login_time")]
        public DateTime LoginTime { get; set; } = DateTime.Now;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation property
        [ForeignKey("UserId")]
        public virtual User? User { get; set; }
    }
}

