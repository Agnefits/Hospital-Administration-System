using Hospital_Administration_System.Data;
using Hospital_Administration_System.Models;
using Hospital_Administration_System.Repository;
using Microsoft.EntityFrameworkCore;

namespace Hospital_Administration_System.Services
{
    public class PatientService
    {
        private readonly IRepository<Patient> _patientRepository;

        public PatientService(IRepository<Patient> patientRepository)
        {
            _patientRepository = patientRepository;
        }

        public async Task<IEnumerable<Patient>> GetAllPatientsAsync()
        {
            return await _patientRepository.GetAllAsync();
        }

        public async Task<Patient> GetPatientByIdAsync(int id)
        {
            return await _patientRepository.GetByIdAsync(id);
        }

        public async Task AddPatientAsync(Patient patient)
        {
            await _patientRepository.AddAsync(patient);
        }

        public async Task UpdatePatientAsync(Patient patient)
        {
            await _patientRepository.UpdateAsync(patient);
        }

        public async Task DeletePatientAsync(Patient patient)
        {
            await _patientRepository.DeleteAsync(patient);
        }
    }
}
