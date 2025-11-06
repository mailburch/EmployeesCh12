using EmployeesCh12.Data;
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

        // ===== Details (composite key) =====
        // /DepartmentLocations/Details?DepartmentID=1&LocationID=2
        public async Task<IActionResult> Details(int? DepartmentID, int? LocationID)
        {
            if (DepartmentID == null || LocationID == null)
                return NotFound();

            var departmentLocation = await _context.DepartmentLocations
                .Include(d => d.Department)
                .Include(d => d.Location)
                .FirstOrDefaultAsync(m =>
                    m.DepartmentID == DepartmentID && m.LocationID == LocationID);

            if (departmentLocation == null)
                return NotFound();

            return View(departmentLocation);
        }

        // GET: DepartmentLocations/Create
        public IActionResult Create()
        {
            ViewData["DepartmentID"] = new SelectList(
                _context.Departments.OrderBy(d => d.Name), "Id", "Name");
            ViewData["LocationID"] = new SelectList(
                _context.Locations.OrderBy(l => l.Address), "Id", "Address");
            return View();
        }

        // POST: DepartmentLocations/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DepartmentID,LocationID")] DepartmentLocation departmentLocation)
        {
            // 10.5 — duplicate check (composite key must be unique)
            bool exists = await _context.DepartmentLocations.AnyAsync(dl =>
                dl.DepartmentID == departmentLocation.DepartmentID &&
                dl.LocationID == departmentLocation.LocationID);

            if (exists)
            {
                ModelState.AddModelError(string.Empty, "That Department/Location pair already exists.");
            }

            if (!ModelState.IsValid)
            {
                // repopulate friendly dropdowns
                ViewData["DepartmentID"] = new SelectList(
                    _context.Departments.OrderBy(d => d.Name), "Id", "Name", departmentLocation.DepartmentID);
                ViewData["LocationID"] = new SelectList(
                    _context.Locations.OrderBy(l => l.Address), "Id", "Address", departmentLocation.LocationID);
                return View(departmentLocation);
            }

            _context.Add(departmentLocation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // ===== Delete (GET) with composite key =====
        // /DepartmentLocations/Delete?DepartmentID=1&LocationID=2
        public async Task<IActionResult> Delete(int? DepartmentID, int? LocationID)
        {
            if (DepartmentID == null || LocationID == null)
                return NotFound();

            var departmentLocation = await _context.DepartmentLocations
                .Include(d => d.Department)
                .Include(d => d.Location)
                .FirstOrDefaultAsync(m =>
                    m.DepartmentID == DepartmentID && m.LocationID == LocationID);

            if (departmentLocation == null)
                return NotFound();

            return View(departmentLocation);
        }

        // ===== Delete (POST) with composite key =====
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int DepartmentID, int LocationID)
        {
            // EF Core supports FindAsync for composite keys with multiple key values
            var departmentLocation = await _context.DepartmentLocations.FindAsync(DepartmentID, LocationID);
            if (departmentLocation != null)
            {
                _context.DepartmentLocations.Remove(departmentLocation);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // Helper if you want it
        private async Task<bool> DepartmentLocationExists(int departmentId, int locationId) =>
            await _context.DepartmentLocations.AnyAsync(e =>
                e.DepartmentID == departmentId && e.LocationID == locationId);
    }
}
