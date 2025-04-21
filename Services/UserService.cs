
namespace Hospital_Administration_System.Services;

public class UserService: GenericRepository<User>, IUserRepository
{
    public UserService(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        return await GetAllAsync();
    }

    public async Task<User> GetUserByIdAsync(int id)
    {
        return await GetByIdAsync(id);
    }

    public async Task AddUserAsync(User user)
    {

        await AddAsync(user);
    }

    public async Task UpdateUserAsync(User user)
    {
        await UpdateAsync(user);
    }

    public async Task DeleteUserAsync(User user)
    {
        await DeleteAsync(user);
    }
}
