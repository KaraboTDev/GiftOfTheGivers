using Microsoft.AspNetCore.Identity;

namespace GiftOfTheGivers.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string UserType { get; set; } = string.Empty; // e.g., "donor", "project_manager", "admin"
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastLoginDate{ get; set; }
        public string PreferredCurrency { get; set; } = string.Empty;
        public string? Address { get; set; }
        public DateTime? DOB { get; set; }
    }
}
