using Hospital_Administration_System.Models;
using Microsoft.AspNetCore.Mvc;

namespace Hospital_Administration_System.Controllers
{
    public class ResultsController : Controller
    {
        private static List<Analysis> Analyses = new List<Analysis>();
        private static List<MedicalRecord> MedicalRecords = new List<MedicalRecord>();
        private static List<Prescription> Prescriptions = new List<Prescription>();

        public IActionResult Index()
        {
            var model = new
            {
                Analyses = Analyses,
                MedicalRecords = MedicalRecords,
                Prescriptions = Prescriptions
            };
            return View(model);
        }
    }
}
