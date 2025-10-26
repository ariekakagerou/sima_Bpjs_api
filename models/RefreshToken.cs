using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace sima_bpjs_api.Models
{
    /// <summary>
    /// Refresh Token untuk JWT authentication
    /// Allows long-lived sessions without compromising security of short-lived access tokens
    /// </summary>
    [Table("refresh_tokens")]
    public class RefreshToken
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("user_id")]
        public int UserId { get; set; }

        [Required]
        [MaxLength(500)]
        [Column("token")]
        public string Token { get; set; } = null!;

        [Required]
        [Column("expires_at")]
        public DateTime ExpiresAt { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("is_revoked")]
        public bool IsRevoked { get; set; } = false;

        [Column("revoked_at")]
        public DateTime? RevokedAt { get; set; }

        [MaxLength(50)]
        [Column("created_by_ip")]
        public string? CreatedByIp { get; set; }

        [MaxLength(200)]
        [Column("created_by_device")]
        public string? CreatedByDevice { get; set; }

        // Navigation property
        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;

        // Helper property
        [NotMapped]
        public bool IsExpired => DateTime.UtcNow >= ExpiresAt;

        [NotMapped]
        public bool IsActive => !IsRevoked && !IsExpired;
    }
}

