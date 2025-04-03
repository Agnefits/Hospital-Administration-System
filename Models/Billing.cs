using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Hospital_Administration_System.Models
{
    public class Billings
    {
        public List<Billing> BillingModel { get; set; } = new List<Billing>();
    }
    public class Billing
    {
        [Key]
        public int BillID { get; set; }

        [Required, ForeignKey("Patient")]
        public int PatientID { get; set; }
        public Patient Patient { get; set; }

        [Required]
        public decimal TotalAmount { get; set; }

        [Required, MaxLength(50)]
        public string Status { get; set; } 
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string? AdditionalData { get; set; }
    }
}
