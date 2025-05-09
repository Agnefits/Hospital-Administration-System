
namespace Hospital_Administration_System.Models;

public class Receipt
{
    [Key]
    public int ReceiptID { get; set; }

    [Required, ForeignKey("Patient")]
    public int PatientID { get; set; }
    public Patient Patient { get; set; }

    [Required]
    public decimal Amount { get; set; }

    [Required, MaxLength(50)]
    public string PaymentMethod { get; set; } 

    [Required]
    public DateTime PaymentDate { get; set; }
    public string? AdditionalData { get; set; }
}
