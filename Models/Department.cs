
namespace Hospital_Administration_System.Models;

public class Department
{
    [Key]
    public int DepartmentID { get; set; }

    [Required, MaxLength(255)]
    public string Name { get; set; }

    [Required, ForeignKey("Branch")]
    public int BranchID { get; set; }
    public Branch Branch { get; set; }
    // 
    public string? HeadDoctorID { get; set; }
    [ForeignKey("HeadDoctorID")]
    public User HeadDoctor { get; set; }

    public string? AdditionalData { get; set; }

    public bool Deleted { get; set; } = false;

    // One-to-Many 
    public ICollection<Doctor> Doctors { get; set; }
    public ICollection<Nurse> Nurses { get; set; }
}
