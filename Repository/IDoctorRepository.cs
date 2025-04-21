
namespace Hospital_Administration_System.Repository;

public interface IDoctorRepository: IRepository<Doctor>
{
    public Task<IEnumerable<Doctor>> GetDoctorsAsync();
    public Task<Doctor> GetDoctorByIdAsync(int id);
    public Task<IEnumerable<Reservation>> GetDoctorReservationsAsync(int doctorId);
}
