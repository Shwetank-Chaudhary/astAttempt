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
        public async Task<IActionResult> Login([FromForm]string EmpEmail, [FromForm]string Password)
        {
            LoginViewModel model = new LoginViewModel() { UserName = EmpEmail, Password = Password };
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

                    return RedirectToAction("show", "Admin");
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
