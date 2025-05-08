
using Hospital_Administration_System.ViewModels.Doctor;

namespace Hospital_Administration_System.Controllers.Admin_Controllers;

public class AdminController : Controller
{
    private readonly UnitOfWork _unitOfWork;
    private readonly UserService _userService;
    private readonly LogService _logService;
    private readonly ApplicationDbContext _context;

    public AdminController(UnitOfWork unitOfWork, UserService userService, LogService logService, ApplicationDbContext context)
    {
        _unitOfWork = unitOfWork;
        _userService = userService;
        _logService = logService;
        _context = context;
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

        var result = await _logService.GetFilteredLogsAsync(startDate, endDate, search);
        return View(result);
    }

    public async Task<IActionResult> UpcomingAppointments()
    {
        //var startDate = DateTime.Today;
        //var endDate = DateTime.Today.AddDays(14).AddDays(1);

        //var allAppointments = new List<Reservation>
        //{
        //    new Reservation
        //    {
        //        ReservationID = 1,
        //        PatientID = 1,
        //        DoctorID = 1,
        //        ReservationDate = new DateTime(2025, 4, 18, 10, 0, 0),
        //        Status = "Pending",
        //        AdditionalData = "Follow-up appointment",
        //        Patient = new Patient { FullName = "Ahmed Hassan" },
        //        Doctor = new Doctor { FullName = "Dr. Mona Youssef" }
        //    },
        //    new Reservation
        //    {
        //        ReservationID = 2,
        //        PatientID = 2,
        //        DoctorID = 1,
        //        ReservationDate = new DateTime(2025, 4, 19, 14, 30, 0),
        //        Status = "Confirmed",
        //        AdditionalData = "First-time visit",
        //        Patient = new Patient { FullName = "Sara Khaled" },
        //        Doctor = new Doctor { FullName = "Dr. Mona Youssef" }
        //    },
        //    new Reservation
        //    {
        //        ReservationID = 3,
        //        PatientID = 3,
        //        DoctorID = 2,
        //        ReservationDate = new DateTime(2025, 4, 20, 9, 15, 0),
        //        Status = "Cancelled",
        //        AdditionalData = "Patient called to cancel",
        //        Patient = new Patient { FullName = "Omar Tarek" },
        //        Doctor = new Doctor { FullName = "Dr. Hossam Ali" }
        //    },
        //    new Reservation
        //    {
        //        ReservationID = 4,
        //        PatientID = 1,
        //        DoctorID = 1,
        //        ReservationDate = new DateTime(2025, 4, 22, 11, 0, 0),
        //        Status = "Confirmed",
        //        AdditionalData = "Routine check-up",
        //        Patient = new Patient { FullName = "Ahmed Hassan" },
        //        Doctor = new Doctor { FullName = "Dr. Mona Youssef" }
        //    },
        //    new Reservation
        //    {
        //        ReservationID = 5,
        //        PatientID = 4,
        //        DoctorID = 2,
        //        ReservationDate = new DateTime(2025, 4, 15, 15, 45, 0),
        //        Status = "Pending",
        //        AdditionalData = "Referred by GP",
        //        Patient = new Patient { FullName = "Laila Mahmoud" },
        //        Doctor = new Doctor { FullName = "Dr. Hossam Ali" }
        //    }
        //};
        //var filteredAppointments = allAppointments
        //    .Where(r => r.ReservationDate >= startDate &&
        //                r.ReservationDate < endDate &&
        //                r.Status != "Cancelled")
        //    .OrderBy(r => r.ReservationDate)
        //    .ToList();

        var reservations = _unitOfWork.ReservationService.GetAllReservationsAsync();

        return View(reservations);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateReservationStatus(ReservationEditStatusVM model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _unitOfWork.DoctorService.UpdateReservationStatusAsync(model);
        if (result.Succeeded)
            return RedirectToAction(nameof(Index));
        else
            return BadRequest(result.Error);
    }

    public async Task<IActionResult> Billings()
    {
        var reservations = _unitOfWork.BillingService.GetAllBillingsAsync();

        return View(reservations);
    }
}
