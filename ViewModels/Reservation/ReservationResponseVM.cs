namespace Hospital_Administration_System.ViewModels.Reservation
{
    public class ReservationResponseVM
    {
        public bool Succeeded { get; set; }
        public string Error { get; set; }
        public string Message { get; set; }
        public Models.Reservation? Reservation { get; set; }
    }
}
