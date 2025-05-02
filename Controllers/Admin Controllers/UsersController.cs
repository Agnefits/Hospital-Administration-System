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
                    return RedirectToAction(nameof(Index));  // Redirect to the list of users
                }

                ModelState.AddModelError("", result.Error);  // Display error message
            }
            return View(model);  // Return the form with the model
        }

        public async Task<IActionResult> Edit(string id)
        {
            var user = await _unitOfWork.UserService.GetByIdAsync(id);
            if (user == null)
                return NotFound();

            return View(user);  // Return the Edit User form with user data
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserEditVM model)
        {
            if (ModelState.IsValid)
            {
                var result = await _unitOfWork.UserService.UpdateAsync(model);

                if (result.Succeeded)
                    return RedirectToAction(nameof(Index));  // Redirect to the list of users

                ModelState.AddModelError("", result.Error);  // Display error message
            }
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
    }
}
