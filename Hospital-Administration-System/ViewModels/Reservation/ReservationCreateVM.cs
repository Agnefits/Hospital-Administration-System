namespace Hospital_Administration_System.ViewModels.Reservation
{
    public class ReservationCreateVM
    {
        public int? PatientID { get; set; }

        public int DoctorID { get; set; }

        public DateTime ReservationDate { get; set; }

        public string? AdditionalData { get; set; } = "N/A";
    }
}
