﻿

namespace Hospital_Administration_System.Models;

public class Log
{
    [Key]
    public int LogID { get; set; }

    [ForeignKey("User")]
    public string? UserID { get; set; }
    public User User { get; set; }

    [Required, MaxLength(255)]
    public string Action { get; set; }

    [Required, MaxLength(255)]
    public string TableName { get; set; }
    public string? RecordID { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.Now;

    [MaxLength(50)]
    public string? IPAddress { get; set; }
    public string? DeviceInfo { get; set; }
    public string? AdditionalData { get; set; }
}
