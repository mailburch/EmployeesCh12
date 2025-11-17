using EmployeesCh12.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EmployeesCh12.Controllers
{
    public class DepartmentLocationsController : Controller
    {
        private readonly EmployeeContext _context;

        public DepartmentLocationsController(EmployeeContext context)
        {
            _context = context;
        }

        // GET: DepartmentLocations
        public async Task<IActionResult> Index()
        {
            var query = _context.DepartmentLocations
                .Include(d => d.Department)
                .Include(d => d.Location);

            return View(await query.ToListAsync());
        }

        // GET: DepartmentLocations/Details?DepartmentID=1&LocationID=1
        public async Task<IActionResult> Details(int? DepartmentID, int? LocationID)
        {
            if (DepartmentID == null || LocationID == null)
            {
                return NotFound();
            }

            var departmentLocation = await _context.DepartmentLocations
                .Include(d => d.Department)
                .Include(d => d.Location)
                .FirstOrDefaultAsync(m =>
                    m.DepartmentID == DepartmentID &&
                    m.LocationID == LocationID);

            if (departmentLocation == null)
            {
                return NotFound();
            }

            return View(departmentLocation);
        }

        // GET: DepartmentLocations/Create
        public IActionResult Create()
        {
            ViewData["DepartmentID"] = new SelectList(
                _context.Departments.OrderBy(d => d.Name),
                "ID", "Name");

            ViewData["LocationID"] = new SelectList(
                _context.Locations.OrderBy(l => l.Address),
                "ID", "Address");

            return View();
        }

        // POST: DepartmentLocations/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DepartmentID,LocationID")] DepartmentLocation departmentLocation)
        {
            // Prevent duplicate DepartmentID+LocationID combo
            bool exists = await _context.DepartmentLocations
                .AnyAsync(dl =>
                    dl.DepartmentID == departmentLocation.DepartmentID &&
                    dl.LocationID == departmentLocation.LocationID);

            if (exists)
            {
                ModelState.AddModelError(string.Empty, "That Department and Location combination already exists.");
            }

            if (ModelState.IsValid)
            {
                _context.Add(departmentLocation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["DepartmentID"] = new SelectList(
                _context.Departments.OrderBy(d => d.Name),
                "ID", "Name",
                departmentLocation.DepartmentID);

            ViewData["LocationID"] = new SelectList(
                _context.Locations.OrderBy(l => l.Address),
                "ID", "Address",
                departmentLocation.LocationID);

            return View(departmentLocation);
        }

        // NOTE: Edit is usually removed for link tables; if your instructor wants it,
        // it would also need dual keys. For this assignment we DROP Edit.

        // GET: DepartmentLocations/Delete?DepartmentID=1&LocationID=1
        public async Task<IActionResult> Delete(int? DepartmentID, int? LocationID)
        {
            if (DepartmentID == null || LocationID == null)
            {
                return NotFound();
            }

            var departmentLocation = await _context.DepartmentLocations
                .Include(d => d.Department)
                .Include(d => d.Location)
                .FirstOrDefaultAsync(m =>
                    m.DepartmentID == DepartmentID &&
                    m.LocationID == LocationID);

            if (departmentLocation == null)
            {
                return NotFound();
            }

            return View(departmentLocation);
        }

        // POST: DepartmentLocations/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int DepartmentID, int LocationID)
        {
            var departmentLocation = await _context.DepartmentLocations
                .FirstOrDefaultAsync(m =>
                    m.DepartmentID == DepartmentID &&
                    m.LocationID == LocationID);

            if (departmentLocation != null)
            {
                _context.DepartmentLocations.Remove(departmentLocation);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool DepartmentLocationExists(int DepartmentID, int LocationID)
        {
            return _context.DepartmentLocations.Any(e =>
                e.DepartmentID == DepartmentID &&
                e.LocationID == LocationID);
        }
    }
}
