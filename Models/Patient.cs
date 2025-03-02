using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Hospital_Administration_System.Models
{
    public class Patient
    {
        [Key]
        public int PatientID { get; set; }

        [Required, ForeignKey("User")]
        public int UserID { get; set; }
        public User User { get; set; }

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
        public string AdditionalData { get; set; }
        public bool Deleted { get; set; } = false;
        // one to many
        public ICollection<Reservation> Reservations { get; set; }
        public ICollection<Receipt> Receipts { get; set; }
        public ICollection<MedicalRecord> MedicalRecords { get; set; }
        public ICollection<Prescription> Prescriptions { get; set; }
        public ICollection<Billing> Billings { get; set; }
        public ICollection<Analysis> Analyses { get; set; }
    }
}
