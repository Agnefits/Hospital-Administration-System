using Hospital_Administration_System.ViewModels.Doctor;

namespace Hospital_Administration_System.Repository
{
    public interface IPrescriptionRepository : IRepository<Prescription>
    {
        Task<DoctorResponseVM> AddAsync(PrescriptionCreateVM viewModel, int doctorId);
        Task<DoctorResponseVM> UpdateAsync(PrescriptionEditVM viewModel);
        Task<bool> DeleteAsync(int id);
    }
}
