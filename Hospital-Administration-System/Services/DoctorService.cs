using Hospital_Administration_System.Models;
using Hospital_Administration_System.ViewModels.Department;
using Hospital_Administration_System.ViewModels.Doctor;

namespace Hospital_Administration_System.Services;

public class DoctorService : GenericRepository<Doctor>, IDoctorRepository
{
    public DoctorService(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Doctor> GetDoctorByIdAsync(int id)
    {
        return await GetByIdAsync(id);
    }

    public async Task<IEnumerable<Reservation>> GetDoctorReservationsAsync(int doctorId)
    {
        return await _context.Reservations
            .Include(r => r.Patient)
                .ThenInclude(p => p.Analyses)
            .Include(r => r.Patient)
                .ThenInclude(p => p.MedicalRecords)
            .Include(r => r.Patient)
                .ThenInclude(p => p.Prescriptions)
            .Where(r => r.DoctorID == doctorId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Reservation>> GetDepartmentReservationsAsync(int departmentId)
    {
        return await _context.Reservations
            .Include(r => r.Doctor)
            .Include(r => r.Patient)
                .ThenInclude(p => p.Analyses)
            .Include(r => r.Patient)
                .ThenInclude(p => p.MedicalRecords)
            .Include(r => r.Patient)
                .ThenInclude(p => p.Prescriptions)
            .Where(r => r.Doctor.DepartmentID == departmentId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Patient>> GetDepartmentPatientsAsync(int departmentId)
    {
        return await _context.Patients
                .Include(p => p.Analyses)
                .Include(p => p.MedicalRecords)
                .Include(p => p.Prescriptions)
            .Where(p => p.Reservations.Any(r => r.Doctor.DepartmentID == departmentId))
        .ToListAsync();
    }

    public async Task<IEnumerable<Prescription>> GetDepartmentPrescriptionsAsync(int departmentId)
    {
        return await _context.Prescriptions
                .Include(p => p.Doctor)
                .Include(p => p.Patient)
            .Where(p => p.Doctor.DepartmentID == departmentId)
            .ToListAsync();
    }

    public async Task<IEnumerable<MedicalRecord>> GetDepartmentMedicalsAsync(int departmentId, DateTime? startDate, DateTime? endDate, string? patientName)
    {
        var query = _context.MedicalRecords
            .Include(m => m.Patient)
            .Include(m => m.Doctor)
            .Where(p => p.Doctor.DepartmentID == departmentId)
            .AsQueryable(); // Start with a queryable collection

        // Apply filtering for start date, end date, and patient name directly on the database query
        if (startDate.HasValue)
        {
            query = query.Where(r => r.CreatedAt >= startDate.Value);
        }

        if (endDate.HasValue)
        {
            query = query.Where(r => r.CreatedAt <= endDate.Value);
        }

        if (!string.IsNullOrEmpty(patientName))
        {
            query = query.Where(r => r.Patient.FullName.Contains(patientName));
        }

        // Execute the query asynchronously
        var records = await query.OrderByDescending(m => m.CreatedAt).ToListAsync();

        return records;
    }

    public async Task<IEnumerable<Doctor>> GetDoctorsAsync()
    {
        return await _context.Doctors
        .Include(d => d.User)  // If FullName comes from User
        .Where(d => !d.Deleted)
        .Select(d => new Doctor
        {
            DoctorID = d.DoctorID,
            FullName = d.FullName // Or d.FullName if directly on Doctor
        })
        .ToListAsync();
        
    }

    public async Task<DoctorResponseVM> UpdateReservationStatusAsync(ReservationEditStatusVM reservationEditStatusVM)
    {
        var reservation = await _context.Reservations
            .Include(r => r.Patient)
            .Include(r => r.Doctor)
            .FirstOrDefaultAsync(r => r.ReservationID == reservationEditStatusVM.ReservationID);

        if (reservation == null)
        {
            return new DoctorResponseVM { Succeeded = false, Message = "Reservation not found." };
        }
        reservation.Status = reservationEditStatusVM.ReservationStatus;
        await _context.SaveChangesAsync();
        return new DoctorResponseVM { Succeeded = true, Message = "Reservation status updated successfully.", Reservation = reservation };
    }
    //public async Task<DoctorResponseVM> RedirectReservationAsync(ReservationRedirectionVM reservationRedirectionVM)
    //{
    //    var reservation = await _context.Reservations
    //        .Include(r => r.Patient)
    //        .FirstOrDefaultAsync(r => r.ReservationID == reservationRedirectionVM.OldReservationID);

    //    if (reservation == null)
    //    {
    //        return new DoctorResponseVM { Succeeded = false, Message = "Old reservation not found." };
    //    }
    //    try
    //    {
    //        var newReservation = new Reservation
    //        {
    //            PatientID = reservation.PatientID,
    //            DoctorID = reservationRedirectionVM.DoctorID,
    //            ReservationDate = reservation.ReservationDate,
    //            Status = ReservationStatus.Pending,
    //            AdditionalData = reservationRedirectionVM.AdditionalData
    //        };

    //        await _context.Reservations.AddAsync(newReservation);
    //        await _context.SaveChangesAsync();

    //        return new DoctorResponseVM
    //        {
    //            Succeeded = true,
    //            Message = "Department updated successfully",
    //            Reservation = newReservation,
    //        };
    //    }
    //    catch(Exception ex)
    //    {
    //        return new DoctorResponseVM
    //        {
    //            Succeeded = false,
    //            Message = ex.Message,
    //        };
    //    }
    //}
    public async Task<DoctorResponseVM> RedirectReservationAsync(ReservationRedirectionVM reservationRedirectionVM)
    {
        var reservation = await _context.Reservations
            .Include(r => r.Patient)
            .FirstOrDefaultAsync(r => r.ReservationID == reservationRedirectionVM.OldReservationID);

        if (reservation == null)
        {
            return new DoctorResponseVM { Succeeded = false, Error = "Old reservation not found." };
        }

        if (!await _context.Doctors.AnyAsync(d => d.DoctorID == reservationRedirectionVM.DoctorID))
        {
            return new DoctorResponseVM { Succeeded = false, Error = "Selected doctor not found." };
        }

        try
        {
            var newReservation = new Reservation
            {
                PatientID = reservation.PatientID,
                DoctorID = reservationRedirectionVM.DoctorID,
                ReservationDate = reservation.ReservationDate,
                Status = ReservationStatus.Pending,
                AdditionalData = reservationRedirectionVM.AdditionalData
            };

            await _context.Reservations.AddAsync(newReservation);
            reservation.Status = ReservationStatus.Cancelled;
            _context.Reservations.Update(reservation);
            await _context.SaveChangesAsync();

            return new DoctorResponseVM
            {
                Succeeded = true,
                Message = "Reservation redirected successfully",
                Reservation = newReservation
            };
        }
        catch (Exception ex)
        {
            //_logger.LogError(ex, "Error redirecting reservation {ReservationID}", reservationRedirectionVM.OldReservationID);
            return new DoctorResponseVM
            {
                Succeeded = false,
                Error = ex.Message ?? "An error occurred while redirecting the reservation."
            };
        }
    }
}

