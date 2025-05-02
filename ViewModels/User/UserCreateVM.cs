namespace Hospital_Administration_System.ViewModels.User
{
    public class 
        UserCreateVM
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Role { get; set; } // Enum أو DropDown [Doctor, Nurse, Admin, Pharmacist]

        public string FullName { get; set; }
        public string ContactNumber { get; set; }

        public int? DepartmentID { get; set; } // Doctor/Nurse
        public string? Specialization { get; set; } // Doctor
        public int? PharmacyID { get; set; } // Pharmacist

        public string? AdditionalData { get; set; }
    }
}
