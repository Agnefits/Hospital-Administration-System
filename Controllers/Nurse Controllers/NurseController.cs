using Hospital_Administration_System.Services;
using Microsoft.AspNetCore.Mvc;

namespace Hospital_Administration_System.Controllers.Nurse_Controllers
{
    public class NurseController : Controller
    {
        private readonly NurseService _nurseService;

        public NurseController(NurseService nurseService)
        {
            _nurseService = nurseService;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
