using System.Linq;

namespace sima_bpjs_api.Validators
{
    /// <summary>
    /// Password strength validator untuk mencegah penggunaan password lemah
    /// Implements OWASP password requirements
    /// </summary>
    public static class PasswordValidator
    {
        /// <summary>
        /// Validasi kekuatan password
        /// Minimal requirements:
        /// - 8 karakter
        /// - 1 huruf besar
        /// - 1 huruf kecil
        /// - 1 angka
        /// - 1 karakter spesial
        /// - Tidak mengandung common passwords
        /// </summary>
        public static (bool IsValid, string Message) ValidateStrength(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return (false, "Password tidak boleh kosong");

            if (password.Length < 8)
                return (false, "Password minimal 8 karakter");

            if (password.Length > 128)
                return (false, "Password maksimal 128 karakter");

            if (!password.Any(char.IsUpper))
                return (false, "Password harus mengandung minimal 1 huruf besar (A-Z)");

            if (!password.Any(char.IsLower))
                return (false, "Password harus mengandung minimal 1 huruf kecil (a-z)");

            if (!password.Any(char.IsDigit))
                return (false, "Password harus mengandung minimal 1 angka (0-9)");

            var specialChars = "!@#$%^&*()_+-=[]{}|;:,.<>?/~`";
            if (!password.Any(c => specialChars.Contains(c)))
                return (false, "Password harus mengandung minimal 1 karakter spesial (!@#$%^&* dll)");

            // Check against common weak passwords
            var commonPasswords = new[] 
            { 
                "password", "password123", "12345678", "qwerty", "abc123",
                "admin", "admin123", "user123", "letmein", "welcome",
                "monkey", "dragon", "master", "sunshine", "princess",
                "qwerty123", "password1", "admin1234", "test123",
                "Password1", "Password123", "Admin123", "Admin1234",
                "Password1!", "Admin123!", "Welcome1!", "Test123!"
            };
            
            if (commonPasswords.Any(cp => password.Equals(cp, StringComparison.OrdinalIgnoreCase)))
                return (false, "Password terlalu umum. Gunakan kombinasi yang lebih unik dan kuat");

            // Check for sequential characters (123, abc, etc)
            if (HasSequentialChars(password))
                return (false, "Password tidak boleh mengandung karakter berurutan (123, abc, dll)");

            return (true, "Password valid");
        }

        /// <summary>
        /// Check if password contains sequential characters
        /// </summary>
        private static bool HasSequentialChars(string password)
        {
            var sequences = new[] 
            { 
                "0123456789", "abcdefghijklmnopqrstuvwxyz", 
                "qwertyuiop", "asdfghjkl", "zxcvbnm"
            };

            foreach (var seq in sequences)
            {
                for (int i = 0; i <= seq.Length - 3; i++)
                {
                    var substring = seq.Substring(i, 3);
                    if (password.ToLower().Contains(substring))
                        return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Generate password strength score (0-100)
        /// </summary>
        public static int GetPasswordScore(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return 0;

            int score = 0;

            // Length score (max 30 points)
            score += Math.Min(password.Length * 3, 30);

            // Uppercase letters
            if (password.Any(char.IsUpper))
                score += 10;

            // Lowercase letters
            if (password.Any(char.IsLower))
                score += 10;

            // Numbers
            if (password.Any(char.IsDigit))
                score += 10;

            // Special characters
            var specialChars = "!@#$%^&*()_+-=[]{}|;:,.<>?/~`";
            if (password.Any(c => specialChars.Contains(c)))
                score += 20;

            // Length bonus
            if (password.Length >= 12)
                score += 10;
            if (password.Length >= 16)
                score += 10;

            return Math.Min(score, 100);
        }

        /// <summary>
        /// Get password strength label
        /// </summary>
        public static string GetPasswordStrengthLabel(string password)
        {
            var score = GetPasswordScore(password);

            if (score < 40)
                return "Lemah";
            if (score < 60)
                return "Sedang";
            if (score < 80)
                return "Kuat";
            
            return "Sangat Kuat";
        }
    }
}

