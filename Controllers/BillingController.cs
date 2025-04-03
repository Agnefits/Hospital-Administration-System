using Hospital_Administration_System.Models;
using Hospital_Administration_System.Services;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;

namespace H.Controllers
{
    public class BillingController : Controller
    {
        private readonly BillingService _billingService;
        private readonly PatientService _patientService;

        public BillingController(BillingService billingService, PatientService patientService)
        {
            _billingService = billingService;
            _patientService = patientService;
        }

        public async Task<IActionResult> Index()
        {
            var billing = await _billingService.GetAllBillingsAsync();
            return View(billing); 
        }

        public async Task<IActionResult> Add()
        {
            ViewData["Patients"] = await _patientService.GetAllPatientsAsync();
            return View(); 
        }

        [HttpPost]
        public async Task<IActionResult> Add(Billing billing)
        {
            try
            {
                await _billingService.AddBillingAsync(billing);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        
        
    }
}
