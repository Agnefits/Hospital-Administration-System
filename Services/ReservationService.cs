
namespace Hospital_Administration_System.Services;

public class ReservationService: GenericRepository<Reservation>, IReservationRepository
{
    public ReservationService(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Reservation>> GetAllReservationsAsync()
    {
        return await GetAllAsync();
    }

    public async Task<Reservation> GetReservationByIdAsync(int id)
    {
        return await GetByIdAsync(id);
    }

    public async Task AddReservationAsync(Reservation reservation)
    {
        await AddAsync(reservation);
    }

    public async Task UpdateReservationAsync(Reservation reservation)
    {
        await UpdateAsync(reservation);
    }

    public async Task DeleteReservationAsync(Reservation reservation)
    {
        await DeleteAsync(reservation);
    }
}
