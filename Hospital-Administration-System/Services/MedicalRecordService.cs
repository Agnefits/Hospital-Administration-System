using Hospital_Administration_System.ViewModels.Doctor;

namespace Hospital_Administration_System.Services
{
    public class MedicalRecordService : GenericRepository<MedicalRecord>, IMedicalRecordRepository
    {
        private readonly ApplicationDbContext _context;

        public MedicalRecordService(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<DoctorResponseVM> AddAsync(MedicalRecordCreateVM viewModel, int doctorId)
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
                var medicalRecord = new MedicalRecord
                {
                    PatientID = viewModel.PatientID,
                    DoctorID = doctorId,
                    Diagnosis = viewModel.Diagnosis,
                    Treatment = viewModel.Treatment,
                    CreatedAt = DateTime.Now,
                    AdditionalData = viewModel.AdditionalData
                };

                await _context.MedicalRecords.AddAsync(medicalRecord);

                return new DoctorResponseVM
                {
                    Succeeded = true,
                    Message = "Medical record added successfully",
                    MedicalRecord = medicalRecord
                };
            }
            catch (Exception ex)
            {
                return new DoctorResponseVM { Succeeded = false, Error = ex.Message };
            }
        }

        public async Task<DoctorResponseVM> UpdateAsync(MedicalRecordEditVM viewModel)
        {
            var patient = await _context.Patients.FindAsync(viewModel.PatientID);
            if (patient == null)
                return new DoctorResponseVM
                {
                    Succeeded = false,
                    Message = "Patient not found"
                };

            var medicalRecord = await _context.MedicalRecords.FindAsync(viewModel.RecordID);
            if (medicalRecord == null)
                return new DoctorResponseVM
                {
                    Succeeded = false,
                    Message = "Medical record not found"
                };

            medicalRecord.PatientID = viewModel.PatientID;
            medicalRecord.Diagnosis = viewModel.Diagnosis;
            medicalRecord.Treatment = viewModel.Treatment;
            medicalRecord.AdditionalData = viewModel.AdditionalData;
            _context.MedicalRecords.Update(medicalRecord);

            return new DoctorResponseVM
            {
                Succeeded = true,
                Message = "Medical record edited successfully",
                MedicalRecord = medicalRecord
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var medicalRecord = await _context.MedicalRecords.FindAsync(id);
            if (medicalRecord == null)
                return false;

            _context.MedicalRecords.Remove(medicalRecord);
            return true;
        }
    }
}
