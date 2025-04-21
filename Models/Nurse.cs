
namespace Hospital_Administration_System.Models;

public class Nurse
{
    [Key]
    public int NurseID { get; set; }

    [Required, ForeignKey("User")]
    public string UserID { get; set; }
    public User User { get; set; }

    [Required, MaxLength(255)]
    public string FullName { get; set; }

    [Required, ForeignKey("Department")]
    public int DepartmentID { get; set; }
    public Department Department { get; set; }

    [Required, MaxLength(50)]
    public string ContactNumber { get; set; }

    public string? AdditionalData { get; set; }

    public bool Deleted { get; set; } = false;
}
