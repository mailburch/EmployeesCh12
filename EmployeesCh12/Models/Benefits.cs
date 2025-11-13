using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace EmployeesCh12.Models
{
    public class Benefits
    {
        [Key] // PK == FK to Employee (1–1)
        public int EmployeeID { get; set; }

        public string? PlanName { get; set; }

        // Navigation property – do NOT require this on form posts
        [ValidateNever]
        public Employee? Employee { get; set; }
    }
}
