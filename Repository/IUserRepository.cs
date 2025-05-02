using Hospital_Administration_System.ViewModels.User;

namespace Hospital_Administration_System.Repository;

public interface IUserRepository: IRepository<User>
{
    Task<IEnumerable<User>> GetAllUsersAsync();
    Task<User?> GetByIdAsync(string userId);
    Task<UserResponseVM> AddAsync(UserCreateVM model);
    Task<UserResponseVM> UpdateAsync(UserEditVM model);
    Task<bool> DeleteAsync(string userId);
}
