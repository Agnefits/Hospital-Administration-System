namespace Hospital_Administration_System.ViewModels.User
{
    public class UserResponseVM
    {
        public bool Succeeded { get; set; }
        public string Error { get; set; }
        public string Message { get; set; }
        public Models.User? User { get; set; }
    }
}
