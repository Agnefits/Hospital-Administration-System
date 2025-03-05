using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Hospital_Administration_System.Models
{
    public class MedicalRecord
    {
        [Key]
        public int RecordID { get; set; }

        [Required, ForeignKey("Patient")]
        public int PatientID { get; set; }
        public Patient Patient { get; set; }

        [Required, ForeignKey("Doctor")]
        public int DoctorID { get; set; }
        public Doctor Doctor { get; set; }

        [Required]
        public string Diagnosis { get; set; }

        [Required]
        public string Treatment { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string? AdditionalData { get; set; }
    }
}
