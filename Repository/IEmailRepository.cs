namespace Hospital_Administration_System.Repository
{
    public interface IEmailRepository
    {
        Task<bool> SendEmailAsync(string email, string subject, string message);
        Task<bool> SendVerificationEmailAsync(string email, string userId, string verificationCode);
        Task<bool> SendPasswordResetEmailAsync(string email, string resetCode);
        Task<bool> SendWelcomeEmailAsync(string email);
    }
}
