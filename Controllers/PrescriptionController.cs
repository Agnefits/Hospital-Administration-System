using Hospital_Administration_System.Models;
using Hospital_Administration_System.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hospital_Administration_System.Controllers
{
    public class PrescriptionController : Controller
    {
        private readonly PrescriptionService _prescriptionService;
        private readonly PatientService _patientService;
        private readonly DoctorService _doctorService;

        public PrescriptionController(PrescriptionService prescriptionService, 
            PatientService patientService, DoctorService doctorService)
        {
            _prescriptionService = prescriptionService;
            _patientService = patientService;
            _doctorService = doctorService;
        }

        // GET: Prescription
        public async Task<IActionResult> Index()
        {
            var prescriptions = await _prescriptionService.GetAllPrescriptionsAsync();
            return View(prescriptions);
        }

        // GET: Prescription/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Prescription/Create
        public async Task<IActionResult> Create()
        {
            ViewData["Patients"] = await _patientService.GetAllPatientsAsync();
            ViewData["Doctors"] = await _doctorService.GetAllDoctorsAsync();
            return View();
        }

        // POST: Prescription/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Prescription prescription)
        {

            try
            {
                await _prescriptionService.AddPrescriptionAsync(prescription);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
            
        }

        // GET: Prescription/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var pre = await _prescriptionService.GetPrescriptionByIdAsync(id);
            ViewData["Patients"] = await _patientService.GetAllPatientsAsync();
            ViewData["Doctors"] = await _doctorService.GetAllDoctorsAsync();
            return View(pre);
        }


        // POST: Prescription/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Prescription prescription)
        {

            try
            {
                await _prescriptionService.UpdatePrescriptionAsync(prescription);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View("Edit", prescription);
            }
        }

        // GET: Prescription/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Prescription/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
