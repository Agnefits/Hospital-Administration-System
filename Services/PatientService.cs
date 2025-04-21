
namespace Hospital_Administration_System.Services;

public class PatientService:GenericRepository<Patient>, IPatientRepository
{


    public PatientService(ApplicationDbContext context): base(context)
    {
    }

    public async Task<IEnumerable<Patient>> GetAllPatientsAsync()
    {
        return await GetAllAsync();
    }

    public async Task<Patient> GetPatientByIdAsync(int id)
    {
        return await GetByIdAsync(id);
    }

    public async Task AddPatientAsync(Patient patient)
    {
        await AddAsync(patient);
    }

    public async Task UpdatePatientAsync(Patient patient)
    {
        await UpdateAsync(patient);
    }

    public async Task DeletePatientAsync(Patient patient)
    {
        await DeleteAsync(patient);
    }
}
