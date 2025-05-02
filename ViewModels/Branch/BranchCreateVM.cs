namespace Hospital_Administration_System.ViewModels.Branch
{
    public class BranchCreateVM
    {
        [Required, MaxLength(255)]
        public string Name { get; set; }

        [Required, MaxLength(255)]
        public string Location { get; set; }

        [Required, MaxLength(20)]
        public string ContactNumber { get; set; }

        public string? AdditionalData { get; set; }

    }
}
