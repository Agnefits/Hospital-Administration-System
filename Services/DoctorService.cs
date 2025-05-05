namespace Hospital_Administration_System.Services;

public class DoctorService : GenericRepository<Doctor>, IDoctorRepository
{

    public DoctorService(ApplicationDbContext context) : base(context)
    {

    }

    public async Task<Doctor> GetDoctorByIdAsync(int id)
    {
        return await GetByIdAsync(id);
    }

    public async Task<IEnumerable<Reservation>> GetDoctorReservationsAsync(int doctorId)
    {
        return await _context.Reservations
            .Include(r => r.Patient)
                .ThenInclude(p => p.Analyses)
            .Include(r => r.Patient)
                .ThenInclude(p => p.MedicalRecords)
            .Include(r => r.Patient)
                .ThenInclude(p => p.Prescriptions)
            .Where(r => r.DoctorID == doctorId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Reservation>> GetDepartmentReservationsAsync(int departmentId)
    {
        return await _context.Reservations
            .Include(r => r.Doctor)
            .Include(r => r.Patient)
                .ThenInclude(p => p.Analyses)
            .Include(r => r.Patient)
                .ThenInclude(p => p.MedicalRecords)
            .Include(r => r.Patient)
                .ThenInclude(p => p.Prescriptions)
            .Where(r => r.Doctor.DepartmentID == departmentId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Patient>> GetDepartmentPatientsAsync(int departmentId)
    {
        return await _context.Patients
                .Include(p => p.Analyses)
                .Include(p => p.MedicalRecords)
                .Include(p => p.Prescriptions)
            .Where(p => p.Reservations.Any(r => r.Doctor.DepartmentID == departmentId))
        .ToListAsync();
    }

    public async Task<IEnumerable<Prescription>> GetDepartmentPrescriptionsAsync(int departmentId)
    {
        return await _context.Prescriptions
                .Include(p => p.Doctor)
                .Include(p => p.Patient)
            .Where(p => p.Doctor.DepartmentID == departmentId)
            .ToListAsync();
    }

    public async Task<IEnumerable<MedicalRecord>> GetDepartmentMedicalsAsync(int departmentId, DateTime? startDate, DateTime? endDate, string? patientName)
    {
        var query = _context.MedicalRecords
            .Include(m => m.Patient)
            .Include(m => m.Doctor)
            .Where(p => p.Doctor.DepartmentID == departmentId)
            .AsQueryable(); // Start with a queryable collection

        // Apply filtering for start date, end date, and patient name directly on the database query
        if (startDate.HasValue)
        {
            query = query.Where(r => r.CreatedAt >= startDate.Value);
        }

        if (endDate.HasValue)
        {
            query = query.Where(r => r.CreatedAt <= endDate.Value);
        }

        if (!string.IsNullOrEmpty(patientName))
        {
            query = query.Where(r => r.Patient.FullName.Contains(patientName));
        }

        // Execute the query asynchronously
        var records = await query.OrderByDescending(m => m.CreatedAt).ToListAsync();

        return records;
    }

    public async Task<IEnumerable<Doctor>> GetDoctorsAsync()
    {
        return await GetAllAsync();
    }
}

