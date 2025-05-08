

namespace Hospital_Administration_System.Controllers;

public class PrescriptionController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public PrescriptionController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    // GET: Prescription
    public async Task<IActionResult> Index()
    {
        var prescriptions = await _unitOfWork.PrescriptionService.GetAllAsync();
        return View(prescriptions);
    }

    // GET: Prescription/Details/5
    public ActionResult Details(int id)
    {
        return View();
    }

    // GET: Prescription/Create
    public async Task<IActionResult> Create()
    {
        ViewData["Patients"] = await _unitOfWork.PatientService.GetAllAsync();
        ViewData["Doctors"] = await _unitOfWork.DoctorService.GetDoctorsAsync();
        return View();
    }

    // POST: Prescription/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create(Prescription prescription)
    {

        try
        {
            await _unitOfWork.PrescriptionService.AddAsync(prescription);
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            return View();
        }
        
    }

    // GET: Prescription/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var pre = await _unitOfWork.PrescriptionService.GetByIdAsync(id);
        ViewData["Patients"] = await _unitOfWork.PatientService.GetAllAsync();
        ViewData["Doctors"] = await _unitOfWork.DoctorService.GetDoctorsAsync();
        return View(pre);
    }


    // POST: Prescription/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit(Prescription prescription)
    {

        try
        {
            await _unitOfWork.PrescriptionService.UpdateAsync(prescription);
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            return View("Edit", prescription);
        }
    }

    // GET: Prescription/Delete/5
    public ActionResult Delete(int id)
    {
        return View();
    }

    // POST: Prescription/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Delete(int id, IFormCollection collection)
    {
        try
        {
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            return View();
        }
    }
}
