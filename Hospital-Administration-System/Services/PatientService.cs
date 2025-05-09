
namespace Hospital_Administration_System.Services;

public class PatientService : GenericRepository<Patient>, IPatientRepository
{
    public PatientService(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Reservation>> GetPatientReservations(int patientId)
    {
        return await _context.Reservations
            .Include(r => r.Patient)
            .Include(r => r.Doctor)
            .Where(r => r.PatientID == patientId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Prescription>> GetPatientPrescriptions(int patientId)
    {
        return await _context.Prescriptions
            .Include(p => p.Patient)
            .Include(p => p.Doctor)
            .Where(p => p.PatientID == patientId)
            .ToListAsync();
    }

    public async Task<IEnumerable<MedicalRecord>> GetPatientMedicals(int patientId)
    {
        return await _context.MedicalRecords
            .Include(m => m.Patient)
            .Include(m => m.Doctor)
            .Where(m => m.PatientID == patientId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Analysis>> GetPatientAnalysis(int patientId)
    {
        return await _context.Analyses
            .Include(a => a.Patient)
            .Include(a => a.Laboratory)
            .Where(a => a.PatientID == patientId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Billing>> GetPatientBills(int patientId)
    {
        return await _context.Billings
            .Include(b => b.Patient)
            .Where(b => b.PatientID == patientId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Receipt>> GetPatientReceipts(int patientId)
    {
        return await _context.Receipts
            .Include(r => r.Patient)
            .Where(r => r.PatientID == patientId)
            .ToListAsync();
    }


}
