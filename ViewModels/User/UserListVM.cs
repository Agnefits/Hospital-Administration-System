namespace Hospital_Administration_System.ViewModels.User
{
    public class UserListVM
    {
        public string UserId { get; set; }
        public string? Email { get; set; }
        public string Role { get; set; } // Admin, Doctor, Nurse, Pharmacist
        public string FullName { get; set; }
        public string? Department { get; set; }
        public string? Branch { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime CreatedAt { get; set; }
        public Models.User? User { get; set; }
    }
}
