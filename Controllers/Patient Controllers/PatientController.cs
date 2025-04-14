using Hospital_Administration_System.Services;
using Microsoft.AspNetCore.Mvc;

namespace Hospital_Administration_System.Controllers.Patient_Controllers
{
    public class PatientController : Controller
    {
        private readonly PatientService _patientService;

        public PatientController(PatientService patientService)
        {
            _patientService = patientService;
        }
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
