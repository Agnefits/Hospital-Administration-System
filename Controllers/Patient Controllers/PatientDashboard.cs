using Microsoft.AspNetCore.Mvc;

namespace Hospital_Administration_System.Controllers.Patient_Controllers
{
    public class PatientDashboard : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
