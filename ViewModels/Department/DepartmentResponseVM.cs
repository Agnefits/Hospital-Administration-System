namespace Hospital_Administration_System.ViewModels.Department
{
    public class DepartmentResponseVM
    {
        public bool Succeeded { get; set; }
        public string Error { get; set; }
        public string Message { get; set; }
        public Models.Department? Department { get; set; }
    }
}
