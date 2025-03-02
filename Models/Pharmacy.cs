using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Hospital_Administration_System.Models
{
    public class Pharmacy
    {
        [Key]
        public int PharmacyID { get; set; }

        [Required, ForeignKey("Branch")]
        public int BranchID { get; set; }
        public Branch Branch { get; set; }

        [Required, MaxLength(255)]
        public string Name { get; set; }

        [Required, MaxLength(255)]
        public string Location { get; set; }

        public string AdditionalData { get; set; }

        public bool Deleted { get; set; } = false;

        // One-to-Many
        public ICollection<Pharmacist> Pharmacists { get; set; }
    }
}
