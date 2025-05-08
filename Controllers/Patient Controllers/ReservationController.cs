//using Hospital_Administration_System.Data;
//using Hospital_Administration_System.Models;
//using Hospital_Administration_System.Services;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using System.Security.Claims;

//namespace Hospital_Administration_System.Controllers.Patient_Controllers
//{
//    public class ReservationController : Controller
//    {
//        private readonly ReservationService _reservationService;

//        public ReservationController(ReservationService reservationService)
//        {
//            _reservationService = reservationService;
//        }
//        // GET: AppointmentsController
//        public async Task<IActionResult> Index()
//        {
//            var reservations = await _reservationService.GetAllReservationsAsync();
//            return View(reservations);
//        }

//        // GET: AppointmentsController/Details/5
//        public async Task<IActionResult> Details(int id)
//        {
//            var reservation = await _reservationService.GetReservationByIdAsync(id);
//            if (reservation == null)
//                return NotFound();

//            return View(reservation);
//        }

//        // GET: AppointmentsController/Create
//        public ActionResult Create()
//        {
//            return View();
//        }

//        // POST: AppointmentsController/Create
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Create(Reservation reservation)
//        {
//            if (!ModelState.IsValid)
//                return View(reservation);

//            // Prevent double booking for same doctor by same patient
//            var allReservations = await _reservationService.GetAllReservationsAsync();
//            var exists = allReservations.Any(r => r.PatientID == reservation.PatientID &&
//                                                  r.DoctorID == reservation.DoctorID &&
//                                                  r.Status != "Cancelled");

//            if (exists)
//            {
//                ModelState.AddModelError("", "You already have an appointment with this doctor.");
//                return View(reservation);
//            }

//            reservation.Status = "Pending";
//            await _reservationService.AddReservationAsync(reservation);
//            return RedirectToAction(nameof(Index));
//        }

//        // GET: AppointmentsController/Edit/5
//        public async Task<IActionResult> Edit(int id)
//        {
//            var reservation = await _reservationService.GetReservationByIdAsync(id);
//            if (reservation == null)
//                return NotFound();

//            return View(reservation);
//        }

//        // POST: AppointmentsController/Edit/5
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Edit(int id, Reservation reservation)
//        {
//            if (id != reservation.ReservationID)
//                return BadRequest();

//            if (!ModelState.IsValid)
//                return View(reservation);

//            await _reservationService.UpdateReservationAsync(reservation);
//            return RedirectToAction(nameof(Index));
//        }

//        // GET: AppointmentsController/Delete/5
//        public async Task<IActionResult> Delete(int id)
//        {
//            var reservation = await _reservationService.GetReservationByIdAsync(id);
//            if (reservation == null)
//                return NotFound();

//            return View(reservation);
//        }

//        // POST: AppointmentsController/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> DeleteConfirmed(int id)
//        {
//            var reservation = await _reservationService.GetReservationByIdAsync(id);
//            if (reservation != null)
//                await _reservationService.DeleteReservationAsync(reservation);

//            return RedirectToAction(nameof(Index));
//        }
//    }
//}




namespace Hospital_Administration_System.Controllers.Patient_Controllers;

public class ReservationController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public ReservationController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    //public async Task<IActionResult> Index()
    //{
    //    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
    //    var patient = await _patientService.GetPatientByIdAsync(userId);
    //    var reservations = await _reservationService.GetReservationsByPatientIdAsync(patient.PatientID);
    //    return View(reservations);
    //}

    public async Task<IActionResult> Details(int id)
    {
        var reservation = await _unitOfWork.ReservationService.GetByIdAsync(id);
        if (reservation == null)
            return NotFound();

        return View(reservation);
    }

    public async Task<IActionResult> Create()
    {
        var doctors = await _unitOfWork.DoctorService.GetDoctorsAsync();
        var viewModel = new ReservationViewModel
        {
            ReservationDate = DateTime.Now,
            Doctors = doctors.Select(d => new SelectListItem
            {
                Value = d.DoctorID.ToString(),
                Text = d.FullName
            }).ToList()
        };

        return View(viewModel);
    }

    //[HttpPost]
    //[ValidateAntiForgeryToken]
    //public async Task<IActionResult> Create(ReservationViewModel model)
    //{
    //    if (!ModelState.IsValid)
    //    {
    //        model.Doctors = (await _doctorService.GetAllDoctorsAsync()).Select(d => new SelectListItem
    //        {
    //            Value = d.DoctorID.ToString(),
    //            Text = d.FullName
    //        }).ToList();
    //        return View(model);
    //    }

    //    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
    //    var patient = await _patientService.GetPatientByIdAsync(userId);

    //    var allReservations = await _reservationService.GetAllReservationsAsync();
    //    var exists = allReservations.Any(r => r.PatientID == patient.PatientID &&
    //                                          r.DoctorID == model.DoctorID &&
    //                                          r.Status != "Cancelled");

    //    if (exists)
    //    {
    //        ModelState.AddModelError("", "You already have an appointment with this doctor.");
    //        model.Doctors = (await _doctorService.GetAllDoctorsAsync()).Select(d => new SelectListItem
    //        {
    //            Value = d.DoctorID.ToString(),
    //            Text = d.FullName
    //        }).ToList();
    //        return View(model);
    //    }

    //    var reservation = new Reservation
    //    {
    //        PatientID = patient.PatientID,
    //        DoctorID = model.DoctorID,
    //        ReservationDate = model.ReservationDate,
    //        AdditionalData = model.AdditionalData,
    //        Status = "Pending"
    //    };

    //    await _reservationService.AddReservationAsync(reservation);
    //    return RedirectToAction(nameof(Index));
    //}

    //public async Task<IActionResult> Edit(int id)
    //{
    //    var reservation = await _reservationService.GetReservationByIdAsync(id);
    //    if (reservation == null)
    //        return NotFound();

    //    var doctors = await _doctorService.GetAllDoctorsAsync();
    //    var viewModel = new ReservationViewModel
    //    {
    //        ReservationID = reservation.ReservationID,
    //        DoctorID = reservation.DoctorID,
    //        ReservationDate = reservation.ReservationDate,
    //        AdditionalData = reservation.AdditionalData,
    //        Doctors = doctors.Select(d => new SelectListItem
    //        {
    //            Value = d.DoctorID.ToString(),
    //            Text = d.FullName
    //        }).ToList()
    //    };

    //    return View(viewModel);
    //}

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ReservationViewModel model)
    {
        if (id != model.ReservationID || !ModelState.IsValid)
        {
            model.Doctors = (await _unitOfWork.DoctorService.GetDoctorsAsync()).Select(d => new SelectListItem
            {
                Value = d.DoctorID.ToString(),
                Text = d.FullName
            }).ToList();
            return View(model);
        }

        var reservation = await _unitOfWork.ReservationService.GetByIdAsync(id);
        if (reservation == null)
            return NotFound();

        reservation.DoctorID = model.DoctorID;
        reservation.ReservationDate = model.ReservationDate;
        reservation.AdditionalData = model.AdditionalData;

        await _unitOfWork.ReservationService.UpdateAsync(reservation);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var reservation = await _unitOfWork.ReservationService.GetByIdAsync(id);
        if (reservation == null)
            return NotFound();

        return View(reservation);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var reservation = await _unitOfWork.ReservationService.GetByIdAsync(id);
        if (reservation != null)
            await _unitOfWork.ReservationService.DeleteAsync(reservation);

        return RedirectToAction(nameof(Index));
    }
}
