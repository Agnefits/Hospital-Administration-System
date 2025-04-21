

namespace Hospital_Administration_System.Controllers.Nurse_Controllers;

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

    public async Task<IActionResult> AppointmentsAsync()
    {
        var appointments = await _unitOfWork.ReservationService.GetAllReservationsAsync();
        return View(appointments);
    }

}
