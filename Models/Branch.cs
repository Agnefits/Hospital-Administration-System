using System.ComponentModel.DataAnnotations;

namespace Hospital_Administration_System.Models
{
    public class Branch
    {
        [Key]
        public int BranchID { get; set; }

        [Required, MaxLength(255)]
        public string Name { get; set; }

        [Required, MaxLength(255)]
        public string Location { get; set; }

        [Required, MaxLength(20)]
        public string ContactNumber { get; set; }
        public string? AdditionalData { get; set; }
        public bool Deleted { get; set; } = false;

        public ICollection<Department> Departments { get; set; }
        public ICollection<Pharmacy> Pharmacies { get; set; }
        public ICollection<Laboratory> Laboratories { get; set; }
    }
}
