using System.ComponentModel.DataAnnotations;

namespace EmployeesCh12.Models
{
    public class Department
    {
        public int Id { get; set; }

        [Required, StringLength(50)]
        public string Name { get; set; } = string.Empty;

        // one Department → many Employees
        public ICollection<Employee> Employees { get; set; } = new HashSet<Employee>();

        // link to bridge (many-to-many Department↔Location)
        public ICollection<DepartmentLocation> DepartmentLocations { get; set; } = new List<DepartmentLocation>();
    }
}
