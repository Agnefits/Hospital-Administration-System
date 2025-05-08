namespace Hospital_Administration_System.ViewModels.User
{
    public class UserEditVM 
    {
        public string UserId { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Role { get; set; }

        public string FullName { get; set; }
        public string ContactNumber { get; set; }
        public int? DepartmentID { get; set; }
        public string? Specialization { get; set; }
        public int? PharmacyID { get; set; }
        public string AdditionalData { get; set; }
    }
}
