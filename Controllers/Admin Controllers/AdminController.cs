using Microsoft.AspNetCore.Mvc;

namespace Hospital_Administration_System.Controllers.Admin_Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
