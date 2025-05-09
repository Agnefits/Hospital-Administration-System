namespace Hospital_Administration_System.ViewModels.Patient;

public class PatientConditionMonitoringVM
{
    // Patient Info
    public int PatientId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public int Age { get; set; }
    public string Gender { get; set; } = string.Empty;

    // Vitals
    public float Temperature { get; set; }
    public int HeartRate { get; set; }
    public int SystolicBP { get; set; } // Upper
    public int DiastolicBP { get; set; } // Lower
    public int RespiratoryRate { get; set; }
    public int OxygenSaturation { get; set; }

    // Condition & Observations
    public string ConditionNotes { get; set; } = string.Empty;
    public DateTime MonitoringTime { get; set; }

    // Medication Info
    public string? MedicationGiven { get; set; }
    public string? Dosage { get; set; }

    // Nurse Info
    public int NurseId { get; set; }
    public string NurseName { get; set; } = string.Empty;
}