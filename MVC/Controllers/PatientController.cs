using Microsoft.AspNetCore.Mvc;

namespace MVC.Controllers
{
    public class PatientController : Controller
    {
        public IActionResult Patients()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "User");
            return View();
        }
    }
}
