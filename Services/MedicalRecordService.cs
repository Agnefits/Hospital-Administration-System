
namespace Hospital_Administration_System.Services;


public class MedicalRecordService : GenericRepository<MedicalRecord>, IMedicalRecordRepository
{
    public MedicalRecordService(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<MedicalRecord>> GetAllWithDetailsAsync()
    {
        return await _context.MedicalRecords
            .Include(m => m.Patient)
            .Include(m => m.Doctor)
            .OrderByDescending(m => m.CreatedAt)
            .ToListAsync();
    }

    public async Task<MedicalRecord?> GetWithDetailsByIdAsync(int id)
    {
        return await _context.MedicalRecords
            .Include(m => m.Patient)
            .Include(m => m.Doctor)
            .FirstOrDefaultAsync(m => m.RecordID == id);
    }
}
