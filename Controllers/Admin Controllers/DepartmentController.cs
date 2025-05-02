using Hospital_Administration_System.Models;
using Hospital_Administration_System.ViewModels.Department;
using Microsoft.AspNetCore.Authorization;

namespace Hospital_Administration_System.Controllers.Admin_Controllers
{
    //[Authorize(Roles = "Admin")] //Note Abdallah: Uncomment for authorization
    public class DepartmentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public DepartmentController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            var departments = await _unitOfWork.DepartmentService.GetAllDepartmentsAsync();

            return View(departments);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DepartmentCreateVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var response = await _unitOfWork.DepartmentService.AddAsync(model);
            if (!response.Succeeded)
            {
                ModelState.AddModelError("", response.Error);
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var department = await _unitOfWork.DepartmentService.GetByIdAsync(id);
            if (department == null || department.Deleted)
                return NotFound();

            return View(department);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(DepartmentEditVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var response = await _unitOfWork.DepartmentService.UpdateAsync(model);
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
            bool deleted = await _unitOfWork.DepartmentService.DeleteAsync(id);
            if (!deleted)
            {
                ModelState.AddModelError("", "Error deleting department");
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
