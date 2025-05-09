

namespace Hospital_Administration_System.Services;

public class NurseService: GenericRepository<Nurse>, INurseRepository
{

    public NurseService(ApplicationDbContext context): base(context)
    {

    }

    public async Task AddNurseAsync(Nurse nurse)
    {
        await AddAsync(nurse);
    }

    public async Task DeleteNurseAsync(Nurse nurse)
    {
        await DeleteAsync(nurse);
    }

    public async Task<IEnumerable<Nurse>> GetAllNursesAsync()
    {
        return await GetAllAsync();
    }

    public async Task<Nurse> GetNurseByIdAsync(int id)
    {
        return await GetByIdAsync(id);
    }
    public Task<IEnumerable<Reservation>> GetNursesByDepartmentAsync(int Id)
    {
        throw new NotImplementedException();
    }


    public async Task UpdateNurseAsync(Nurse nurse)
    {
        await UpdateAsync(nurse);
    }

    public async Task<Nurse?> GetByUserIdAsync(string userId)
    {
        return await _context.Nurses
            .FirstOrDefaultAsync(n => n.UserID == userId);
    }

}

