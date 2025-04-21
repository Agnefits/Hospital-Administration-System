
namespace Hospital_Administration_System.Repository;

public interface INurseRepository: IRepository<Nurse>
{

    public Task<IEnumerable<Nurse>> GetAllNursesAsync();
    public Task<Nurse> GetNurseByIdAsync(int id);
    public Task AddNurseAsync(Nurse nurse);
    public Task UpdateNurseAsync(Nurse nurse);
    public Task DeleteNurseAsync(Nurse nurse);
    public Task<IEnumerable<Reservation>> GetNursesByDepartmentAsync(int Id);

}
