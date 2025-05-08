using Hospital_Administration_System.ViewModels.Reservation;

namespace Hospital_Administration_System.Repository;

public interface IReservationRepository: IRepository<Reservation>
{
    public Task<IEnumerable<Reservation>> GetAllReservationsAsync();
    public Task<ReservationResponseVM> AddAsync(ReservationCreateVM reservation);
    public Task<ReservationResponseVM> UpdateAsync(ReservationEditVM reservation);
    public Task<bool> DeleteAsync(int reservationId);
}
