using Hospital_Administration_System.ViewModels.Doctor;
using System.Security.Claims;

namespace Hospital_Administration_System.Controllers.Doctor_Controllers;

//[Authorize(Roles = "Doctor")] //Note Abdallah: Uncomment for authorization
public class DoctorPrescriptionController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public DoctorPrescriptionController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    //public async Task<IActionResult> Create()
    //{
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

        ViewData["Patients"] = new SelectList(patients, "PatientID", "FullName");

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(PrescriptionCreateVM model)
    {
        if (!ModelState.IsValid)
            return View(model);

        if (User.IsInRole("Doctor"))
        {
            var user = await _unitOfWork.UserService.GetByIdAsync(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);

            var result = await _unitOfWork.PrescriptionService.AddAsync(model, user.Doctor.DoctorID);
            if (result.Succeeded)
                return RedirectToAction("Prescriptions", "Doctor");
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

    //public async Task<IActionResult> Edit(int id)
    //{
    //    ViewData["Patients"] = await _unitOfWork.PatientService.GetAllAsync();

    //    var prescription = await _unitOfWork.PrescriptionService.GetByIdAsync(id);
    //    if (prescription == null)
    //        return NotFound();

    //    return View(prescription);
    //}
    public async Task<IActionResult> Edit(int id)
    {
        // Get current doctor's patients (like in Create action)
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await _unitOfWork.UserService.GetByIdAsync(userId);
        var doctorId = user.Doctor.DoctorID;
        var patients = await _unitOfWork.PatientService.GetPatientsByDoctorId(doctorId);

        ViewData["Patients"] = new SelectList(patients, "PatientID", "FullName");

        // Fetch prescription and map to PrescriptionEditVM
        var prescription = await _unitOfWork.PrescriptionService.GetByIdAsync(id);
        if (prescription == null)
            return NotFound();

        var viewModel = new PrescriptionEditVM
        {
            PrescriptionID = prescription.PrescriptionID,
            PatientID = prescription.PatientID,
            MedicationName = prescription.MedicationName,
            Dosage = prescription.Dosage,
            Instructions = prescription.Instructions,
            AdditionalData = prescription.AdditionalData
        };

        return View(viewModel); // Pass the view model instead of the entity
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(PrescriptionEditVM model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var result = await _unitOfWork.PrescriptionService.UpdateAsync(model);
        if (result.Succeeded)
            return RedirectToAction("Prescriptions", "Doctor");
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
        bool deleted = await _unitOfWork.PrescriptionService.DeleteAsync(id);
        if (!deleted)
        {
            ModelState.AddModelError("", "Error deleting prescription record");
            return RedirectToAction("Prescriptions", "Doctor");
        }

        return RedirectToAction("Prescriptions", "Doctor");
    }
}