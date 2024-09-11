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
        public string email_public;
            

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


        [HttpPost]
        [Route("show")]
        public async Task<IActionResult> Details([FromForm] string CustomerName, [FromForm] string Password)
        {

            Customer customer = _context.Customers.SingleOrDefault(c => (c.CustomerEmail == CustomerName) && (c.Password == Password));
            if (customer == null)
            {
                return NotFound();
            }
            email_public = CustomerName;
            return View(customer);
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
        [Route("Home")]
        public async Task<IActionResult> Home([FromBody] string? CustomerEmail)
        {

            Customer customer = _context.Customers.SingleOrDefault(c => (c.CustomerEmail == CustomerEmail));
            if (customer == null)
            {
                return NotFound("Customer Not Found");
            }

            return View(customer);
        }
        [HttpGet]
        [Route("Home")]
        public async Task<IActionResult> Home()
        {

            Customer customer = _context.Customers.SingleOrDefault(c => c.CustomerEmail == email_public);
            if (customer == null)
            {
                return NotFound($"{email_public}");
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
                return RedirectToAction("Home", "customers");
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
                return BadRequest();
            }

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound("Customer Not Found__UPDATE");
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
                //return RedirectToAction(nameof(Customer_Dashboard),customer.CustomerEmail);
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
            UserMaster user = new UserMaster()
            {
                UserID = cust.CustomerId.ToString(),
                UserName = cust.CustomerEmail,
                UserPassword = cust.Password,
                UserType = cust.Role
            };
            _context.Customers.Remove(cust);
            _context.UserMasters.Remove(user);
            _context.SaveChanges();
            return RedirectToAction("Home");
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
