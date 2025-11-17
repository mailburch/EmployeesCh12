using System.ComponentModel.DataAnnotations;

namespace EmployeesCh12.Models
{
    public class Benefits
    {
        public int ID { get; set; }

        [Required]
        public Category Category { get; set; }

        public bool Dental { get; set; }
        public bool Health { get; set; }
        public bool Vision { get; set; }

        // Life insurance amount in dollars
        public int LifeIns { get; set; }

        // One benefits package can apply to many employees
        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}
