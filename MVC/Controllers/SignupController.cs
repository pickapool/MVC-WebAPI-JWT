using Microsoft.AspNetCore.Mvc;

namespace MVC.Controllers
{
    public class SignupController : Controller
    {
        public IActionResult Signup()
        {
            return View();
        }
    }
}
