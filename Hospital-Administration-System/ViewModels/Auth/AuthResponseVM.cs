namespace Hospital_Administration_System.ViewModels.Auth
{
    public class AuthResponseVM
    {
        public bool Succeeded { get; set; }
        public string Error { get; set; }
        public string Message { get; set; }
        public List<string> Roles { get; set; }
    }
}
