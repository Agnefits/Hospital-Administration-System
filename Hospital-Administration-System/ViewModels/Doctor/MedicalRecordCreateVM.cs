namespace Hospital_Administration_System.ViewModels.Doctor
{
    public class MedicalRecordCreateVM
    {        
        [Required]
        public int PatientID { get; set; }

        [Required]
        public string Diagnosis { get; set; }

        [Required]
        public string Treatment { get; set; }

        public string? AdditionalData { get; set; }
    }
} 