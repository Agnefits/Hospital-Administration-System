namespace Hospital_Administration_System.Repository;

public interface IPharmacistRepository: IRepository<Pharmacist>
{
    Task<IEnumerable<Pharmacist>> GetPharmacistsAsync();
    Task<Pharmacist> GetPharmacistAsync(int id);
    Task AddPharmacistAsync(Pharmacist pharmacist);
    Task UpdatePharmacistAsync(Pharmacist pharmacist);
    Task DeletePharmacistAsync(int id);
    Task<IEnumerable<Pharmacy>> GetAllPharmaciesAsync();
}
