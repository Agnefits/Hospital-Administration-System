using Microsoft.AspNetCore.Mvc;

namespace Hospital_Administration_System.Controllers.Nurse_Controllers
{
    public class NurseController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
