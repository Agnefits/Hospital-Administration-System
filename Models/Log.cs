using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Hospital_Administration_System.Models
{
    public class Log
    {
        [Key]
        public int LogID { get; set; }

        [ForeignKey("User")]
        public int? UserID { get; set; }
        public User User { get; set; }

        [Required, MaxLength(255)]
        public string Action { get; set; }

        [Required, MaxLength(255)]
        public string TableName { get; set; }
        public int? RecordID { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now;

        [MaxLength(50)]
        public string IPAddress { get; set; }
        public string DeviceInfo { get; set; }
        public string AdditionalData { get; set; }
    }
}
