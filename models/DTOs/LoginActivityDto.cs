using System;

namespace sima_bpjs_api.Models.DTOs
{
    public class LoginActivityDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public string? IpAddress { get; set; }
        public string? Device { get; set; }
        public string? Browser { get; set; }
        public string Status { get; set; } = "BERHASIL";
        public DateTime LoginTime { get; set; }
    }

    public class LoginActivityCreateDto
    {
        public int? UserId { get; set; }
        public string Username { get; set; } = null!;
        public string? IpAddress { get; set; }
        public string? Device { get; set; }
        public string? Browser { get; set; }
        public string Status { get; set; } = "BERHASIL";
    }
}

