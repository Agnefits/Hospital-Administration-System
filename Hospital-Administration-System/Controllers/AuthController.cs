using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Data;

namespace Hospital_Administration_System.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthRepository _authService;
        private readonly UserManager<User> _userManager;
        public AuthController(IAuthRepository authService, UserManager<User> userManager)
        {
            _authService = authService;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model)
        {
            if (ModelState.IsValid)
            {
                var result = await _authService.Login(model);
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByEmailAsync(model.Email);
                    var roles = await _userManager.GetRolesAsync(user);

                    return RedirectToRoleDashboard(roles);
                    //return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", result.Error);
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM model)
        {
            if (ModelState.IsValid)
            {
                var result = await _authService.Register(model);
                if (result.Succeeded)
                {
                    return RedirectToAction("Login");
                }
                ModelState.AddModelError("", result.Error);
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (!string.IsNullOrEmpty(userId) && !string.IsNullOrEmpty(token))
            {
                var result = await _authService.verifyAccount(userId, token);
                if (result.Succeeded)
                {
                    return RedirectToAction("Login");
                }
            }
            return View();
        }

        [HttpGet]
        public IActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM model)
        {
            if (ModelState.IsValid)
            {
                var result = await _authService.ResetPassword(model.Email);
                if (result.Succeeded)
                {
                    return RedirectToAction("ResetPasswordConfirm", model.Email);
                }
                ModelState.AddModelError("", result.Error);
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ResetPasswordConfirm(string email)
        {
            if (string.IsNullOrEmpty(email))
                return View(email);
            else
                return RedirectToAction("ResetPassword");
        }

        [HttpPost]
        public async Task<IActionResult> ResetPasswordConfirm(ResetPasswordConfirmVM model)
        {
            if (ModelState.IsValid)
            {
                var result = await _authService.ResetPassword(model);
                if (result.Succeeded)
                {
                    return RedirectToAction("Login");
                }
                ModelState.AddModelError("", result.Error);
            }
            return View(model);
        }

        //[Authorize] //Note Abdallah: Uncomment for authorization
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _authService.Logout();
            return RedirectToAction("Login");
        }

        private IActionResult RedirectToRoleDashboard(IList<string> roles)
        {
            if (roles.Contains("Admin")) return RedirectToAction("Index", "Admin");
            if (roles.Contains("Doctor")) return RedirectToAction("Index", "Doctor");
            if (roles.Contains("Nurse")) return RedirectToAction("Index", "Nurse");
            if (roles.Contains("Patient")) return RedirectToAction("PatientDashboard", "Patient");

            return RedirectToAction("Index", "Home");
        }
    }
}
