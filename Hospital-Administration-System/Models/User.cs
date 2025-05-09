
namespace Hospital_Administration_System.Models;
public class User : IdentityUser
{
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public string? AdditionalData { get; set; }

    public bool Deleted { get; set; } = false;
    
    public Admin Admin { get; set; }
    public Doctor Doctor { get; set; }
    public Nurse Nurse { get; set; }
    public Pharmacist Pharmacist { get; set; }
    public Patient Patient { get; set; }
    public ICollection<Log> Logs { get; set; }
}
