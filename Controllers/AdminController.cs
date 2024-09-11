using Microsoft.AspNetCore.Mvc;

namespace astAttempt.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Show()
        {
            return View();
        }
    }
}
