namespace Hospital_Administration_System.ViewModels.Auth
{
    public class ResetPasswordVM
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
