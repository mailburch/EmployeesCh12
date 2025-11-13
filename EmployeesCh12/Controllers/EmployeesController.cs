using EmployeesCh12.Data;
using EmployeesCh12.Models;
using EmployeesCh12.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;


namespace EmployeesCh12.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly EmployeeContext _context;

        public EmployeesController(EmployeeContext context)
        {
            _context = context;
        }

        // GET: Employees
        public async Task<IActionResult> Index(
            string? sortOrder,
            string? currentFilter,
            string? searchString,
            int? pageNumber)
        {
            // current sort state
            ViewData["CurrentSort"] = sortOrder;

            // set up sort toggles (Name and HireDate)
            ViewData["NameSortParm"] = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";

            // if new search text, reset to page 1
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            // base query with Department eager loaded
            var employees = from e in _context.Employees
                                .Include(e => e.Department)
                            select e;

            // FILTER: search by Name (and optionally Department name)
            if (!string.IsNullOrEmpty(searchString))
            {
                employees = employees.Where(e =>
                    e.Name.Contains(searchString) ||
                    (e.Department != null && e.Department.Name.Contains(searchString)));
            }

            // SORT: Name & HireDate
            employees = sortOrder switch
            {
                "name_desc" => employees.OrderByDescending(e => e.Name),
                "Date" => employees.OrderBy(e => e.HireDate),
                "date_desc" => employees.OrderByDescending(e => e.HireDate),
                _ => employees.OrderBy(e => e.Name), // default
            };

            // PAGINATION
            int pageSize = 3; // rows per page; adjust if you want
            return View(await PaginatedList<Employee>.CreateAsync(
                employees.AsNoTracking(),
                pageNumber ?? 1,
                pageSize));
        }


        // GET: Employees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .Include(e => e.Department)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // GET: Employees/Create
        public IActionResult Create()
        {
            ViewData["DepartmentID"] = new SelectList(_context.Departments, "Id", "Name");
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Hours,Rate,DepartmentID")] Employee employee)
        //                                             ^^^ remove Id here
        {
            // optional if you still had nullability warnings:
            ModelState.Remove(nameof(Employee.Department));
            ModelState.Remove(nameof(Employee.Benefits));

            if (!ModelState.IsValid)
            {
                ViewData["DepartmentID"] = new SelectList(_context.Departments, "Id", "Name", employee.DepartmentID);
                return View(employee);
            }

            _context.Add(employee);          // SQL Server will generate Id
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            ViewData["DepartmentID"] = new SelectList(_context.Departments, "Id", "Name", employee.DepartmentID);
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Hours,Rate,DepartmentID")] Employee employee)
        {
            if (id != employee.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.Id))
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
            ViewData["DepartmentID"] = new SelectList(_context.Departments, "Id", "Name", employee.DepartmentID);
            return View(employee);
        }

        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .Include(e => e.Department)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee != null)
            {
                _context.Employees.Remove(employee);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.Id == id);
        }

        public IActionResult DeptCount()
        {
            IQueryable<DepartmentGroup> data =
                from e in _context.Employees
                    .Include(e => e.Department)
                group e by e.DepartmentID into deptGroup
                select new DepartmentGroup
                {
                    DepartmentID = deptGroup.Key,
                    DepartmentCount = deptGroup.Count()
                };

            return View(data.ToList());
        }

    }
}
