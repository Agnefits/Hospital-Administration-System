namespace Hospital_Administration_System.ViewModels.Doctor;

public class ReservationRedirectionVM
{
    public int OldReservationID { get; set; }
    [Required(ErrorMessage = "Please select a doctor.")]
    public int DoctorID { get; set; }

    public string? AdditionalData { get; set; }
}
