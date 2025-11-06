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
            var employeeContext = _context.Benefits.Include(b => b.Employee);
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
            ViewData["EmployeeID"] = new SelectList(_context.Employees, "Id", "Name");
            return View();
        }

        // POST: Benefits/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EmployeeID,PlanName")] Benefits benefits)
        {
            if (ModelState.IsValid)
            {
                _context.Add(benefits);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EmployeeID"] = new SelectList(_context.Employees, "Id", "Name", benefits.EmployeeID);
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
            ViewData["EmployeeID"] = new SelectList(_context.Employees, "Id", "Name", benefits.EmployeeID);
            return View(benefits);
        }

        // POST: Benefits/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
                return RedirectToAction(nameof(Index));
            }
            ViewData["EmployeeID"] = new SelectList(_context.Employees, "Id", "Name", benefits.EmployeeID);
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
