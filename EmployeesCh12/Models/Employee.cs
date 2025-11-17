using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeesCh12.Models
{
    public class Employee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "Number")]
        public int ID { get; set; }

        [Display(Name = "First Name")]
        [StringLength(50, MinimumLength = 2)]
        public string FirstName { get; set; } = string.Empty;

        [Display(Name = "Last Name")]
        [StringLength(50, MinimumLength = 3)]
        public string LastName { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Hire Date")]
        public DateTime? HireDate { get; set; }

        // FK to Department (matches DepartmentID in seed)
        public int DepartmentID { get; set; }
        public Department? Department { get; set; }

        // FK to Benefits (BenefitsID in seed)
        public int BenefitsID { get; set; }
        public Benefits? Benefits { get; set; }
    }
}
