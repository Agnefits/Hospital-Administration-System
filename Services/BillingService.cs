
namespace Hospital_Administration_System.Services;

public class BillingService: GenericRepository<Billing>, IBillingRepository
{
    public BillingService(ApplicationDbContext context):base(context)
    {
        
    }

    public async Task<IEnumerable<Billing>> GetAllBillingsAsync()
    {
        return await _context.Billings
            .Include(b=>b.Patient)
            .ToListAsync();
    }

    public async Task<Billing> GetBillingByIdAsync(int id)
    {
        return await GetByIdAsync(id);
    }

    public async Task AddBillingAsync(Billing billing)
    {
        await AddAsync(billing);
    }

    public async Task UpdateBillingAsync(Billing billing)
    {
        await UpdateAsync(billing);
    }   

    public async Task DeleteBillingAsync(Billing billing)
    {
        await DeleteAsync(billing);
    }

}
