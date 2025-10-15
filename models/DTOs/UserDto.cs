using System.ComponentModel.DataAnnotations;

namespace sima_bpjs_api.Models.DTOs
{
    public class UserCreateDto
    {
        [Required(ErrorMessage = "Username is required")]
        [MaxLength(100, ErrorMessage = "Username cannot exceed 100 characters")]
        public string Username { get; set; } = null!;

        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
        public string Password { get; set; } = null!;

        [EmailAddress(ErrorMessage = "Invalid email format")]
        [MaxLength(100, ErrorMessage = "Email cannot exceed 100 characters")]
        public string? Email { get; set; }

        [Phone(ErrorMessage = "Invalid phone number format")]
        [MaxLength(20, ErrorMessage = "Phone number cannot exceed 20 characters")]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "Role is required")]
        [RegularExpression("^(USER|ADMIN)$", ErrorMessage = "Role must be USER or ADMIN")]
        public string Role { get; set; } = "USER";

        [StringLength(16, MinimumLength = 16, ErrorMessage = "NIK must be exactly 16 characters")]
        public string? Nik { get; set; }

        public DateTime? DateOfBirth { get; set; }
    }

    public class UserUpdateDto
    {
        [Required(ErrorMessage = "Username is required")]
        [MaxLength(100, ErrorMessage = "Username cannot exceed 100 characters")]
        public string Username { get; set; } = null!;

        [EmailAddress(ErrorMessage = "Invalid email format")]
        [MaxLength(100, ErrorMessage = "Email cannot exceed 100 characters")]
        public string? Email { get; set; }

        [Phone(ErrorMessage = "Invalid phone number format")]
        [MaxLength(20, ErrorMessage = "Phone number cannot exceed 20 characters")]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "Role is required")]
        [RegularExpression("^(USER|ADMIN)$", ErrorMessage = "Role must be USER or ADMIN")]
        public string Role { get; set; } = null!;

        [StringLength(16, MinimumLength = 16, ErrorMessage = "NIK must be exactly 16 characters")]
        public string? Nik { get; set; }

        public DateTime? DateOfBirth { get; set; }
    }

    public class UserPasswordUpdateDto
    {
        [Required(ErrorMessage = "Current password is required")]
        public string CurrentPassword { get; set; } = null!;

        [Required(ErrorMessage = "New password is required")]
        [MinLength(6, ErrorMessage = "New password must be at least 6 characters")]
        public string NewPassword { get; set; } = null!;
    }

    public class UserResponseDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string Role { get; set; } = null!;
        public string? Nik { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
