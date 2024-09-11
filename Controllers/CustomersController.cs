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
//using NuGet.Versioning;
//using Newtonsoft.Json.Linq;

namespace astAttempt.Controllers
{
    [ApiController]
    [Route("/api/[Controller]")]
    public class CustomersController : Controller
    {
        private readonly ApplicationDbContext _context;
            

        public CustomersController(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        [Route("showall/")]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Customers.Include(c => c.City);
            return View(await applicationDbContext.ToListAsync());
        }



        //[HttpGet]
        //[Route("showall/{id}")]
        //public async Task<IActionResult> Detail(int? id)
        //{
        //    Customer customer = _context.Customers.SingleOrDefault(c => c.CustomerId == id);
        //    if (customer == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(customer);
        //}

        [HttpGet]
        [Route("show")]
        public async Task<IActionResult> Details()
        {
            string email = HttpContext.Session.GetString("UserId");
            var customer = await _context.Customers
                .Include(c => c.City)
                .FirstOrDefaultAsync(m => m.CustomerEmail == email);
            
            if (customer == null)
            {
                return Redirect("Create");
            }

            return View(email);
        }

        [HttpGet]
        [Route("create")]
        public IActionResult Create()
        {
            var options = new List<SelectListItem>();
            foreach (int id in _context.Citys.Select(c => c.CityId)) {
                options.Add(new SelectListItem { Value = id.ToString(), Text = _context.Citys.Single(i => i.CityId == id).CityName });
            }
            ViewData["Options"] = options;
            //var message = HttpContext.Session.GetString("UserId");
            //ViewBag.WelcomeMessage = message;
            
            return View();
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("create")]
        public async Task<IActionResult> Create([FromForm] Customer customer)
        {
            if (ModelState.IsValid)
            {
                if (customer.CustomerEmail != HttpContext.Session.GetString("UserId"))
                    return BadRequest("Email Mismatched");
                _context.Add(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CityId"] = new SelectList(_context.Citys, "CityId", "CityId", customer.CityId);
            return View(customer);
        }

        [HttpGet]
        [Route("update/{id?}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            ViewData["CityId"] = new SelectList(_context.Citys, "CityId", "CityId", customer.CityId);
            return View(customer);
        }

        // POST: Customers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("update/{id?}")]
        public  IActionResult Edit(int id, [FromForm] Customer customer)
        {
            if (id != customer.CustomerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customer);
                     _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.CustomerId))
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
            ViewData["CityId"] = new SelectList(_context.Citys, "CityId", "CityId", customer.CityId);
            return View(customer);
        }

        [HttpGet]
        [Route("delete/{id?}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var cust = _context.Customers.Find(id);
            if (cust == null)
            {
                return BadRequest();
            }

            _context.Customers.Remove(cust);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        // POST: Customers/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //[Route("delete")]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var customer = await _context.Customers.FindAsync(id);
        //    if (customer != null)
        //    {
        //        _context.Customers.Remove(customer);
        //    }

        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.CustomerId == id);
        }
    }
}
