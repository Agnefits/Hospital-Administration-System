
using Hospital_Administration_System.Models;
using Hospital_Administration_System.ViewModels.Reservation;
using System.Security.Claims;

namespace Hospital_Administration_System.Controllers.Patient_Controllers;

//[Authorize(Roles = "Patient")] //Note Abdallah: Uncomment for authorization
public class PatientController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public PatientController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public IActionResult Index()
    {
        return View();
    }
    public IActionResult PatientDashboard()
    {
        return View();
    }

    public async Task<IActionResult> Reservations()
    { 
        if (User.IsInRole("Patient"))
        {
            var user = await _unitOfWork.UserService.GetByIdAsync(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);

            var reservations = await _unitOfWork.PatientService.GetPatientReservations(user.Patient.PatientID);
            return View(reservations);
        }
        return Unauthorized();
    }

    public async Task<IActionResult> AddReservation()
    {
        ViewData["Doctors"] = await _unitOfWork.DoctorService.GetDoctorsAsync();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> AddReservation(ReservationCreateVM model)
    {
        if (ModelState.IsValid)
        {
            var user = await _unitOfWork.UserService.GetByIdAsync(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
            model.PatientID = user.Patient.PatientID;

            var response = await _unitOfWork.ReservationService.AddAsync(model);
            if(response.Succeeded)
            {
                TempData["Success"] = "Reservation added successfully.";
                return RedirectToAction("Reservations");
            }
            else
            {
                TempData["Error"] = response.Error;
            }
        }
        ViewData["Doctors"] = await _unitOfWork.DoctorService.GetDoctorsAsync();
        return View();
    }

    public async Task<IActionResult> UpdateReservation(int id)
    {
        var reservation = await _unitOfWork.ReservationService.GetByIdAsync(id);
        if (reservation == null)
        {
            return NotFound();
        }
        var viewModel = new ReservationEditVM
        {
            ReservationID = reservation.ReservationID,
            PatientID = reservation.PatientID,
            DoctorID = reservation.DoctorID,
            ReservationDate = reservation.ReservationDate,
            AdditionalData = reservation.AdditionalData
        };

        ViewData["Doctors"] = await _unitOfWork.DoctorService.GetDoctorsAsync();
        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateReservation(ReservationEditVM reservation)
    {
        if (ModelState.IsValid)
        {
            var response = await _unitOfWork.ReservationService.UpdateAsync(reservation);
            if (response.Succeeded)
            {
                TempData["Success"] = "Reservation updated successfully.";
                return RedirectToAction("Reservations");
            }
            else
            {
                TempData["Error"] = response.Error;
            }
        }
        ViewData["Doctors"] = await _unitOfWork.DoctorService.GetDoctorsAsync();
        return View(reservation);
    }

    public async Task<IActionResult> Prescriptions()
    {
        if (User.IsInRole("Patient"))
        {
            var user = await _unitOfWork.UserService.GetByIdAsync(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var prescriptions = await _unitOfWork.PatientService.GetPatientPrescriptions(user.Patient.PatientID);
            return View(prescriptions);
        }
        return Unauthorized();
    }

    public async Task<IActionResult> Medicals()
    {
        if (User.IsInRole("Patient"))
        {
            var user = await _unitOfWork.UserService.GetByIdAsync(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var medicals = await _unitOfWork.PatientService.GetPatientMedicals(user.Patient.PatientID);
            return View(medicals);
        }
        return Unauthorized();
    }

    public async Task<IActionResult> Analysis()
    {
        if (User.IsInRole("Patient"))
        {
            var user = await _unitOfWork.UserService.GetByIdAsync(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var analysis = await _unitOfWork.PatientService.GetPatientAnalysis(user.Patient.PatientID);
            return View(analysis);
        }
        return Unauthorized();
    }

    public async Task<IActionResult> Bills()
    {
        if (User.IsInRole("Patient"))
        {
            var user = await _unitOfWork.UserService.GetByIdAsync(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var bills = await _unitOfWork.PatientService.GetPatientBills(user.Patient.PatientID);
            return View(bills);
        }
        return Unauthorized();
    }

    public async Task<IActionResult> Receipts()
    {
        if (User.IsInRole("Patient"))
        {
            var user = await _unitOfWork.UserService.GetByIdAsync(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var receipts = await _unitOfWork.PatientService.GetPatientReceipts(user.Patient.PatientID);
            return View(receipts);
        }
        return Unauthorized();
    }
}
