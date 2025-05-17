using Hospital_Administration_System.ViewModels.Doctor;
using System.Security.Claims;

namespace Hospital_Administration_System.Controllers.Doctor_Controllers;

//[Authorize(Roles = "Doctor")] //Note Abdallah: Uncomment for authorization
public class DoctorMedicalRecordController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public DoctorMedicalRecordController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    //public async Task<IActionResult> Create()
    //{
    //    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
    //    var user = await _unitOfWork.UserService.GetByIdAsync(userId);
    //    if (user == null || user.Doctor == null)
    //        return Unauthorized();
    //    var doctorId = user.Doctor.DoctorID;
    //    var patients = await _unitOfWork.PatientService.GetPatientsByDoctorId(doctorId);
    //    ViewData["Patients"] = await _unitOfWork.PatientService.GetAllAsync();
    //    return View();
    //}
    public async Task<IActionResult> Create()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await _unitOfWork.UserService.GetByIdAsync(userId);
        if (user == null || user.Doctor == null)
            return Unauthorized();

        var doctorId = user.Doctor.DoctorID;
        var patients = await _unitOfWork.PatientService.GetPatientsByDoctorId(doctorId);

        ViewData["Patients"] = new SelectList(patients, "PatientID", "FullName"); // Filtered patients
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(MedicalRecordCreateVM model)
    {
        if (!ModelState.IsValid)
            return View(model);

        if (User.IsInRole("Doctor"))
        {
            var user = await _unitOfWork.UserService.GetByIdAsync(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);

            var result = await _unitOfWork.MedicalRecordService.AddAsync(model, user.Doctor.DoctorID);
            if (result.Succeeded)
                return RedirectToAction("MedicalRecords", "Doctor");
            else
            {
                ModelState.AddModelError("", result.Error);
                return View(model);
            }
        }
        else
        {
            return Unauthorized();
        }
    }

    public async Task<IActionResult> Edit(int id)
    {
        // Get current doctor's patients
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await _unitOfWork.UserService.GetByIdAsync(userId);
        var doctorId = user.Doctor.DoctorID;
        var patients = await _unitOfWork.PatientService.GetPatientsByDoctorId(doctorId);

        ViewData["Patients"] = new SelectList(patients, "PatientID", "FullName");

        var medicalRecord = await _unitOfWork.MedicalRecordService.GetByIdAsync(id);
        if (medicalRecord == null)
            return NotFound();

        // Map to ViewModel
        var viewModel = new MedicalRecordEditVM
        {
            RecordID = medicalRecord.RecordID,
            PatientID = medicalRecord.PatientID,
            Diagnosis = medicalRecord.Diagnosis,
            Treatment = medicalRecord.Treatment,
            AdditionalData = medicalRecord.AdditionalData
        };

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(MedicalRecordEditVM model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var result = await _unitOfWork.MedicalRecordService.UpdateAsync(model);
        if (result.Succeeded)
            return RedirectToAction("MedicalRecords", "Doctor");
        else
        {
            ModelState.AddModelError("", result.Error);
            return View(model);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        bool deleted = await _unitOfWork.MedicalRecordService.DeleteAsync(id);
        if (!deleted)
        {
            ModelState.AddModelError("", "Error deleting medical record");
            return RedirectToAction("MedicalRecords", "Doctor");
        }

        return RedirectToAction("MedicalRecords", "Doctor");
    }
}