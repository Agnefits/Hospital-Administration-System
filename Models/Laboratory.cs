
namespace Hospital_Administration_System.Models;

public class Laboratory
{
    [Key]
    public int LabID { get; set; }

    [Required, ForeignKey("Branch")]
    public int BranchID { get; set; }
    public Branch Branch { get; set; }

    [Required, MaxLength(255)]
    public string Name { get; set; }

    [Required, MaxLength(255)]
    public string Location { get; set; }

    public string? AdditionalData { get; set; }

    public bool Deleted { get; set; } = false;

    public ICollection<Analysis> Analyses { get; set; }
}
