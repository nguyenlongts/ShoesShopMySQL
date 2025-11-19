using Microsoft.AspNetCore.Identity;

namespace API_ShoesShop.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Gender { get; set; }

        public string? RefreshToken { get; set; }

        public DateTime? RefreshTokenExpiryTime { get; set; }

        public DateTime CreateAt { get; set; } = DateTime.UtcNow;

        public DateOnly DoB { get; set; }

        public string? Address { get; set; }

        public bool isActive {  get; set; }
    }
}
