namespace Hospital_Administration_System.Repository;

public interface IMedicalRecordRepository : IRepository<MedicalRecord>
{
    Task<IEnumerable<MedicalRecord>> GetAllWithDetailsAsync();
    Task<MedicalRecord?> GetWithDetailsByIdAsync(int id);
}

