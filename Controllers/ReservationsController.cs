
namespace Hospital_Administration_System.Controllers;

public class ReservationsController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public ReservationsController(IUnitOfWork unitOfWork)
    {

        _unitOfWork = unitOfWork;
    }
    // GET: Reservations
    public async Task<ActionResult> Index()
    {
        var Res = await _unitOfWork.ReservationService.GetAllReservationsAsync();
        return View(Res);
    }

    //// GET: Reservations/Details/5
    //public ActionResult Details(int id)
    //{
    //    return View();
    //}

    // GET: Reservations/Create
    public async Task<IActionResult> Create()
    {
        ViewData["Patients"] = await _unitOfWork.PatientService.GetAllPatientsAsync();
        ViewData["Doctors"] = await _unitOfWork.DoctorService.GetDoctorsAsync();
        return View();
    }

    // POST: Reservations/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create(Reservation reservation)
    {
        try
        {
            await _unitOfWork.ReservationService.AddReservationAsync(reservation);
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            return View();
        }
    }

    // GET: Reservations/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        ViewData["Patients"] = await _unitOfWork.PatientService.GetAllPatientsAsync();
        ViewData["Doctors"] = await _unitOfWork.DoctorService.GetDoctorsAsync();
        return View();
    }

    // POST: Reservations/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit(int id, IFormCollection collection)
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

    // GET: Reservations/Delete/5
    public ActionResult Delete(int id)
    {
        return View();
    }

    // POST: Reservations/Delete/5
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
