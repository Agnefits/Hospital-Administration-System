namespace Hospital_Administration_System.Repository
{
    public interface IAuthRepository
    {
        Task<AuthResponseVM> Register(RegisterVM registerVM);
        Task<AuthResponseVM> verifyAccount(string userId, string verificationCode);
        Task<AuthResponseVM> Login(LoginVM loginVM);
        Task<AuthResponseVM> ChangePassword(User user, string oldPassword, string newPassword);
        Task<AuthResponseVM> ResetPassword(string email);
        Task<AuthResponseVM> ResetPassword(ResetPasswordConfirmVM resetPasswordConfirmVM);
        Task Logout();
    }
}
