

namespace Hospital_Administration_System.Controllers;

public class BillingController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public BillingController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IActionResult> Index()
    {
        var billing = await _unitOfWork.BillingService.GetAllBillingsAsync();
        return View(billing); 
    }

    public async Task<IActionResult> Add()
    {
        ViewData["Patients"] = await _unitOfWork.PatientService.GetAllPatientsAsync();
        return View(); 
    }

    [HttpPost]
    public async Task<IActionResult> Add(Billing billing)
    {
        try
        {
            await _unitOfWork.BillingService.AddBillingAsync(billing);
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            return View();
        }
    }
    
    
}
