
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
        //Note Abdallah: For Testing, please remove
        return View(new List<Reservation> { new Reservation { } });

        if (User.IsInRole("Patient"))
        {
            var user = await _unitOfWork.UserService.GetByIdAsync(User.Claims.First(c => c.Type == "").Value);

            var reservations = await _unitOfWork.PatientService.GetPatientReservations(user.Patient.PatientID);
            return Json(reservations);
        }
        return Unauthorized();
    }

    public IActionResult AddReservation()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> AddReservation(ReservationCreateVM model)
    {
        if (ModelState.IsValid)
        {
            var user = await _unitOfWork.UserService.GetByIdAsync(User.Claims.First(c => c.Type == "").Value);
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

        return View();
    }

    public async Task<IActionResult> UpdateReservation(int id)
    {
        var reservation = await _unitOfWork.ReservationService.GetByIdAsync(id);
        if (reservation == null)
        {
            return NotFound();
        }

        return View(reservation);
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
        return View(reservation.ReservationID);
    }

    public async Task<IActionResult> Prescriptions()
    {
        //Note Abdallah: For Testing, please remove
        return View(new List<Prescription> { new Prescription { } });
        if (User.IsInRole("Patient"))
        {
            var user = await _unitOfWork.UserService.GetByIdAsync(User.Claims.First(c => c.Type == "").Value);
            var prescriptions = await _unitOfWork.PatientService.GetPatientPrescriptions(user.Patient.PatientID);
            return Json(prescriptions);
        }
        return Unauthorized();
    }

    public async Task<IActionResult> Medicals()
    {
        //Note Abdallah: For Testing, please remove
        return View(new List<MedicalRecord> { new MedicalRecord { } });
        if (User.IsInRole("Patient"))
        {
            var user = await _unitOfWork.UserService.GetByIdAsync(User.Claims.First(c => c.Type == "").Value);
            var medicals = await _unitOfWork.PatientService.GetPatientMedicals(user.Patient.PatientID);
            return Json(medicals);
        }
        return Unauthorized();
    }

    public async Task<IActionResult> Analysis()
    {
        //Note Abdallah: For Testing, please remove
        return View(new List<Analysis> { new Analysis { } });
        if (User.IsInRole("Patient"))
        {
            var user = await _unitOfWork.UserService.GetByIdAsync(User.Claims.First(c => c.Type == "").Value);
            var analysis = await _unitOfWork.PatientService.GetPatientAnalysis(user.Patient.PatientID);
            return Json(analysis);
        }
        return Unauthorized();
    }

    public async Task<IActionResult> Bills()
    {
        //Note Abdallah: For Testing, please remove
        return View(new List<Billing> { new Billing { } });
        if (User.IsInRole("Patient"))
        {
            var user = await _unitOfWork.UserService.GetByIdAsync(User.Claims.First(c => c.Type == "").Value);
            var bills = await _unitOfWork.PatientService.GetPatientBills(user.Patient.PatientID);
            return Json(bills);
        }
        return Unauthorized();
    }

    public async Task<IActionResult> Receipts()
    {
        //Note Abdallah: For Testing, please remove
        return View(new List<Receipt> { new Receipt { } });
        if (User.IsInRole("Patient"))
        {
            var user = await _unitOfWork.UserService.GetByIdAsync(User.Claims.First(c => c.Type == "").Value);
            var receipts = await _unitOfWork.PatientService.GetPatientReceipts(user.Patient.PatientID);
            return Json(receipts);
        }
        return Unauthorized();
    }
}
