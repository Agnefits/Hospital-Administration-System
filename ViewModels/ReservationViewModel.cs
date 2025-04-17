using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Hospital_Administration_System.ViewModels
{
    public class ReservationViewModel
    {
        public int? ReservationID { get; set; }

        [Required(ErrorMessage = "Please select a doctor.")]
        [Display(Name = "Doctor")]
        public int DoctorID { get; set; }

        public List<SelectListItem>? Doctors { get; set; }

        [Required(ErrorMessage = "Please select a date and time.")]
        [DataType(DataType.DateTime)]
        [Display(Name = "Appointment Date")]
        public DateTime ReservationDate { get; set; }

        [Display(Name = "Additional Notes")]
        [StringLength(255)]
        public string? AdditionalData { get; set; }
    }
}
