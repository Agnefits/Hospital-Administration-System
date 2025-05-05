
namespace Hospital_Administration_System.Repository;

public interface IDoctorRepository: IRepository<Doctor>
{
    public Task<IEnumerable<Doctor>> GetDoctorsAsync();
    public Task<Doctor> GetDoctorByIdAsync(int id);
    public Task<IEnumerable<Reservation>> GetDoctorReservationsAsync(int doctorId);
    public Task<IEnumerable<Reservation>> GetDepartmentReservationsAsync(int departmentId);
    public Task<IEnumerable<Patient>> GetDepartmentPatientsAsync(int departmentId);
    public Task<IEnumerable<Prescription>> GetDepartmentPrescriptionsAsync(int departmentId);
    public Task<IEnumerable<MedicalRecord>> GetDepartmentMedicalsAsync(int departmentId, DateTime? startDate, DateTime? endDate, string? search);
}
