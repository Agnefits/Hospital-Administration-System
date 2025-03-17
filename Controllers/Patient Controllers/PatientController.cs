using Microsoft.AspNetCore.Mvc;

namespace Hospital_Administration_System.Controllers.Patient_Controllers
{
    public class PatientController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult PatientDashboard()
        {
            return View();
        }
    }
}
