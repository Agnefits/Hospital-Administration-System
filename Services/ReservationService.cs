
using Hospital_Administration_System.ViewModels.Reservation;
using System.Linq;

namespace Hospital_Administration_System.Services;

public class ReservationService : GenericRepository<Reservation>, IReservationRepository
{
    public ReservationService(ApplicationDbContext context) : base(context)
    {

    }

    public async Task<IEnumerable<Reservation>> GetAllReservationsAsync()
    {
        return await _context.Reservations
            .Include(r => r.Patient)
            .Include(r => r.Doctor)
            .OrderBy(r => r.ReservationDate)
            .ToListAsync();
    }

    public async Task<ReservationResponseVM> AddAsync(ReservationCreateVM model)
    {
        if (!await _context.Patients.AnyAsync(p => p.PatientID == model.PatientID))
        {
            return new ReservationResponseVM
            {
                Succeeded = false,
                Error = "Patient not found"
            };
        }

        if (!await _context.Doctors.AnyAsync(p => p.DoctorID == model.DoctorID))
        {
            return new ReservationResponseVM
            {
                Succeeded = false,
                Error = "Doctor not found"
            };
        }

        Reservation reservation = new Reservation
        {
            PatientID = model.PatientID.Value,
            DoctorID = model.DoctorID,
            ReservationDate = model.ReservationDate,
            AdditionalData = model.AdditionalData,
            Status = ReservationStatus.Pending
        };

        await _context.Reservations.AddAsync(reservation);

        return new ReservationResponseVM
        {
            Succeeded = await _context.SaveChangesAsync() > 0,
            Message = "Reservation created successfully",
            Reservation = reservation
        };
    }

    public async Task<ReservationResponseVM> UpdateAsync(ReservationEditVM model)
    {
        var reservation = await _context.Reservations.FindAsync(model.ReservationID);
        if (reservation == null)
        {
            return new ReservationResponseVM
            {
                Succeeded = false,
                Error = "Reservation not found"
            };
        }

        if (!await _context.Patients.AnyAsync(p => p.PatientID == model.PatientID))
        {
            return new ReservationResponseVM
            {
                Succeeded = false,
                Error = "Patient not found"
            };
        }

        if (!await _context.Doctors.AnyAsync(p => p.DoctorID == model.DoctorID))
        {
            return new ReservationResponseVM
            {
                Succeeded = false,
                Error = "Doctor not found"
            };
        }

        reservation.PatientID = model.PatientID.Value;
        reservation.DoctorID = model.DoctorID;
        reservation.ReservationDate = model.ReservationDate;
        reservation.AdditionalData = model.AdditionalData;

        _context.Reservations.Update(reservation);
        return new ReservationResponseVM
        {
            Succeeded = await _context.SaveChangesAsync() > 0,
            Message = "Reservation updated successfully",
            Reservation = reservation
        };
    }

    public async Task<bool> DeleteAsync(int reservationId)
    {
        var reservation = await _context.Reservations.FindAsync(reservationId);
        if (reservation == null)
        {
            return false;
        }
        _context.Reservations.Remove(reservation);
        return await _context.SaveChangesAsync() > 0;
    }
}
