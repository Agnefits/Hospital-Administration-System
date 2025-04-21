
namespace Hospital_Administration_System.Controllers.Doctor_Controllers;

public class DoctorController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public DoctorController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public IActionResult Index()
    {
        return View();
    }


    public async Task<IActionResult> AppointmentsAsync()
    {
        //return (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
        //    ? PartialView("_AppointmentsPartial")
        //        : View();
        //    return PartialView("_AppointmentsPartial");
        //return View("Appointments");
        var appointments = await _unitOfWork.DoctorService.GetDoctorReservationsAsync(1);
        return View(appointments);
    }

    //[HttpPost, ActionName("DeleteAppointment")]
    //[ValidateAntiForgeryToken]
    //public async Task<ActionResult> DeleteConfirmed(int id)
    //{
    //    var ap = await  _doctorService.
    //    return RedirectToAction("AppointmentsAsync");
    //}

    public IActionResult Patients()
    {
        return PartialView();
    }

    public IActionResult Prescriptions()
    {
        return PartialView();
    }

    public async Task<IActionResult> MedicalRecords(DateTime? startDate, DateTime? endDate, string? patientName)
    {
        ViewData["StartDate"] = startDate?.ToString("yyyy-MM-dd");
        ViewData["EndDate"] = endDate?.ToString("yyyy-MM-dd");
        ViewData["PatientName"] = patientName;
        //var records = await _medicalRecordService.GetFilteredRecordsAsync(startDate, endDate, patientName); //Note Abdallah: Uncomment for the real function
        var records = new List<MedicalRecord>
{
new MedicalRecord
{
    RecordID = 1,
    PatientID = 1,
    DoctorID = 1,
    Diagnosis = "Acute Bronchitis",
    Treatment = "Prescribed antibiotics and rest",
    CreatedAt = new DateTime(2024, 12, 5, 9, 0, 0),
    AdditionalData = "Patient advised to avoid cold drinks",
    Patient = new Patient { FullName = "Ahmed Hassan" },
    Doctor = new Doctor { FullName = "Dr. Mona Youssef" }
},
new MedicalRecord
{
    RecordID = 2,
    PatientID = 1,
    DoctorID = 1,
    Diagnosis = "Seasonal Allergy",
    Treatment = "Antihistamines prescribed",
    CreatedAt = new DateTime(2025, 1, 12, 11, 30, 0),
    AdditionalData = "Scheduled follow-up in 2 weeks",
    Patient = new Patient { FullName = "Ahmed Hassan" },
    Doctor = new Doctor { FullName = "Dr. Mona Youssef" }
},
new MedicalRecord
{
    RecordID = 3,
    PatientID = 1,
    DoctorID = 1,
    Diagnosis = "Mild Concussion",
    Treatment = "Complete rest and hydration",
    CreatedAt = new DateTime(2025, 2, 3, 14, 45, 0),
    AdditionalData = "CT scan recommended if symptoms persist",
    Patient = new Patient { FullName = "Ahmed Hassan" },
    Doctor = new Doctor { FullName = "Dr. Mona Youssef" }
},
new MedicalRecord
{
    RecordID = 4,
    PatientID = 1,
    DoctorID = 1,
    Diagnosis = "High Blood Pressure",
    Treatment = "Lifestyle changes and ACE inhibitors",
    CreatedAt = new DateTime(2025, 3, 20, 16, 0, 0),
    AdditionalData = "Regular monitoring required",
    Patient = new Patient { FullName = "Ahmed Hassan" },
    Doctor = new Doctor { FullName = "Dr. Mona Youssef" }
},
new MedicalRecord
{
    RecordID = 5,
    PatientID = 1,
    DoctorID = 1,
    Diagnosis = "Type 2 Diabetes",
    Treatment = "Metformin 500mg twice daily",
    CreatedAt = new DateTime(2025, 4, 10, 10, 15, 0),
    AdditionalData = "Diet plan shared with the patient",
    Patient = new Patient { FullName = "Ahmed Hassan" },
    Doctor = new Doctor { FullName = "Dr. Mona Youssef" }
}
};


        return View(records);
    }
}
