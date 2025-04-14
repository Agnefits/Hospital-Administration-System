using Hospital_Administration_System.Services;
using Microsoft.AspNetCore.Mvc;

namespace Hospital_Administration_System.Controllers.Admin_Controllers
{
    public class AdminController : Controller
    {
        private readonly UserService _userService;

        public AdminController(UserService userService)
        {
            _userService = userService;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
