using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Hospital_Administration_System.Models
{
    public class Pharmacist
    {
        [Key]
        public int PharmacistID { get; set; }

        [Required, ForeignKey("User")]
        public string UserID { get; set; }
        public User User { get; set; }

        [Required, MaxLength(255)]
        public string FullName { get; set; }

        [Required, ForeignKey("Pharmacy")]
        public int PharmacyID { get; set; }
        public Pharmacy Pharmacy { get; set; }

        [Required, MaxLength(50)]
        public string ContactNumber { get; set; }

        public string? AdditionalData { get; set; }

        public bool Deleted { get; set; } = false;
    }
}
