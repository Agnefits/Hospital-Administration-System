namespace Hospital_Administration_System.Repository;

public interface IReservationRepository: IRepository<Reservation>
{
    public Task<IEnumerable<Reservation>> GetAllReservationsAsync();
    public Task<Reservation> GetReservationByIdAsync(int id);
    public Task UpdateReservationAsync(Reservation reservation);
    public Task DeleteReservationAsync(Reservation reservation);
    public Task AddReservationAsync(Reservation reservation);
}
