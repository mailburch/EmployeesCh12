using System.ComponentModel.DataAnnotations;

namespace EmployeesCh12.Models
{
    public class Department
    {
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;

        // One department has many employees
        public ICollection<Employee> Employees { get; set; } = new List<Employee>();

        // Many-to-many to Location via DepartmentLocation
        public ICollection<DepartmentLocation> DepartmentLocations { get; set; } = new List<DepartmentLocation>();
    }
}
