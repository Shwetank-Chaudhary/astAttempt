using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using astAttempt.Data;
using astAttempt.Models.Entity;
using Microsoft.AspNetCore.Identity;

namespace astAttempt.Controllers
{
    [ApiController]
    [Route("/api/[Controller]")]
    public class CargoOrdersController : Controller
    {
        private readonly ApplicationDbContext _context;


        public CargoOrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        

        // GET: CargoOrders
        [HttpGet("showall")]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.CargoOrders.Include(c => c.Customer);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: CargoOrders/Details/5
        [HttpGet("show/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cargoOrder = await _context.CargoOrders
                .Include(c => c.Customer)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (cargoOrder == null)
            {
                return NotFound();
            }

            return View(cargoOrder);
        }

        // GET: CargoOrders/Create
        [HttpGet("create")]
        public IActionResult Create()
        {
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId");
            return View();
        }

        // POST: CargoOrders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm, Bind("OrderId,CustomerId,OrderDate,ShipDate")] CargoOrder cargoOrder)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cargoOrder);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId", cargoOrder.CustomerId);
            return View(cargoOrder);
        }

        // GET: CargoOrders/Edit/5
        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cargoOrder = await _context.CargoOrders.FindAsync(id);
            if (cargoOrder == null)
            {
                return NotFound();
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId", cargoOrder.CustomerId);
            return View(cargoOrder);
        }

        // POST: CargoOrders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [FromForm,Bind("OrderId,CustomerId,OrderDate,ShipDate")] CargoOrder cargoOrder)
        {
            if (id != cargoOrder.OrderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cargoOrder);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CargoOrderExists(cargoOrder.OrderId))
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
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId", cargoOrder.CustomerId);
            return View(cargoOrder);
        }

        // GET: CargoOrders/Delete/5
        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cargoOrder = await _context.CargoOrders
                .Include(c => c.Customer)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (cargoOrder == null)
            {
                return NotFound();
            }

            return View(cargoOrder);
        }

        // POST: CargoOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("Delete/{id}")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cargoOrder = await _context.CargoOrders.FindAsync(id);
            if (cargoOrder != null)
            {
                _context.CargoOrders.Remove(cargoOrder);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CargoOrderExists(int id)
        {
            return _context.CargoOrders.Any(e => e.OrderId == id);
        }
    }
}
