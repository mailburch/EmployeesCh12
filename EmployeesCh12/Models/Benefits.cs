using System.ComponentModel.DataAnnotations;

namespace EmployeesCh12.Models
{
    public class Benefits
    {
        [Key] // PK == FK to Employee (1–1)
        public int EmployeeID { get; set; }

        public string? PlanName { get; set; }

        public Employee Employee { get; set; } = null!;
    }
}
