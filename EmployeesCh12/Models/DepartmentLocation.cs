using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace EmployeesCh12.Models
{
    public class DepartmentLocation
    {
        public int DepartmentID { get; set; }
        public int LocationID { get; set; }

        // Navigation properties – do NOT validate these on form posts
        [ValidateNever]
        public Department? Department { get; set; }

        [ValidateNever]
        public Location? Location { get; set; }
    }
}
