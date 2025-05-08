
using Hospital_Administration_System.ViewModels.Patient;

namespace Hospital_Administration_System.Services;

public class PatientConditionMonitoringService : GenericRepository<PatientConditionMonitoring>, IPatientConditionMonitoring
{
    public PatientConditionMonitoringService(ApplicationDbContext context) : base(context)
    {
    }

    public async Task AddPatientConditionMonitoring(PatientConditionMonitoringVM patientConditionMonitoring)
    {
        var patient = new PatientConditionMonitoring
        {
            PatientId = patientConditionMonitoring.PatientId,
            FullName = patientConditionMonitoring.FullName,
            Age = patientConditionMonitoring.Age,
            Gender = patientConditionMonitoring.Gender,
            Temperature = patientConditionMonitoring.Temperature,
            HeartRate = patientConditionMonitoring.HeartRate,
            SystolicBP = patientConditionMonitoring.SystolicBP,
            DiastolicBP = patientConditionMonitoring.DiastolicBP,
            RespiratoryRate = patientConditionMonitoring.RespiratoryRate,
            OxygenSaturation = patientConditionMonitoring.OxygenSaturation,
            ConditionNotes = patientConditionMonitoring.ConditionNotes,
            MonitoringTime = patientConditionMonitoring.MonitoringTime,
            MedicationGiven = patientConditionMonitoring.MedicationGiven,
            Dosage = patientConditionMonitoring.Dosage,
            NurseId = patientConditionMonitoring.NurseId,
            NurseName = patientConditionMonitoring.NurseName
        };
        _context.PatientConditionMonitorings.Add(patient);
        await _context.SaveChangesAsync(); 
    }

    public async Task<IEnumerable<PatientConditionMonitoring>> GetAllPatientConditionMonitoringAsync(int nurseId)
    {
        var patientConditionMonitorings = await _context.PatientConditionMonitorings
            .Where(p => p.NurseId == nurseId)
            .ToListAsync();
        return patientConditionMonitorings;
    }

    public async Task UpdatePatientConditionMonitoring(PatientConditionMonitoringVM patientConditionMonitoring)
    {
        var patient = new PatientConditionMonitoring
        {
            PatientId = patientConditionMonitoring.PatientId,
            FullName = patientConditionMonitoring.FullName,
            Age = patientConditionMonitoring.Age,
            Gender = patientConditionMonitoring.Gender,
            Temperature = patientConditionMonitoring.Temperature,
            HeartRate = patientConditionMonitoring.HeartRate,
            SystolicBP = patientConditionMonitoring.SystolicBP,
            DiastolicBP = patientConditionMonitoring.DiastolicBP,
            RespiratoryRate = patientConditionMonitoring.RespiratoryRate,
            OxygenSaturation = patientConditionMonitoring.OxygenSaturation,
            ConditionNotes = patientConditionMonitoring.ConditionNotes,
            MonitoringTime = patientConditionMonitoring.MonitoringTime,
            MedicationGiven = patientConditionMonitoring.MedicationGiven,
            Dosage = patientConditionMonitoring.Dosage,
            NurseId = patientConditionMonitoring.NurseId,
            NurseName = patientConditionMonitoring.NurseName
        };
        _context.PatientConditionMonitorings.Update(patient);
        await _context.SaveChangesAsync();
    }
}
