namespace Hospital_Administration_System.Repository;

public interface IPatientRepository : IRepository<Patient>
{
    Task<IEnumerable<Reservation>> GetPatientReservations(int patientId);
    Task<IEnumerable<Prescription>> GetPatientPrescriptions(int patientId);
    Task<IEnumerable<MedicalRecord>> GetPatientMedicals(int patientId);
    Task<IEnumerable<Analysis>> GetPatientAnalysis(int patientId);
    Task<IEnumerable<Billing>> GetPatientBills(int patientId);
    Task<IEnumerable<Receipt>> GetPatientReceipts(int patientId);
}
