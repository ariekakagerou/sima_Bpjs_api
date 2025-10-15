using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace sima_bpjs_api.Models
{
    public class Admin
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        [Column("username")]
        public string Username { get; set; } = null!;

        [Required]
        [Column("password")]
        public int Password { get; set; }
    }
}
