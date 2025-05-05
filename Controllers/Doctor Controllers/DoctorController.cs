using Hospital_Administration_System.Services;
using Hospital_Administration_System.ViewModels.Doctor;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Hospital_Administration_System.Controllers.Doctor_Controllers;

//[Authorize(Roles = "Doctor")] //Note Abdallah: Uncomment for authorization
public class DoctorController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public DoctorController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IActionResult> Index()
    {
        //Note Abdallah: For Testing, please remove
        //return View();

        if (User.IsInRole("Doctor"))
        {
            var user = await _unitOfWork.UserService.GetByIdAsync(User.Claims.First(c => c.Type == "").Value);

            var reservations = _unitOfWork.DoctorService.GetDoctorReservationsAsync(user.Doctor.DoctorID);
            return View(reservations);
        }
        return Unauthorized();
    }

    public async Task<IActionResult> AppointmentsAsync()
    {
        //Note Abdallah: For Testing, please remove
        //return View(new List<Reservation> { new Reservation { } });

        //return (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
        //    ? PartialView("_AppointmentsPartial")
        //        : View();
        //    return PartialView("_AppointmentsPartial");
        //return View("Appointments");

        if (User.IsInRole("Doctor"))
        {
            var user = await _unitOfWork.UserService.GetByIdAsync(User.Claims.First(c => c.Type == "").Value);

            var appointments = _unitOfWork.DoctorService.GetDepartmentReservationsAsync(user.Doctor.DepartmentID); 
            return View(appointments);
        }
        return Unauthorized();
    }

    //[HttpPost, ActionName("DeleteAppointment")]
    //[ValidateAntiForgeryToken]
    //public async Task<ActionResult> DeleteConfirmed(int id)
    //{
    //    var ap = await  _doctorService.
    //    return RedirectToAction("AppointmentsAsync");
    //}

    public async Task<IActionResult> Patients()
    {
        //Note Abdallah: For Testing, please remove
        //return View(new List<Patient> { new Patient { } });

        if (User.IsInRole("Doctor"))
        {
            var user = await _unitOfWork.UserService.GetByIdAsync(User.Claims.First(c => c.Type == "").Value);

            var patients = _unitOfWork.DoctorService.GetDepartmentPatientsAsync(user.Doctor.DepartmentID);
            return View(patients);
        }
        return Unauthorized();
    }

    public async Task<IActionResult> Prescriptions()
    {
        //Note Abdallah: For Testing, please remove
        //return View(new List<Prescription> { new Prescription { } });

        if (User.IsInRole("Doctor"))
        {
            var user = await _unitOfWork.UserService.GetByIdAsync(User.Claims.First(c => c.Type == "").Value);

            var patients = _unitOfWork.DoctorService.GetDepartmentPrescriptionsAsync(user.Doctor.DepartmentID);
            return View(patients);
        }
        return Unauthorized();
    }

    public async Task<IActionResult> MedicalRecords(DateTime? startDate, DateTime? endDate, string? patientName)
    {
        ViewData["StartDate"] = startDate?.ToString("yyyy-MM-dd");
        ViewData["EndDate"] = endDate?.ToString("yyyy-MM-dd");
        ViewData["PatientName"] = patientName;

        //Note Abdallah: For Testing, please remove
//        return View(new List<MedicalRecord>
//{
//new MedicalRecord
//{
//    RecordID = 1,
//    PatientID = 1,
//    DoctorID = 1,
//    Diagnosis = "Acute Bronchitis",
//    Treatment = "Prescribed antibiotics and rest",
//    CreatedAt = new DateTime(2024, 12, 5, 9, 0, 0),
//    AdditionalData = "Patient advised to avoid cold drinks",
//    Patient = new Patient { FullName = "Ahmed Hassan" },
//    Doctor = new Doctor { FullName = "Dr. Mona Youssef" }
//},
//new MedicalRecord
//{
//    RecordID = 2,
//    PatientID = 1,
//    DoctorID = 1,
//    Diagnosis = "Seasonal Allergy",
//    Treatment = "Antihistamines prescribed",
//    CreatedAt = new DateTime(2025, 1, 12, 11, 30, 0),
//    AdditionalData = "Scheduled follow-up in 2 weeks",
//    Patient = new Patient { FullName = "Ahmed Hassan" },
//    Doctor = new Doctor { FullName = "Dr. Mona Youssef" }
//},
//new MedicalRecord
//{
//    RecordID = 3,
//    PatientID = 1,
//    DoctorID = 1,
//    Diagnosis = "Mild Concussion",
//    Treatment = "Complete rest and hydration",
//    CreatedAt = new DateTime(2025, 2, 3, 14, 45, 0),
//    AdditionalData = "CT scan recommended if symptoms persist",
//    Patient = new Patient { FullName = "Ahmed Hassan" },
//    Doctor = new Doctor { FullName = "Dr. Mona Youssef" }
//},
//new MedicalRecord
//{
//    RecordID = 4,
//    PatientID = 1,
//    DoctorID = 1,
//    Diagnosis = "High Blood Pressure",
//    Treatment = "Lifestyle changes and ACE inhibitors",
//    CreatedAt = new DateTime(2025, 3, 20, 16, 0, 0),
//    AdditionalData = "Regular monitoring required",
//    Patient = new Patient { FullName = "Ahmed Hassan" },
//    Doctor = new Doctor { FullName = "Dr. Mona Youssef" }
//},
//new MedicalRecord
//{
//    RecordID = 5,
//    PatientID = 1,
//    DoctorID = 1,
//    Diagnosis = "Type 2 Diabetes",
//    Treatment = "Metformin 500mg twice daily",
//    CreatedAt = new DateTime(2025, 4, 10, 10, 15, 0),
//    AdditionalData = "Diet plan shared with the patient",
//    Patient = new Patient { FullName = "Ahmed Hassan" },
//    Doctor = new Doctor { FullName = "Dr. Mona Youssef" }
//}
//});

        if (User.IsInRole("Doctor"))
        {
            var user = await _unitOfWork.UserService.GetByIdAsync(User.Claims.First(c => c.Type == "").Value);

            var records = await _unitOfWork.DoctorService.GetDepartmentMedicalsAsync(user.Doctor.DepartmentID, startDate, endDate, patientName); //Note Abdallah: Uncomment for the real function
            return View(records);
        }
        return Unauthorized();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateReservationStatus(ReservationEditStatusVM model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _unitOfWork.DoctorService.UpdateReservationStatusAsync(model);
        if (result.Succeeded)
            return RedirectToAction(nameof(Index));
        else
            return BadRequest(result.Error);
    }

    public async Task<IActionResult> RedirectReservation(int id)
    {
        var reservation = await _unitOfWork.ReservationService.GetReservationByIdAsync(id);
        if (reservation == null)
            return NotFound();

        ViewData["Doctors"] = await _unitOfWork.DoctorService.GetDoctorsAsync();

        return View(reservation);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RedirectReservation(ReservationRedirectionVM model)
    {
        if (!ModelState.IsValid)
        {
            ViewData["Doctors"] = await _unitOfWork.DoctorService.GetDoctorsAsync();
            return View(model);
        }

        var result = await _unitOfWork.DoctorService.RedirectReservationAsync(model);
        if(result.Succeeded)
            return RedirectToAction(nameof(Index));
        else
        {
            ModelState.AddModelError("", result.Error);
            ViewData["Doctors"] = await _unitOfWork.DoctorService.GetDoctorsAsync();
            return View(model);
        }
    }
}
