namespace Hospital_Administration_System.Models;

public class PatientConditionMonitoring
{
    public int Id { get; set; }
    // Patient Info
    [ForeignKey("Patient")]
    public int PatientId { get; set; }

    public Patient Patient { get; set; }
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
    [ForeignKey("Nurse")]
    public int NurseId { get; set; }

    public Nurse Nurse { get; set; }
    public string NurseName { get; set; } = string.Empty;
}
