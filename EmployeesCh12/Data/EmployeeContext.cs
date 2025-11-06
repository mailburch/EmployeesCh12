using EmployeesCh12.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeesCh12.Data
{
    public class EmployeeContext : DbContext
    {
        public EmployeeContext(DbContextOptions<EmployeeContext> options) : base(options) { }

        public DbSet<Employee> Employees => Set<Employee>();
        public DbSet<Department> Departments => Set<Department>();
        public DbSet<Benefits> Benefits => Set<Benefits>();
        public DbSet<Location> Locations => Set<Location>();
        public DbSet<DepartmentLocation> DepartmentLocations => Set<DepartmentLocation>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Bridge composite key
            modelBuilder.Entity<DepartmentLocation>()
                .HasKey(dl => new { dl.DepartmentID, dl.LocationID });

            // One Department → many DepartmentLocations
            modelBuilder.Entity<DepartmentLocation>()
                .HasOne(dl => dl.Department)
                .WithMany(d => d.DepartmentLocations)
                .HasForeignKey(dl => dl.DepartmentID);

            // One Location → many DepartmentLocations
            modelBuilder.Entity<DepartmentLocation>()
                .HasOne(dl => dl.Location)
                .WithMany(l => l.DepartmentLocations)
                .HasForeignKey(dl => dl.LocationID);

            // One Employee → one Benefits (PK = FK)
            modelBuilder.Entity<Benefits>()
                .HasOne(b => b.Employee)
                .WithOne(e => e.Benefits)
                .HasForeignKey<Benefits>(b => b.EmployeeID);

            // simple seed data
            modelBuilder.Entity<Department>().HasData(new Department { Id = 1, Name = "IT" });
            modelBuilder.Entity<Location>().HasData(new Location { Id = 1, Address = "HQ" });
        }
    }
}
