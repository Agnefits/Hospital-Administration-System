using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Hospital_Administration_System.Models
{
    public class Doctor
    {
        [Key]
        public int DoctorID { get; set; }

        [Required, ForeignKey("User")]
        public int UserID { get; set; }
        public User User { get; set; }

        [Required, MaxLength(255)]
        public string FullName { get; set; }

        [Required, ForeignKey("Department")]
        public int DepartmentID { get; set; }
        public Department Department { get; set; }

        [Required, MaxLength(255)]
        public string Specialization { get; set; }

        [Required, MaxLength(20)]
        public string ContactNumber { get; set; }

        public string? AdditionalData { get; set; }

        public bool Deleted { get; set; } = false;

        // One-to-Many 
        public ICollection<Reservation> Reservations { get; set; }
        public ICollection<MedicalRecord> MedicalRecords { get; set; }
        public ICollection<Prescription> Prescriptions { get; set; }
    }
}
