using EmployeesCh12.Data;
using EmployeesCh12.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EmployeesCh12.Controllers
{
    public class BenefitsController : Controller
    {
        private readonly EmployeeContext _context;

        public BenefitsController(EmployeeContext context)
        {
            _context = context;
        }

        // GET: Benefits
        public async Task<IActionResult> Index()
        {
            var employeeContext = _context.Benefits
                .Include(b => b.Employee);
            return View(await employeeContext.ToListAsync());
        }

        // GET: Benefits/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var benefits = await _context.Benefits
                .Include(b => b.Employee)
                .FirstOrDefaultAsync(m => m.EmployeeID == id);
            if (benefits == null)
            {
                return NotFound();
            }

            return View(benefits);
        }

        // GET: Benefits/Create
        public IActionResult Create()
        {
            // Only show employees who DON'T already have a benefits row
            var usedEmployeeIds = _context.Benefits
                .Select(b => b.EmployeeID)
                .ToList();

            var availableEmployees = _context.Employees
                .Where(e => !usedEmployeeIds.Contains(e.Id))
                .OrderBy(e => e.Name)
                .ToList();

            if (!availableEmployees.Any())
            {
                // Optional: tell the user why nothing is in the dropdown
                ModelState.AddModelError(string.Empty, "All employees already have benefits. Create a new employee first.");
            }

            ViewData["EmployeeID"] = new SelectList(availableEmployees, "Id", "Name");
            return View();
        }

        // POST: Benefits/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EmployeeID,PlanName")] Benefits benefits)
        {
            // Enforce 1-to-1: don't allow duplicate benefits for same employee
            if (await _context.Benefits.AnyAsync(b => b.EmployeeID == benefits.EmployeeID))
            {
                ModelState.AddModelError("EmployeeID", "This employee already has a benefits plan.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(benefits);
                    await _context.SaveChangesAsync();
                    // DEBUG: prove we hit the success path
                    TempData["BenefitsDebug"] = "Save was successful.";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {
                    // Show the actual DB error text in dev so we stop guessing
                    ModelState.AddModelError(string.Empty,
                        $"DB error: {ex.GetBaseException().Message}");
                }
            }

            // At this point, ModelState is NOT valid. Let's dump it.
            var allErrors = string.Join(" | ",
                ModelState
                    .Where(kvp => kvp.Value.Errors.Count > 0)
                    .SelectMany(kvp =>
                        kvp.Value.Errors.Select(e => $"{kvp.Key}: {e.ErrorMessage}")));

            ViewBag.Debug = $"ModelState invalid. Errors: {allErrors}";

            // Rebuild dropdown
            var usedEmployeeIds = _context.Benefits
                .Select(b => b.EmployeeID)
                .ToList();

            var availableEmployees = _context.Employees
                .Where(e => !usedEmployeeIds.Contains(e.Id) || e.Id == benefits.EmployeeID)
                .OrderBy(e => e.Name)
                .ToList();

            ViewData["EmployeeID"] = new SelectList(availableEmployees, "Id", "Name", benefits.EmployeeID);
            return View(benefits);
        }


        // GET: Benefits/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var benefits = await _context.Benefits.FindAsync(id);
            if (benefits == null)
            {
                return NotFound();
            }

            // For simplicity, let user see all employees; primary key still must match in POST
            ViewData["EmployeeID"] = new SelectList(_context.Employees.OrderBy(e => e.Name), "Id", "Name", benefits.EmployeeID);
            return View(benefits);
        }

        // POST: Benefits/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EmployeeID,PlanName")] Benefits benefits)
        {
            if (id != benefits.EmployeeID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(benefits);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BenefitsExists(benefits.EmployeeID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError(string.Empty,
                        "Unable to save changes. Try again, and if the problem persists, contact your system administrator.");
                    ViewData["EmployeeID"] = new SelectList(_context.Employees.OrderBy(e => e.Name), "Id", "Name", benefits.EmployeeID);
                    return View(benefits);
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["EmployeeID"] = new SelectList(_context.Employees.OrderBy(e => e.Name), "Id", "Name", benefits.EmployeeID);
            return View(benefits);
        }

        // GET: Benefits/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var benefits = await _context.Benefits
                .Include(b => b.Employee)
                .FirstOrDefaultAsync(m => m.EmployeeID == id);
            if (benefits == null)
            {
                return NotFound();
            }

            return View(benefits);
        }

        // POST: Benefits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var benefits = await _context.Benefits.FindAsync(id);
            if (benefits != null)
            {
                _context.Benefits.Remove(benefits);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BenefitsExists(int id)
        {
            return _context.Benefits.Any(e => e.EmployeeID == id);
        }
    }
}
