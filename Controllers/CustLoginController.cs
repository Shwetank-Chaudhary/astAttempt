using astAttempt.Data;
using astAttempt.Models.Entity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace astAttempt.Controllers
{
    [ApiController]
    [Route("/api/[Controller]")]
    public class CustLoginController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CustLoginController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("login")]
        public IActionResult LoginCust()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoginCust(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var employee = await _context.UserMasters
                    .FirstOrDefaultAsync(e => e.UserID == model.UserName && e.UserPassword == model.Password);

                if (employee != null)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, employee.UserID),
                        new Claim(ClaimTypes.Role, "Employee")
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity));

                    return RedirectToAction("Details", "Employee", new { id = employee.UserID });
                }
                else
                {
                    ModelState.AddModelError("", "Invalid username or password");
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> UserMaster(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("LoginEmp");
        }
    }
}
