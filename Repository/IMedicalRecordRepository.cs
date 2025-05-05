using Hospital_Administration_System.ViewModels.Doctor;

namespace Hospital_Administration_System.Repository;

public interface IMedicalRecordRepository : IRepository<MedicalRecord>
{
    Task<DoctorResponseVM> AddAsync(MedicalRecordCreateVM viewModel, int doctorId);
    Task<DoctorResponseVM> UpdateAsync(MedicalRecordEditVM viewModel);
    Task<bool> DeleteAsync(int id);
}

