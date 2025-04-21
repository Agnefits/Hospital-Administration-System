
namespace Hospital_Administration_System.Controllers.Patient_Controllers;

public class PatientController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public PatientController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public IActionResult Index()
    {
        return View();
    }
    public IActionResult PatientDashboard()
    {
        return View();
    }
}
