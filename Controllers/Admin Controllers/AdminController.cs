using Microsoft.AspNetCore.Mvc;
using Hospital_Administration_System.Models;
using Hospital_Administration_System.Data;
using Microsoft.EntityFrameworkCore;

namespace Hospital_Administration_System.Controllers.Admin_Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> UpcomingAppointments()
        {
            var startDate = DateTime.Today;
            var endDate = DateTime.Today.AddDays(14).AddDays(1);

            var appointments = await _context.Reservations
                .Include(r => r.Patient)
                .Include(r => r.Doctor)
                .Where(r => r.ReservationDate >= startDate && 
                            r.ReservationDate < endDate &&
                            r.Status != "Cancelled")
                .OrderBy(r => r.ReservationDate)
                .ToListAsync();
            return View(appointments);
        }
    }
}
