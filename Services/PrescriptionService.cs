
namespace Hospital_Administration_System.Services;

public class PrescriptionService: GenericRepository<Prescription>, IPrescriptionRepository
{

    public PrescriptionService(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Prescription>> GetAllPrescriptionsAsync()
    {
        return await GetAllAsync();
    }

    public async Task<Prescription> GetPrescriptionByIdAsync(int id)
    {
        return await GetByIdAsync(id);
    }

    public async Task AddPrescriptionAsync(Prescription prescription)
    {
        await AddAsync(prescription);
    }

    public async Task UpdatePrescriptionAsync(Prescription prescription)
    {
        await UpdateAsync(prescription);
    }

    public async Task DeletePrescriptionAsync(Prescription prescription)
    {
        await DeleteAsync(prescription);
    }
}
