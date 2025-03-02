using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Hospital_Administration_System.Models
{
    public class Prescription
    {
        [Key]
        public int PrescriptionID { get; set; }

        [Required, ForeignKey("Patient")]
        public int PatientID { get; set; }
        public Patient Patient { get; set; }

        [Required, ForeignKey("Doctor")]
        public int DoctorID { get; set; }
        public Doctor Doctor { get; set; }

        [Required, MaxLength(255)]
        public string MedicationName { get; set; }

        [Required, MaxLength(50)]
        public string Dosage { get; set; }

        [Required]
        public string Instructions { get; set; }

        [Required]
        public DateTime IssuedDate { get; set; }
        public string AdditionalData { get; set; }
    }
}
