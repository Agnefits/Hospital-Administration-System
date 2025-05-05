using System.Security.Claims;

namespace Hospital_Administration_System.Services
{
    public class AuthService : IAuthRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IEmailRepository _emailService;
        private readonly IUnitOfWork _unitOfWork;

        public AuthService(UserManager<User> userManager,
                           SignInManager<User> signInManager,
                           IEmailRepository emailService,
                           IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _unitOfWork = unitOfWork;
        }


        public async Task<AuthResponseVM> Register(RegisterVM register)
        {
            var isUserExist = await _userManager.FindByEmailAsync(register.Email);
            if (isUserExist != null)
                return new AuthResponseVM { Succeeded = false, Error = "Email already exists." };

            var user = new User { UserName = register.Email, Email = register.Email, CreatedAt = DateTime.Now };
            user.Patient = new Patient
            {
                User = user,
                FullName = register.FullName,
                Gender = register.Gender,
                BloodType = register.BloodType,
                DateOfBirth = register.DateOfBirth,
                ContactNumber = register.ContactNumber,
                EmergencyContact = register.EmergencyContact,
                Address = register.Address,
            };

            var result = await _userManager.CreateAsync(user, register.Password);
            if (result.Succeeded)
                result = await _userManager.AddToRoleAsync(user, "Patient");
            if (result.Succeeded)
            {
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                // Send the token to the user's email
                await _emailService.SendVerificationEmailAsync(user.Email, user.Id, token);
                return new AuthResponseVM { Succeeded = true, Message = "Account Registered successfully. Please check your email for verification code." };
            }

            return new AuthResponseVM { Succeeded = false, Error = result.Errors.FirstOrDefault()?.Description };
        }

        public async Task<AuthResponseVM> verifyAccount(string userId, string verificationCode)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return new AuthResponseVM { Succeeded = false, Error = "User not found." };

            var result = await _userManager.VerifyUserTokenAsync(user, "Default", "EmailVerification", verificationCode);
            if (result)
            {
                user.EmailConfirmed = true;
                await _emailService.SendWelcomeEmailAsync(user.Email);
                return new AuthResponseVM { Succeeded = true, Message = "Account verified successfully." };
            }

            return new AuthResponseVM { Succeeded = false, Error = "Invalid verification code." };
        }

        public async Task<AuthResponseVM> Login(LoginVM login)
        {
            var user = await _userManager.FindByEmailAsync(login.Email);
            if (user == null)
                return new AuthResponseVM { Succeeded = false, Error = "Invalid email or password." };

            if (!user.EmailConfirmed)
                return new AuthResponseVM { Succeeded = false, Error = "Email not verified." };

            var result = await _signInManager.CheckPasswordSignInAsync(user, login.Password, false);
            if (result.Succeeded)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Email, user.Email)
                };

                var roles = await _userManager.GetRolesAsync(user);

                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }

                await _signInManager.SignInWithClaimsAsync(user, true, claims);

                return new AuthResponseVM { Succeeded = true, Message = "Login successful.", Roles = roles.ToList() };
            }

            return new AuthResponseVM { Succeeded = false, Error = "Invalid email or password." };
        }

        public async Task<AuthResponseVM> ChangePassword(User user, string oldPassword, string newPassword)
        {
            var isUserExist = await _userManager.FindByEmailAsync(user.Email);
            if (isUserExist == null)
                return new AuthResponseVM { Succeeded = false, Error = "User not found." };

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, oldPassword);
            if (!isPasswordValid)
                return new AuthResponseVM { Succeeded = false, Error = "Invalid old password." };

            if (oldPassword == newPassword)
                return new AuthResponseVM { Succeeded = false, Error = "New password cannot be the same as old password." };

            var result = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
            if (result.Succeeded)
                return new AuthResponseVM { Succeeded = true, Message = "Password changed successfully." };

            return new AuthResponseVM { Succeeded = false, Error = result.Errors.FirstOrDefault()?.Description };
        }

        public async Task<AuthResponseVM> ResetPassword(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return new AuthResponseVM { Succeeded = false, Error = "User not found." };

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            // Send the token to the user's email
            await _emailService.SendPasswordResetEmailAsync(email, token);
            return new AuthResponseVM { Succeeded = true, Message = "Password reset email sent successfully." };

        }

        public async Task<AuthResponseVM> ResetPassword(ResetPasswordConfirmVM reset)
        {
            var user = await _userManager.FindByEmailAsync(reset.Email);
            if (user == null)
                return new AuthResponseVM { Succeeded = false, Error = "User not found." };

            var isTokenValid = await _userManager.VerifyUserTokenAsync(user, "Default", "PasswordReset", reset.Token);
            if (!isTokenValid)
                return new AuthResponseVM { Succeeded = false, Error = "Invalid password reset token." };

            var result = await _userManager.ResetPasswordAsync(user, reset.Token, reset.NewPassword);
            if (result.Succeeded)
                return new AuthResponseVM { Succeeded = true, Message = "Password reset successfully." };

            return new AuthResponseVM { Succeeded = false, Error = result.Errors.FirstOrDefault()?.Description };
        }
        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
