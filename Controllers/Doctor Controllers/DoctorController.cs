using Microsoft.AspNetCore.Mvc;

namespace Hospital_Administration_System.Controllers.Doctor_Controllers
{
    public class DoctorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Appointments()
        {
            return (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                ? PartialView("_AppointmentsPartial")
                    : View();
            //    return PartialView("_AppointmentsPartial");
            //return View("Appointments");
        }

        public IActionResult Patients()
        {
            return PartialView();
        }

        public IActionResult Prescriptions()
        {
            return PartialView();
        }

        public IActionResult MedicalRecords()
        {
            return PartialView();
        }
    }
}
