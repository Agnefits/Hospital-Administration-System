

using Hospital_Administration_System.Models;
using Hospital_Administration_System.ViewModels.Patient;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Hospital_Administration_System.Controllers.Nurse_Controllers;

[Authorize]
public class NurseController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public NurseController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public IActionResult Index()
    {
        return View();
    }

    [Authorize(Roles = "Nurse")]
    public async Task<IActionResult> AppointmentsAsync()
    {
        var appointments = await _unitOfWork.ReservationService.GetAllReservationsAsync();
        return View(appointments);
    }

    public async Task<IActionResult> index()
    {
        if (User.IsInRole("Nurse"))
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _unitOfWork.UserService.GetByIdAsync(userId);
            var patients = await _unitOfWork.PatientConditionMonitoringService.GetAllPatientConditionMonitoringAsync(user.Nurse.NurseID);
            return View(patients);
        }
        return Unauthorized();
    }
    public async Task<IActionResult> AddPatient()
    {
        if (!User.IsInRole("Nurse"))
        {
            return RedirectToAction("Index", "Home");
        }
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var user = await _unitOfWork.UserService.GetByIdAsync(userId);
        var nurse = _unitOfWork.NurseService.GetByIdAsync(user.Nurse.NurseID);
        ViewBag["patiets"] = _unitOfWork.PatientService.GetAllAsync();
        return View(nurse);
    }

    [HttpPost]
    public async Task<IActionResult> AddPatient(PatientConditionMonitoringVM patientCondition)
    {
        if (ModelState.IsValid)
        {
            await _unitOfWork.PatientConditionMonitoringService.AddPatientConditionMonitoring(patientCondition);
            return RedirectToAction("Index");
        }
        return View(patientCondition);

    }

    public async Task<IActionResult> UpdatePatientData(int Id)
    {
        var patient = await _unitOfWork.PatientConditionMonitoringService.GetByIdAsync(Id);
        return View(patient);
    }

    [HttpPost]
    public async Task<ActionResult> UpdateAsync(PatientConditionMonitoringVM patientCondition)
    {
        if (ModelState.IsValid)
        {
            await _unitOfWork.PatientConditionMonitoringService.UpdatePatientConditionMonitoring(patientCondition);
            return RedirectToAction("Index");
        }
        return View("UpdatePatientData", patientCondition);
    }
}
