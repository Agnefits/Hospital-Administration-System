using Hospital_Administration_System.ViewModels.Doctor;

namespace Hospital_Administration_System.Controllers.Doctor_Controllers;

//[Authorize(Roles = "Doctor")] //Note Abdallah: Uncomment for authorization
public class DoctorMedicalRecordController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public DoctorMedicalRecordController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IActionResult> Create()
    {
        ViewData["Patients"] = await _unitOfWork.PatientService.GetAllAsync();
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
            var user = await _unitOfWork.UserService.GetByIdAsync(User.Claims.First(c => c.Type == "").Value);

            var result = await _unitOfWork.MedicalRecordService.AddAsync(model, user.Doctor.DoctorID);
            if (result.Succeeded)
                return RedirectToAction(nameof(Index));
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
        ViewData["Patients"] = await _unitOfWork.PatientService.GetAllAsync();

        var medicalRecord = await _unitOfWork.MedicalRecordService.GetByIdAsync(id);
        if (medicalRecord == null)
            return NotFound();

        return View(medicalRecord);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(MedicalRecordEditVM model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var result = await _unitOfWork.MedicalRecordService.UpdateAsync(model);
        if (result.Succeeded)
            return RedirectToAction(nameof(Index));
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
            return RedirectToAction(nameof(Index));
        }

        return RedirectToAction(nameof(Index));
    }
}