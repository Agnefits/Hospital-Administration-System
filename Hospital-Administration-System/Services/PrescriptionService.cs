using Hospital_Administration_System.Models;
using Hospital_Administration_System.Repository;
using Hospital_Administration_System.ViewModels.Doctor;
using Microsoft.EntityFrameworkCore;

namespace Hospital_Administration_System.Services
{
    public class PrescriptionService : GenericRepository<Prescription>, IPrescriptionRepository
    {
        private readonly ApplicationDbContext _context;
        public PrescriptionService(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<DoctorResponseVM> AddAsync(PrescriptionCreateVM viewModel, int doctorId)
        {
            var doctor = await _context.Doctors.FindAsync(doctorId);
            if (doctor == null)
                return new DoctorResponseVM
                {
                    Succeeded = false,
                    Message = "Doctor not found"
                };

            var patient = await _context.Patients.FindAsync(viewModel.PatientID);
            if (patient == null)
                return new DoctorResponseVM
                {
                    Succeeded = false,
                    Message = "Patient not found"
                };

            try
            {
                var prescription = new Prescription
                {
                    PatientID = viewModel.PatientID,
                    DoctorID = doctorId,
                    MedicationName = viewModel.MedicationName,
                    Instructions = viewModel.Instructions,
                    Dosage = viewModel.Dosage,
                    IssuedDate = DateTime.Now,
                    AdditionalData = viewModel.AdditionalData
                };

                await _context.Prescriptions.AddAsync(prescription);

                return new DoctorResponseVM
                {
                    Succeeded = true,
                    Message = "Prescription added successfully",
                    Prescription = prescription
                };
            }
            catch (Exception ex)
            {
                return new DoctorResponseVM { Succeeded = false, Error = ex.Message };
            }
        }

        public async Task<DoctorResponseVM> UpdateAsync(PrescriptionEditVM viewModel)
        {
            var patient = await _context.Patients.FindAsync(viewModel.PatientID);
            if (patient == null)
                return new DoctorResponseVM
                {
                    Succeeded = false,
                    Message = "Patient not found"
                };

            var prescription = await _context.Prescriptions.FindAsync(viewModel.PrescriptionID);
            if (prescription == null)
                return new DoctorResponseVM
                {
                    Succeeded = false,
                    Message = "Prescription not found"
                };

            prescription.PatientID = viewModel.PatientID;
            prescription.MedicationName = viewModel.MedicationName;
            prescription.Instructions = viewModel.Instructions;
            prescription.Dosage = viewModel.Dosage;
            prescription.AdditionalData = viewModel.AdditionalData;
            _context.Prescriptions.Update(prescription);

            return new DoctorResponseVM
            {
                Succeeded = true,
                Message = "Prescription edited successfully",
                Prescription = prescription
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var prescription = await _context.Prescriptions.FindAsync(id);
            if (prescription == null)
                return false;

            _context.Prescriptions.Remove(prescription);
            return true;
        }
    }
}
