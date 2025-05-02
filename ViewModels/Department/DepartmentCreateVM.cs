namespace Hospital_Administration_System.ViewModels.Department
{
    public class DepartmentCreateVM
    {
        [Required, MaxLength(255)]
        public string Name { get; set; }

        [Required]
        public int BranchID { get; set; }

        public string? HeadDoctorID { get; set; }

        public string? AdditionalData { get; set; }
    }
}
