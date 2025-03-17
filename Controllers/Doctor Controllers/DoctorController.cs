using Microsoft.AspNetCore.Mvc;

namespace Hospital_Administration_System.Controllers.Doctor_Controllers
{
    public class DoctorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
