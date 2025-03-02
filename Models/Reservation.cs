﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Hospital_Administration_System.Models
{
    public class Reservation
    {
        [Key]
        public int ReservationID { get; set; }

        [ForeignKey("Patient")]
        public int PatientID { get; set; }
        public Patient Patient { get; set; }

        [ForeignKey("Doctor")]
        public int DoctorID { get; set; }
        public Doctor Doctor { get; set; }

        public DateTime ReservationDate { get; set; }

        [Required, MaxLength(50)]
        public string Status { get; set; }  // "Pending", "Confirmed", "Cancelled"

        public string? AdditionalData { get; set; }
    }
}
