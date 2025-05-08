using Hospital_Administration_System.ViewModels.User;

namespace Hospital_Administration_System.Controllers.Admin_Controllers
{

    //[Authorize(Roles = "Admin")] //Note Abdallah: Uncomment for authorization
    public class UsersController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public UsersController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _unitOfWork.UserService.GetAllUsersAsync();
            return View(users);
        }

        public async Task<IActionResult> Details(string id)
        {
            var user = await _unitOfWork.UserService.GetByIdAsync(id);
            if (user == null)
                return NotFound();

            return View(user);  // Returns the user details view
        }

        public IActionResult Create()
        {
            PopulateDropdowns();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserCreateVM model)
        {
            if (ModelState.IsValid)
            {
                var result = await _unitOfWork.UserService.AddAsync(model);

                if (result.Succeeded)
                {
                    TempData["SuccessMessage"] = "User created successfully!";
                    return RedirectToAction(nameof(Index));
                }

                TempData["ErrorMessage"] = result.Error;
            }

            PopulateDropdowns();
            return View(model);
        }

       

        public async Task<IActionResult> Edit(string id)
        {
            var user = await _unitOfWork.UserService.GetByIdAsync(id);
            if (user == null)
                return NotFound();

            var model = MapUserToEditVM(user);  // Map user data to the Edit ViewModel
            PopulateDropdowns();  // Populate dropdowns for roles, departments, and pharmacies
            return View(model);  // Return the Edit User form with user data
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserEditVM model)
        {
            if (ModelState.IsValid)
            {
                var result = await _unitOfWork.UserService.UpdateAsync(model);

                if (result.Succeeded)
                {
                    TempData["SuccessMessage"] = "User updated successfully!";

                    return RedirectToAction(nameof(Index));  // Redirect to the list of users
                }

                ModelState.AddModelError("", result.Error);  // Display error message
                TempData["ErrorMessage"] = result.Error;
            }
            else
            {
                // Capture validation errors
                TempData["ErrorMessage"] = "Please correct the form errors.";
            }
            PopulateDropdowns();  // Populate dropdowns for roles, departments, and pharmacies
            return View(model);  // Return the form with the model
        }

        public async Task<IActionResult> Delete(string id)
        {
            var user = await _unitOfWork.UserService.GetByIdAsync(id);
            if (user == null)
                return NotFound();

            return View(user);  // Show delete confirmation view
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var success = await _unitOfWork.UserService.DeleteAsync(id);
            if (success)
                return RedirectToAction(nameof(Index));  // Redirect to the user list

            return NotFound();  // User not found
        }



        private UserEditVM MapUserToEditVM(User user)
        {
            var vm = new UserEditVM
            {
                UserId = user.Id,
                Email = user.Email,
                AdditionalData = user.AdditionalData,
                Role = user.GetRole()
            };

            switch (user.GetRole().ToLower())
            {
                case "admin":
                    vm.FullName = user.Admin?.FullName;
                    vm.ContactNumber = user.Admin?.ContactNumber;
                    break;
                case "doctor":
                    vm.FullName = user.Doctor?.FullName;
                    vm.ContactNumber = user.Doctor?.ContactNumber;
                    vm.DepartmentID = user.Doctor?.DepartmentID;
                    vm.Specialization = user.Doctor?.Specialization;
                    break;
                case "nurse":
                    vm.FullName = user.Nurse?.FullName;
                    vm.ContactNumber = user.Nurse?.ContactNumber;
                    vm.DepartmentID = user.Nurse?.DepartmentID;
                    break;
                case "pharmacist":
                    vm.FullName = user.Pharmacist?.FullName;
                    vm.ContactNumber = user.Pharmacist?.ContactNumber;
                    vm.PharmacyID = user.Pharmacist?.PharmacyID;
                    break;
            }

            return vm;
        }

        private void PopulateDropdowns()
        {
            ViewBag.Roles = new SelectList(new List<string> { "Admin", "Doctor", "Nurse", "Pharmacist", "Patient" });
            ViewBag.Departments = new SelectList(
                _unitOfWork.DepartmentService.GetAllDepartmentsAsync().Result,
                "DepartmentID", "Name");
            ViewBag.Pharmacies = new SelectList(
                _unitOfWork.PharmacistService.GetAllPharmaciesAsync().Result,
                "PharmacyID", "Name");
        }
    }
}
