namespace Hospital_Administration_System.Repository;

public interface IPrescriptionRepository: IRepository<Prescription>
{
    Task<IEnumerable<Prescription>> GetAllPrescriptionsAsync();
    Task<Prescription> GetPrescriptionByIdAsync(int id);
    Task AddPrescriptionAsync(Prescription prescription);
    Task UpdatePrescriptionAsync(Prescription prescription);
    Task DeletePrescriptionAsync(Prescription prescription);
}
