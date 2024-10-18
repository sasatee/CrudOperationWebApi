using Microsoft.AspNetCore.Identity;

namespace AuthenticationAPI.Models
{
    public class AppUser:IdentityUser
    {
        public string? Fullname { get; set; }

        // Many-to-Many Relationship: A user can belong to many projects
        public ICollection<Project> Projects { get; set; } = new List<Project>();
    }
}
