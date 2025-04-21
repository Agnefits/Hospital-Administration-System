
namespace Hospital_Administration_System.Models;

public class Analysis
{
    [Key]
    public int AnalysisID { get; set; }

    [Required, ForeignKey("Patient")]
    public int PatientID { get; set; }
    public Patient Patient { get; set; }

    [Required, ForeignKey("Laboratory")]
    public int LabID { get; set; }
    public Laboratory Laboratory { get; set; }

    [Required, MaxLength(255)]
    public string TestName { get; set; }
    public string Result { get; set; }

    [Required]
    public DateTime TestDate { get; set; }
    public string? AdditionalData { get; set; }
}
