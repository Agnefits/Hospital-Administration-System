using Hospital_Administration_System.Services;
using Hospital_Administration_System.ViewModels.Doctor;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

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
        if (User.IsInRole("Doctor"))
        {
            var user = await _unitOfWork.UserService.GetByIdAsync(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);

            var reservations = await _unitOfWork.DoctorService.GetDoctorReservationsAsync(user.Doctor.DoctorID);
            return View(reservations);
        }
        return Unauthorized();
    }

    public async Task<IActionResult> Patients()
    {

        if (User.IsInRole("Doctor"))
        {
            var user = await _unitOfWork.UserService.GetByIdAsync(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);

            var patients = await _unitOfWork.DoctorService.GetDepartmentPatientsAsync(user.Doctor.DepartmentID);
            return View(patients);
        }
        return Unauthorized();
    }

    public async Task<IActionResult> Prescriptions()
    {

        if (User.IsInRole("Doctor"))
        {
            var user = await _unitOfWork.UserService.GetByIdAsync(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);


            var patients = await _unitOfWork.DoctorService.GetDepartmentPrescriptionsAsync(user.Doctor.DepartmentID);
            return View(patients);
        }
        return Unauthorized();
    }

    public async Task<IActionResult> MedicalRecords(DateTime? startDate, DateTime? endDate, string? patientName)
    {
        ViewData["StartDate"] = startDate?.ToString("yyyy-MM-dd");
        ViewData["EndDate"] = endDate?.ToString("yyyy-MM-dd");
        ViewData["PatientName"] = patientName;

        if (User.IsInRole("Doctor"))
        {
            var user = await _unitOfWork.UserService.GetByIdAsync(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);

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
        var reservation = await _unitOfWork.ReservationService.GetByIdAsync(id);
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
