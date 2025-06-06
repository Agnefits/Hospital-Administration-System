﻿
namespace Hospital_Administration_System.Models;

public class Admin
{
    [Key]
    public int AdminID { get; set; }

    [Required, ForeignKey("User")]
    public string UserID { get; set; }
    public User User { get; set; }

    [Required, MaxLength(255)]
    public string FullName { get; set; }

    [Required, MaxLength(20)]
    public string ContactNumber { get; set; }

    public string? AdditionalData { get; set; }
    public bool Deleted { get; set; } = false;
}
