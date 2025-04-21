

namespace Hospital_Administration_System.Services;

public class DoctorService: GenericRepository<Doctor>, IDoctorRepository
{

    public DoctorService(ApplicationDbContext context) :base(context)
    {
           
    }

    public async Task<Doctor> GetDoctorByIdAsync(int id)
    {
        return await GetByIdAsync(id);
    }

    public async Task<IEnumerable<Reservation>?> GetDoctorReservationsAsync(int doctorId)
    {
        var doc = await _context.Doctors.Include(r => r.Reservations).ThenInclude(x=>x.Patient).FirstOrDefaultAsync(r => r.DoctorID == doctorId);
        var rev = doc?.Reservations.ToList();
        return rev;
    }

    public async Task<IEnumerable<Doctor>> GetDoctorsAsync()
    {
        return await GetAllAsync();
    }
}

