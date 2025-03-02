using Hospital_Administration_System.Models;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;

namespace H.Controllers
{
    public class BillingController : Controller
    {
        public static List<Billing> BillingModel = new List<Billing>();
        public IActionResult Index()
        {
            var model = new Billing {} ;
            return View(); 
        }

        public IActionResult Add()
        {
            return View(); 
        }
        
        
    }
}
