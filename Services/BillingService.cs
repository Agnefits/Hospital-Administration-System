using Hospital_Administration_System.Models;
using Hospital_Administration_System.Repository;

namespace Hospital_Administration_System.Services
{
    public class BillingService
    {
        private readonly IRepository<Billing> _billingRepository;
        public BillingService(IRepository<Billing> billingRepository)
        {
            _billingRepository = billingRepository;
        }

        public async Task<IEnumerable<Billing>> GetAllBillingsAsync()
        {
            return await _billingRepository.GetAllAsync();
        }

        public async Task<Billing> GetBillingByIdAsync(int id)
        {
            return await _billingRepository.GetByIdAsync(id);
        }

        public async Task AddBillingAsync(Billing billing)
        {
            await _billingRepository.AddAsync(billing);
        }

        public async Task UpdateBillingAsync(Billing billing)
        {
            await _billingRepository.UpdateAsync(billing);
        }

        public async Task DeleteBillingAsync(Billing billing)
        {
            await _billingRepository.DeleteAsync(billing);
        }
    }
}
