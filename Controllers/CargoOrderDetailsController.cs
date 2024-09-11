using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using astAttempt.Data;
using astAttempt.Models.Entity;
using Microsoft.AspNetCore.Authorization;

namespace astAttempt.Controllers
{
    //[Authorize("Customer")]
    [ApiController]
    [Route("/api/[Controller]")]
    public class CargoOrderDetailsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CargoOrderDetailsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CargoOrderDetails
        [HttpGet]
        [Route("showall")]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.CargoOrderDetails.Include(c => c.CargoOrder).Include(c => c.Product);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: CargoOrderDetails/Details/5

        [HttpGet]
        [Route("show/{id?}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cargoOrderDetails = await _context.CargoOrderDetails
                .Include(c => c.CargoOrder)
                .Include(c => c.Product)
                .FirstOrDefaultAsync(m => m.ShippmentId == id);
            if (cargoOrderDetails == null)
            {
                return NotFound();
            }

            return View(cargoOrderDetails);
        }

        // GET: CargoOrderDetails/Create
        [Route("create")]
        public IActionResult Create()
        {
            ViewData["CargoOrderId"] = new SelectList(_context.CargoOrders, "OrderId", "OrderId");
            ViewData["ProductId"] = new SelectList(_context.Set<Product>(), "ProductId", "ProductId");
            return View();
        }

        // POST: CargoOrderDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("create")]
        public async Task<IActionResult> Create([FromForm] CargoOrderDetails cargoOrderDetails)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cargoOrderDetails);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            ViewData["CargoOrderId"] = new SelectList(_context.CargoOrders, "OrderId", "OrderId", cargoOrderDetails.CargoOrderId);
            ViewData["ProductId"] = new SelectList(_context.Set<Product>(), "ProductId", "ProductId", cargoOrderDetails.ProductId);
            return View(cargoOrderDetails);
        }

        // GET: CargoOrderDetails/Edit/5
        [HttpPut("Edit/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cargoOrderDetails = await _context.CargoOrderDetails.FindAsync(id);
            if (cargoOrderDetails == null)
            {
                return NotFound();
            }
            ViewData["CargoOrderId"] = new SelectList(_context.CargoOrders, "OrderId", "OrderId", cargoOrderDetails.CargoOrderId);
            ViewData["ProductId"] = new SelectList(_context.Set<Product>(), "ProductId", "ProductId", cargoOrderDetails.ProductId);
            return View(cargoOrderDetails);
        }

        // POST: CargoOrderDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ShippmentId,CargoOrderId,ProductId,Quantity,UnitCost,Status")] CargoOrderDetails cargoOrderDetails)
        {
            if (id != cargoOrderDetails.ShippmentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cargoOrderDetails);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CargoOrderDetailsExists(cargoOrderDetails.ShippmentId))
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
            ViewData["CargoOrderId"] = new SelectList(_context.CargoOrders, "OrderId", "OrderId", cargoOrderDetails.CargoOrderId);
            ViewData["ProductId"] = new SelectList(_context.Set<Product>(), "ProductId", "ProductId", cargoOrderDetails.ProductId);
            return View(cargoOrderDetails);
        }

        // GET: CargoOrderDetails/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cargoOrderDetails = await _context.CargoOrderDetails
                .Include(c => c.CargoOrder)
                .Include(c => c.Product)
                .FirstOrDefaultAsync(m => m.ShippmentId == id);
            if (cargoOrderDetails == null)
            {
                return NotFound();
            }

            return View(cargoOrderDetails);
        }

        // POST: CargoOrderDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cargoOrderDetails = await _context.CargoOrderDetails.FindAsync(id);
            if (cargoOrderDetails != null)
            {
                _context.CargoOrderDetails.Remove(cargoOrderDetails);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CargoOrderDetailsExists(int id)
        {
            return _context.CargoOrderDetails.Any(e => e.ShippmentId == id);
        }
    }
}
