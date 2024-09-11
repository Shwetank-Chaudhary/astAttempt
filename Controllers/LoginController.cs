using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using astAttempt.Models.Entity;
using astAttempt.Data;
using System.Threading.Tasks;
using System.Collections.Generic;


namespace astAttempt.Controllers
{
    [ApiController]
    [Route("/api/[Controller]")]
    public class AccountController : Controller
    {
        [HttpGet]
        [Route("login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult>Login(LoginViewModel model)
        {
            //// Validate credentials and set authentication cookie
            //if (CustomerName == "admin" && password == "password") // Example validation
            //{
            //    var claims = new[]
            //    {
            //    new Claim(ClaimTypes.Name, CustomerName),
            //    new Claim(ClaimTypes.Role, "Admin")
            //};

            //    var identity = new ClaimsIdentity(claims, "CustomAuthScheme");
            //    var principal = new ClaimsPrincipal(identity);

            //    HttpContext.SignInAsync("CustomAuthScheme", principal);

            //    return RedirectToAction("Index", "Home");
            //}
            //ViewBag.Username = CustomerName;
            //ViewBag.Password = password;
            //ModelState.AddModelError("", "Invalid login attempt.");
            //return View();

            if (ModelState.IsValid)
            {
                if (model.Password == "password" && model.UserName == "admin")
                {
                    var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, model.UserName),
                new Claim(ClaimTypes.Role, "Admin")
            };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var authProperties = new AuthenticationProperties
                    {
                        // Optionally configure properties such as IsPersistent
                    };

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity), authProperties);

                    return RedirectToAction("Dashboard", "AdminDashboard");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid username or password");
                }
            }
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("LoginEmp");
        }
    }

    //[HttpPost]
    //public async Task<IActionResult>Logout()
    //{
    //    await HttpContext.SignOutAsync("CustomAuthScheme");
    //    return RedirectToAction("Index", "Home");
    //}
    
    
}
