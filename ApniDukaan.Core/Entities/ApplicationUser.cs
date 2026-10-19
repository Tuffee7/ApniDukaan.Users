using System.ComponentModel.DataAnnotations;

namespace ApniDukaan.Core.Entities
{
    public class ApplicationUser
    {
        [Key]
        public Guid UserId { get; set; }

        [StringLength(100)] // NVARCHAR(100)
        public string? Email { get; set; }
        
        [StringLength(100)]
        public string? PasswordHash { get; set; }
        
        [StringLength(50)]
        public string? PersonName { get; set; }
        
        [StringLength(10)]
        public string? Gender { get; set; }

        [StringLength(20)]
        public string? Role { get; set; } = String.Empty; // Default role is "User"

        [StringLength(100)]
        public string? RefreshToken { get; set; } = String.Empty; // Refresh token for JWT authentication

        public DateTime? RefreshTokenExpiry { get; set; }
    }
}