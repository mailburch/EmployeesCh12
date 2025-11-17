namespace EmployeesCh12.Models
{
    // Bridge table for many-to-many Department ↔ Location
    public class DepartmentLocation
    {
        public int DepartmentID { get; set; }
        public Department? Department { get; set; }

        public int LocationID { get; set; }
        public Location? Location { get; set; }
    }
}
