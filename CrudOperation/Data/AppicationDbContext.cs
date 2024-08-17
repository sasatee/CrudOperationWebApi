using CrudOperation.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace CrudOperation.Data
{
    public class AppicationDbContext:DbContext
    {
        public AppicationDbContext(DbContextOptions<AppicationDbContext> options) : base (options)
        {
            
        }

        public DbSet<Employee> Employees { get; set; }
    }
}
