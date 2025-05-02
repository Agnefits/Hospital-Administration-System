using Microsoft.AspNetCore.Authorization;

namespace Hospital_Administration_System.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthRepository _authService;
        public AuthController(IAuthRepository authService)
        {
            _authService = authService;
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
                    return RedirectToAction("Index", "Home");
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
            if (string.IsNullOrEmpty(userId) && string.IsNullOrEmpty(token))
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

    }
}
