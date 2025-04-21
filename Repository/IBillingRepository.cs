namespace Hospital_Administration_System.Repository;

public interface IBillingRepository: IRepository<Billing>
{
    public Task<IEnumerable<Billing>> GetAllBillingsAsync();
    public Task<Billing> GetBillingByIdAsync(int id);
    public Task AddBillingAsync(Billing billing);
    public Task UpdateBillingAsync(Billing billing);
    public Task DeleteBillingAsync(Billing billing);
}
