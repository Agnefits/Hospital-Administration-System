

namespace Hospital_Administration_System.Services;

public class PharmacistService: GenericRepository<Pharmacist>, IPharmacistRepository
{
    private readonly ApplicationDbContext _context;

    public PharmacistService(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task AddPharmacistAsync(Pharmacist pharmacist)
    {
        await AddAsync(pharmacist);
    }

    public async Task DeletePharmacistAsync(int id)
    {
        var pharmacist = await GetPharmacistAsync(id);
        await DeleteAsync(pharmacist);
    }

    public async Task<Pharmacist> GetPharmacistAsync(int id)
    {
        return await GetByIdAsync(id);
    }

    public async Task<IEnumerable<Pharmacist>> GetPharmacistsAsync()
    {
        return await GetAllAsync();
    }


    public async Task UpdatePharmacistAsync(Pharmacist pharmacist)
    {
        await UpdateAsync(pharmacist);
    }

    // move to PharmacyService.cs
    public async Task<IEnumerable<Pharmacy>> GetAllPharmaciesAsync()
    {
        return await _context.Pharmacies
            .Where(p => !p.Deleted)
            .ToListAsync();
    }
}
