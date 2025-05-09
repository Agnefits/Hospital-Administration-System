
using Hospital_Administration_System.ViewModels.Doctor;

namespace Hospital_Administration_System.Controllers.Admin_Controllers;

public class AdminController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public AdminController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public IActionResult Index()
    {
        return View();
    }
    public async Task<IActionResult> Logs(DateTime? startDate, DateTime? endDate, string? search)
    {
        ViewData["StartDate"] = startDate?.ToString("yyyy-MM-dd");
        ViewData["EndDate"] = endDate?.ToString("yyyy-MM-dd");
        ViewData["Search"] = search;

        var result = await _unitOfWork.LogService.GetFilteredLogsAsync(startDate, endDate, search);
        return View(result);
    }

    public async Task<IActionResult> UpcomingAppointments()
    {
        var reservations = await _unitOfWork.ReservationService.GetAllReservationsAsync();

        return View(reservations);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateReservationStatus(ReservationEditStatusVM model)
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = "Invalid reservation status.";
            return RedirectToAction(nameof(UpcomingAppointments));
        }
        var result = await _unitOfWork.DoctorService.UpdateReservationStatusAsync(model);
        if (result.Succeeded)
        {
            TempData["Success"] = "Reservation status updated successfully.";
            return RedirectToAction(nameof(UpcomingAppointments));
        }
        TempData["Error"] = result.Error;
        return RedirectToAction(nameof(UpcomingAppointments));
    }

    public async Task<IActionResult> Billings()
    {
        var reservations = await _unitOfWork.BillingService.GetAllBillingsAsync();

        return View(reservations);
    }
}
