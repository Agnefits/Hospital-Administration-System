using System.Diagnostics;
using Hospital_Administration_System.Models;
using Hospital_Administration_System.Services;
using Microsoft.AspNetCore.Mvc;

namespace Hospital_Administration_System.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DoctorService _doctorService;

        public HomeController(ILogger<HomeController> logger, DoctorService doctorService)
        {
            _logger = logger;
            _doctorService = doctorService;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Services()
        {
            return View();
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
