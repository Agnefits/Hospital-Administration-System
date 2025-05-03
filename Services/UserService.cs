

namespace Hospital_Administration_System.Services;

public class UserService: GenericRepository<User>, IUserRepository
{
    private RoleManager<IdentityRole> roleManager;

    public UserService(ApplicationDbContext context, UserManager<User> userManager) : base(context)
    {
    }

    public UserService(ApplicationDbContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager) : this(context, userManager)
    {
        this.roleManager = roleManager;
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
