namespace Hospital_Administration_System.ViewModels.Doctor
{
    public class PrescriptionCreateVM
    {
        [Required]
        public int PatientID { get; set; }

        [Required, MaxLength(255)]
        public string MedicationName { get; set; }

        [Required, MaxLength(50)]
        public string Dosage { get; set; }

        [Required]
        public string Instructions { get; set; }

        public string? AdditionalData { get; set; }
    }
} 