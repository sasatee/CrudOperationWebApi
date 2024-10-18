using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthenticationAPI.Models
{
    public class Project
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]  
        public string? Description { get; set; }
        public int TotalBudgetedHours { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        // Many-to-Many Relationship: A project can have many users
        public ICollection<AppUser> Users { get; set; } =  new List<AppUser>();





    }
}
