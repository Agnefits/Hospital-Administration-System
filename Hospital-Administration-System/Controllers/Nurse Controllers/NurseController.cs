

using Hospital_Administration_System.ViewModels.Patient;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Hospital_Administration_System.Controllers.Nurse_Controllers;

[Authorize]
public class NurseController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public NurseController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    //public IActionResult Index()
    //{
    //    return View();
    //}

    [Authorize(Roles = "Nurse")]
    public async Task<IActionResult> AppointmentsAsync()
    {
        var appointments = await _unitOfWork.ReservationService.GetAllReservationsAsync();
        return View(appointments);
    }

    public async Task<IActionResult> Index()
    {
        if (User.IsInRole("Nurse"))
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Get Nurse by UserID (which is a GUID string)
            var nurse = await _unitOfWork.NurseService.GetByUserIdAsync(userId!);

            if (nurse == null)
                return NotFound("Nurse profile not found.");

            var patients = await _unitOfWork.PatientConditionMonitoringService.GetAllPatientConditionMonitoringAsync(nurse.NurseID);
            return View(patients);
        }
        //if (User.IsInRole("Nurse"))
        //{
        //    var nurseId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //    var patients = await _unitOfWork.PatientConditionMonitoringService.GetAllPatientConditionMonitoringAsync(int.Parse(nurseId!));
        //    return View(patients);
        //}
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
        var nurse = await _unitOfWork.NurseService.GetByIdAsync(user.Nurse.NurseID);

        var viewModel = new PatientConditionMonitoringVM
        {
            NurseId = nurse.NurseID,
            NurseName = nurse.FullName
        };
        //ViewBag["patiets"] = await _unitOfWork.PatientService.GetAllAsync();
        ViewBag.Patients = await _unitOfWork.PatientService.GetAllAsync(); 
        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> AddPatient(PatientConditionMonitoringVM patientCondition)
    {
        if (ModelState.IsValid)
        {
            await _unitOfWork.PatientConditionMonitoringService.AddPatientConditionMonitoring(patientCondition);
            return RedirectToAction("Index");
        }
        ViewBag.Patients = await _unitOfWork.PatientService.GetAllAsync();
        return View(patientCondition);

    }

    //public async Task<IActionResult> UpdatePatientData(int Id)
    //{
    //    var patient = await _unitOfWork.PatientConditionMonitoringService.GetByIdAsync(Id);
    //    return View(patient);
    //}
    public async Task<IActionResult> UpdatePatientData(int Id)
    {
        // Get the entity from service
        var patientMonitoring = await _unitOfWork.PatientConditionMonitoringService.GetByIdAsync(Id);

        if (patientMonitoring == null)
            return NotFound();

        // Map to ViewModel
        var viewModel = new PatientConditionMonitoringVM
        {
            // Map properties from entity to ViewModel
            PatientId = patientMonitoring.PatientId,
            FullName = patientMonitoring.Patient.FullName,
            Age = patientMonitoring.Age,
            Gender = patientMonitoring.Patient.Gender,
            Temperature = patientMonitoring.Temperature,
            HeartRate = patientMonitoring.HeartRate,
            SystolicBP = patientMonitoring.SystolicBP,
            DiastolicBP = patientMonitoring.DiastolicBP,
            RespiratoryRate = patientMonitoring.RespiratoryRate,
            OxygenSaturation = patientMonitoring.OxygenSaturation,
            ConditionNotes = patientMonitoring.ConditionNotes,
            MonitoringTime = patientMonitoring.MonitoringTime,
            MedicationGiven = patientMonitoring.MedicationGiven,
            Dosage = patientMonitoring.Dosage,
            NurseId = patientMonitoring.NurseId,
            NurseName = patientMonitoring.Nurse.FullName
        };

        ViewBag.Patients = await _unitOfWork.PatientService.GetAllAsync();
        return View(viewModel);
    }

    [HttpPost]
    [ActionName("UpdateAsync")]
    public async Task<ActionResult> UpdateAsync(PatientConditionMonitoringVM patientCondition)
    {
        if (ModelState.IsValid)
        {
            await _unitOfWork.PatientConditionMonitoringService.UpdatePatientConditionMonitoring(patientCondition);
            return RedirectToAction("Index");
        }
        ViewBag.Patients = await _unitOfWork.PatientService.GetAllAsync();
        return View("UpdatePatientData", patientCondition);
    }
}
