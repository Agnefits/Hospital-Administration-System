
namespace Hospital_Administration_System.Controllers.Core_Controllers;

public class AccountController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public AccountController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public IActionResult Login()
    {
        return View();
    }

    public IActionResult Register()
    {
        return View();
    }

    public IActionResult ForgotPassword()
    {
        return View();
    }

    public IActionResult Logout()
    {
        // Add logic to clear authentication session
        return RedirectToAction("Index", "Home");
    }
}
