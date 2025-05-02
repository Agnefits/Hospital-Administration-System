namespace Hospital_Administration_System.ViewModels.Branch
{
    public class BranchResponseVM
    {
        public bool Succeeded { get; set; }
        public string Error { get; set; }
        public string Message { get; set; }
        public Models.Branch? Branch { get; set; }
    }
}
