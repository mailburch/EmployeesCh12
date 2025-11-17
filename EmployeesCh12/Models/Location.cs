using System.ComponentModel.DataAnnotations;

namespace EmployeesCh12.Models
{
    public class Location
    {
        public int ID { get; set; }

        // Uses your enum Type (Headquarters, Warehouse, etc.)
        [Required]
        public Type Type { get; set; }

        [Required]
        [StringLength(60)]
        public string Address { get; set; } = string.Empty;

        [Required]
        [StringLength(10)]
        [Display(Name = "Zip Code")]
        public string Zipcode { get; set; } = string.Empty;

        // Many-to-many to Department via DepartmentLocation
        public ICollection<DepartmentLocation> DepartmentLocations { get; set; } = new List<DepartmentLocation>();
    }
}
