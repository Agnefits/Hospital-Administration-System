using Hospital_Administration_System.Data;
using Hospital_Administration_System.Models;
using Hospital_Administration_System.Repository;
using Microsoft.EntityFrameworkCore;

namespace Hospital_Administration_System.Services
{
    public interface IMedicalRecordRepository : IRepository<MedicalRecord>
    {
        Task<IEnumerable<MedicalRecord>> GetAllWithDetailsAsync();
        Task<MedicalRecord?> GetWithDetailsByIdAsync(int id);
        Task<IEnumerable<MedicalRecord>> GetFilteredRecordsAsync(DateTime? startDate, DateTime? endDate, string? search);
    }

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
        public async Task<IEnumerable<MedicalRecord>> GetFilteredRecordsAsync(DateTime? startDate, DateTime? endDate, string? patientName)
        {
            var query = _context.MedicalRecords
                .Include(m => m.Patient)
                .Include(m => m.Doctor)
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
    }
}
