using Hospital_Administration_System.ViewModels.Doctor;

namespace Hospital_Administration_System.Controllers.Doctor_Controllers;

//[Authorize(Roles = "Doctor")] //Note Abdallah: Uncomment for authorization
public class DoctorPrescriptionController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public DoctorPrescriptionController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IActionResult> Create()
    {
        ViewData["Patients"] = await _unitOfWork.PatientService.GetAllPatientsAsync();
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
            var user = await _unitOfWork.UserService.GetByIdAsync(User.Claims.First(c => c.Type == "").Value);

            var result = await _unitOfWork.PrescriptionService.AddAsync(model, user.Doctor.DoctorID);
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
        ViewData["Patients"] = await _unitOfWork.PatientService.GetAllPatientsAsync();

        var prescription = await _unitOfWork.PrescriptionService.GetByIdAsync(id);
        if (prescription == null)
            return NotFound();

        return View(prescription);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(PrescriptionEditVM model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var result = await _unitOfWork.PrescriptionService.UpdateAsync(model);
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
        bool deleted = await _unitOfWork.PrescriptionService.DeleteAsync(id);
        if (!deleted)
        {
            ModelState.AddModelError("", "Error deleting prescription record");
            return RedirectToAction(nameof(Index));
        }

        return RedirectToAction(nameof(Index));
    }
}