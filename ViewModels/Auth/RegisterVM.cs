namespace Hospital_Administration_System.ViewModels.Auth
{
    public class RegisterVM
    {
        [Required, MaxLength(255)]
        public string FullName { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required, MaxLength(10)] // مثال: Male, Female, Other
        public string Gender { get; set; }

        [Required, MaxLength(50)]
        public string ContactNumber { get; set; }

        [Required]
        public string Address { get; set; }

        [Required, MaxLength(50)]
        public string EmergencyContact { get; set; }

        [Required, MaxLength(10)]
        public string BloodType { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        public string Password { get; set; }
    }
}
