using Hospital_Administration_System.Models;
using Hospital_Administration_System.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hospital_Administration_System.Controllers
{
    public class ReservationsController : Controller
    {
        private readonly ReservationService _reservationService;
        private readonly PatientService _patientService;
        private readonly DoctorService _doctorService;

        public ReservationsController(ReservationService reservationService, 
            PatientService patientService, DoctorService doctorService)
        {
            _reservationService = reservationService;
            _patientService = patientService;
            _doctorService = doctorService;
        }
        // GET: Reservations
        public async Task<ActionResult> Index()
        {
            var Res = await _reservationService.GetAllReservationsAsync();
            return View(Res);
        }

        //// GET: Reservations/Details/5
        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        // GET: Reservations/Create
        public async Task<IActionResult> Create()
        {
            ViewData["Patients"] = await _patientService.GetAllPatientsAsync();
            ViewData["Doctors"] = await _doctorService.GetAllDoctorsAsync();
            return View();
        }

        // POST: Reservations/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Reservation reservation)
        {
            try
            {
                await _reservationService.AddReservationAsync(reservation);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Reservations/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            ViewData["Patients"] = await _patientService.GetAllPatientsAsync();
            ViewData["Doctors"] = await _doctorService.GetAllDoctorsAsync();
            return View();
        }

        // POST: Reservations/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
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

        // GET: Reservations/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Reservations/Delete/5
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
