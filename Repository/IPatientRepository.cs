namespace Hospital_Administration_System.Repository;

public interface IPatientRepository: IRepository<Patient>
{
    public Task<IEnumerable<Patient>> GetAllPatientsAsync();
    public Task<Patient> GetPatientByIdAsync(int Id);
    public Task UpdatePatientAsync(Patient patient);
    public Task DeletePatientAsync(Patient patient);
}
