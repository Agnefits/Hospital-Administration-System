

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
            var nurseId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var patients = await _unitOfWork.PatientConditionMonitoringService.GetAllPatientConditionMonitoringAsync(int.Parse(nurseId!));
            return View(patients);
        }
        return Unauthorized();
    }
    public IActionResult AddPatient()
    {
        if (!User.IsInRole("Nurse"))
        {
            return RedirectToAction("Index", "Home");
        }
        var nurseId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var nurse = _unitOfWork.NurseService.GetByIdAsync(int.Parse(nurseId));
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
