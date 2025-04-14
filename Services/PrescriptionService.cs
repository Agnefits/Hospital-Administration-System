using Hospital_Administration_System.Models;
using Hospital_Administration_System.Repository;

namespace Hospital_Administration_System.Services
{
    public class PrescriptionService
    {
        private readonly IRepository<Prescription> _prescriptionRepository;

        public PrescriptionService(IRepository<Prescription> prescriptionRepository)
        {
            _prescriptionRepository = prescriptionRepository;
        }

        public async Task<IEnumerable<Prescription>> GetAllPrescriptionsAsync()
        {
            return await _prescriptionRepository.GetAllAsync();
        }

        public async Task<Prescription> GetPrescriptionByIdAsync(int id)
        {
            return await _prescriptionRepository.GetByIdAsync(id);
        }

        public async Task AddPrescriptionAsync(Prescription prescription)
        {
            await _prescriptionRepository.AddAsync(prescription);
        }

        public async Task UpdatePrescriptionAsync(Prescription prescription)
        {
            await _prescriptionRepository.UpdateAsync(prescription);
        }

        public async Task DeletePrescriptionAsync(Prescription prescription)
        {
            await _prescriptionRepository.DeleteAsync(prescription);
        }
    }
}
