using System.ComponentModel.DataAnnotations;

namespace EmployeesCh12.Models
{
    public class Location
    {
        public int Id { get; set; }

        [Required]
        public string Address { get; set; } = string.Empty;

        // link to bridge
        public ICollection<DepartmentLocation> DepartmentLocations { get; set; } = new List<DepartmentLocation>();
    }
}
