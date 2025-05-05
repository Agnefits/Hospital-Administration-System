namespace Hospital_Administration_System.ViewModels.Doctor
{
    public class DoctorResponseVM
    {
        public bool Succeeded { get; set; }
        public string Error { get; set; }
        public string Message { get; set; }
        public Reservation? Reservation { get; set; }
        public MedicalRecord? MedicalRecord { get; set; }
        public Prescription? Prescription { get; set; }
    }
}
