using Hospital_Administration_System.ViewModels.Branch;

namespace Hospital_Administration_System.Controllers.Admin_Controllers
{
    //[Authorize(Roles = "Admin")] //Note Abdallah: Uncomment for authorization
    public class BranchController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public BranchController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            var branches = await _unitOfWork.BranchService.GetAllBranchesAsync();

            return View(branches);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BranchCreateVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var response = await _unitOfWork.BranchService.AddAsync(model);
            if (!response.Succeeded)
            {
                ModelState.AddModelError("", response.Error);
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var branch = await _unitOfWork.BranchService.GetByIdAsync(id);
            if (branch == null || branch.Deleted)
                return NotFound();

            var model = new BranchEditVM
            {
                BranchID = branch.BranchID,
                Name = branch.Name,
                Location = branch.Location,
                ContactNumber = branch.ContactNumber,
                AdditionalData = branch.AdditionalData
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(BranchEditVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var response = await _unitOfWork.BranchService.UpdateAsync(model);
            if (!response.Succeeded)
            {
                ModelState.AddModelError("", response.Error);
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            bool deleted = await _unitOfWork.BranchService.DeleteAsync(id);
            if (!deleted)
            {
                ModelState.AddModelError("", "Error deleting branch");
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
