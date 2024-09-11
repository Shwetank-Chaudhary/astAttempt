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
using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Authorization;
//using NuGet.Versioning;
//using Newtonsoft.Json.Linq;

namespace astAttempt.Controllers
{
    [ApiController]
    [Route("/[Controller]")]
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
        //[Route("show/{email}")]
        //public async Task<IActionResult> Details()
        //{
        //    string email = Request.Headers["CustomerName"].ToString();
        //    Customer customer = _context.Customers.SingleOrDefault(c => (c.CustomerEmail == email));
        //    if (customer == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(customer);
        //}


        [Authorize(Roles="Customer")]
        [HttpGet]
        [Route("show")]
        public async Task<IActionResult> Details([FromForm] string CustomerName, [FromForm] string Password)
        {

            Customer customer = _context.Customers.SingleOrDefault(c => (c.CustomerEmail == CustomerName) && (c.Password == Password));
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        //[HttpGet]
        //[Route("show")]
        //public async Task<IActionResult> Details()
        //{
        //    int? id = HttpContext.Session.GetInt32("UserId");
        //    Customer customer = _context.Customers.SingleOrDefault(c => c.CustomerId  == id);
        //    if (customer == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(customer);
        //}


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
        [Route("Home")]
        public async Task<IActionResult> Home()
        {

            Customer customer = _context.Customers.SingleOrDefault(c => (c.CustomerEmail == HttpContext.Session.GetString("UserId")));
            if (customer == null)
            {
                return NotFound(HttpContext.Session.GetString("UserId"));
            }

            return View(customer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("create")]
        public async Task<IActionResult> Create([FromForm] Customer customer)
        {
            if (ModelState.IsValid)
            {
                if (customer == null) {
                    return BadRequest();
                }
                _context.Add(customer);
                UserMaster model = new UserMaster()
                {
                    UserID = customer.CustomerId.ToString(),
                    UserName = customer.CustomerEmail,
                    UserPassword = customer.Password,
                    UserType = customer.Role
                };
                _context.UserMasters.Add(model);
                await _context.SaveChangesAsync();

                HttpContext.Session.SetString("UserId",customer.CustomerEmail);
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

        [Authorize(Roles ="Employee, Admin")]
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
                    var cust = _context.Customers.Find(id);
                    cust.CustomerName = customer.CustomerName;
                    cust.CustomerPhone = customer.CustomerPhone;
                    _context.Update(cust);
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
                return RedirectToAction(nameof(Details),customer.CustomerEmail);
            }
            ViewData["CityId"] = new SelectList(_context.Citys, "CityId", "CityId", customer.CityId);
            return RedirectToAction("Home","customers");
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
