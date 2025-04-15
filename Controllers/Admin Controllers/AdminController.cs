using Hospital_Administration_System.Models;
using Hospital_Administration_System.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hospital_Administration_System.Controllers.Admin_Controllers
{
    public class AdminController : Controller
    {
        private readonly UserService _userService;
        private readonly LogService _logService;

        public AdminController(UserService userService, LogService logService)
        {
            _userService = userService;
            _logService = logService;
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
    }
}
