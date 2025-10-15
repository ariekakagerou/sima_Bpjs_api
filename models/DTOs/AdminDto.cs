using System.ComponentModel.DataAnnotations;

namespace sima_bpjs_api.Models.DTOs
{
    public class AdminCreateDto
    {
        [Required(ErrorMessage = "Username is required")]
        [MaxLength(255, ErrorMessage = "Username cannot exceed 255 characters")]
        public string Username { get; set; } = null!;

        [Required(ErrorMessage = "Password is required")]
        public int Password { get; set; }
    }

    public class AdminUpdateDto
    {
        [Required(ErrorMessage = "Username is required")]
        [MaxLength(255, ErrorMessage = "Username cannot exceed 255 characters")]
        public string Username { get; set; } = null!;

        [Required(ErrorMessage = "Password is required")]
        public int Password { get; set; }
    }

    public class AdminResponseDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;
    }
}
