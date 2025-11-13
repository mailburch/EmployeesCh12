using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeesCh12.Models
{
    public class Employee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required, StringLength(30)]
        public string Name { get; set; } = string.Empty;

        [Range(10, 75)]
        public double Hours { get; set; }

        [Range(15, 75)]
        public double Rate { get; set; }

        // NEW: Hire Date
        [DataType(DataType.Date)]
        [Display(Name = "Hire Date")]
        public DateTime HireDate { get; set; } = DateTime.Today;

        // FK you actually post from the form
        [Required]
        public int DepartmentID { get; set; }

        // Navigation properties – don't validate on Create
        [ValidateNever]
        public Department? Department { get; set; }

        // 1–1 benefits: likely optional at create time
        [ValidateNever]
        public Benefits? Benefits { get; set; }
    }
}
