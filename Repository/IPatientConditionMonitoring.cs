
using Hospital_Administration_System.ViewModels.Patient;

namespace Hospital_Administration_System.Repository;

public interface IPatientConditionMonitoring : IRepository<PatientConditionMonitoring>
{
    Task AddPatientConditionMonitoring(PatientConditionMonitoringVM patientConditionMonitoring);
    Task UpdatePatientConditionMonitoring(PatientConditionMonitoringVM patientConditionMonitoring);
    Task<IEnumerable<PatientConditionMonitoring>> GetAllPatientConditionMonitoringAsync(int nurseId);
}
