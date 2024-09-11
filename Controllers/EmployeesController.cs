using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using astAttempt.Data;
using astAttempt.Models.Entity;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.AspNetCore.Authorization;

namespace astAttempt.Controllers
{
    [Authorize(Roles = "Administrator")]
    [ApiController]
    [Route("/api/[Controller]")]
    public class EmployeesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmployeesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Employees
        [HttpGet]
        [Route("showall")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Employees.ToListAsync());
        }

        [Authorize(Roles = "Employee, Admin")]
        [HttpGet]
        [Route("show/{id?}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .FirstOrDefaultAsync(m => m.EmpId == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }
        
        // GET: Employees/Create
        [Route("create")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("create")]
        public async Task<IActionResult> Create([FromForm] Employee employee)
        {
            if (ModelState.IsValid)
            {
                _context.Add(employee);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(employee);
        }

        // GET: Employees/Update/5
        [Authorize(Roles = "Employee, Admin")]
        [HttpGet]
        [Route("update/{id?}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound("This Employee does not exists");
            }
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Employee, Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("update/{id?}")]
        public  IActionResult Edit(int id, [FromForm] Employee employee)
        {
            var emp = _context.Employees.Find(id);
            if (emp == null)
                return NotFound();
            if(employee.FirstName!=null)
                emp.FirstName = employee.FirstName;
            if (employee.LastName != null)
                emp.LastName = employee.LastName;
            if (employee.PhoneNumber != null)
                emp.PhoneNumber = employee.PhoneNumber;

            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        // GET: Employees/Delete/5
        [HttpGet]
        [Route("delete/{id?}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var emp = _context.Employees.Find(id);
            if (emp == null)
            {
                return BadRequest();
            }

            _context.Employees.Remove(emp);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        [Route("delete")]
        public IActionResult DeleteConfirmed([FromForm]int id)
        {
            var emp=  _context.Employees.Find(id);
            if (emp == null)
            {
                return BadRequest();
            }

            _context.Employees.Remove(emp);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.EmpId == id);
        }
    }
}
