namespace Hospital_Administration_System.ViewModels.Auth
{
    public class VerifyEmailVM
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Token { get; set; }
    }
}
