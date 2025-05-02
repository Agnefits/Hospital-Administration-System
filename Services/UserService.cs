
using Hospital_Administration_System.Models;
using Hospital_Administration_System.ViewModels.User;

namespace Hospital_Administration_System.Services;

public class UserService : GenericRepository<User>, IUserRepository
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public UserService(ApplicationDbContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager) : base(context)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        return await _context.Users
            .Include(u => u.Admin)
            .Include(u => u.Patient)
            .Include(u => u.Doctor).ThenInclude(d => d.Department)
            .Include(u => u.Nurse).ThenInclude(n => n.Department)
            .Include(u => u.Pharmacist).ThenInclude(p => p.Pharmacy)
            .Where(u => !u.Deleted)
            .ToListAsync();
    }

    public async Task<User?> GetByIdAsync(string userId)
    {
        var user = await _context.Users
            .Include(u => u.Admin)
            .Include(u => u.Patient)
            .Include(u => u.Doctor).ThenInclude(d => d.Department)
            .Include(u => u.Nurse).ThenInclude(n => n.Department)
            .Include(u => u.Pharmacist).ThenInclude(p => p.Pharmacy)
            .FirstOrDefaultAsync(u => u.Id == userId && !u.Deleted);

        return user;
    }

    public async Task<UserResponseVM> AddAsync(UserCreateVM model)
    {
        if (await _context.Users.AnyAsync(u => u.Email != null && u.Email.ToLower() == model.Email.ToLower() && !u.Deleted))
            return new UserResponseVM { Succeeded = false, Error = "Email is already in use." };

        try
        {
            var user = new User
            {
                UserName = model.Email,
                Email = model.Email,
                AdditionalData = model.AdditionalData,
            }; 
            
            var result = await _userManager.CreateAsync(user, model.Password); // Create the user with a password
            if (!result.Succeeded)
                return new UserResponseVM { Succeeded = false, Error = string.Join(", ", result.Errors.Select(e => e.Description)) };
            
            // Ensure the role exists in the database
            var role = model.Role.ToLower();
            if (!await _roleManager.RoleExistsAsync(role))
            {
                // You can create the role if it doesn't exist
                await _roleManager.CreateAsync(new IdentityRole(role));
            }

            // Assign the user to the specified role
            await _userManager.AddToRoleAsync(user, role);

            switch (model.Role.ToLower())
            {
                case "admin":
                    _context.Admins.Add(new Admin
                    {
                        UserID = user.Id,
                        FullName = model.FullName!,
                        ContactNumber = model.ContactNumber!,
                        AdditionalData = model.AdditionalData
                    });
                    break;
                case "doctor":
                    if (!await _context.Departments.AnyAsync(d => d.DepartmentID == model.DepartmentID))
                        return new UserResponseVM { Succeeded = false, Error = "Invalid Department ID." };

                    _context.Doctors.Add(new Doctor
                    {
                        UserID = user.Id,
                        FullName = model.FullName!,
                        ContactNumber = model.ContactNumber!,
                        Specialization = model.Specialization!,
                        DepartmentID = model.DepartmentID!.Value,
                        AdditionalData = model.AdditionalData
                    });
                    break;
                case "nurse":
                    if (!await _context.Departments.AnyAsync(d => d.DepartmentID == model.DepartmentID))
                        return new UserResponseVM { Succeeded = false, Error = "Invalid Department ID." };

                    _context.Nurses.Add(new Nurse
                    {
                        UserID = user.Id,
                        FullName = model.FullName!,
                        ContactNumber = model.ContactNumber!,
                        DepartmentID = model.DepartmentID!.Value,
                        AdditionalData = model.AdditionalData
                    });
                    break;
                case "pharmacist":
                    if (!await _context.Pharmacies.AnyAsync(p => p.PharmacyID == model.PharmacyID))
                        return new UserResponseVM { Succeeded = false, Error = "Invalid Pharmacy ID." };

                    _context.Pharmacists.Add(new Pharmacist
                    {
                        UserID = user.Id,
                        FullName = model.FullName!,
                        ContactNumber = model.ContactNumber!,
                        PharmacyID = model.PharmacyID!.Value,
                        AdditionalData = model.AdditionalData
                    });
                    break;
            }

            await _context.SaveChangesAsync();

            return new UserResponseVM
            {
                Succeeded = true,
                User = user,
                Message = "User created successfully."
            };
        }
        catch (Exception ex)
        {
            return new UserResponseVM { Succeeded = false, Error = ex.Message };
        }
    }

    public async Task<UserResponseVM> UpdateAsync(UserEditVM model)
    {
        try
        {
            var user = await _context.Users
                .Include(u => u.Admin)
                .Include(u => u.Doctor)
                .Include(u => u.Nurse)
                .Include(u => u.Pharmacist)
                .FirstOrDefaultAsync(u => u.Id == model.UserId);

            if (user == null)
                return new UserResponseVM { Succeeded = false, Error = "User not found." };

            if (await _context.Users.AnyAsync(u => u.Email != null && u.Email.ToLower() == model.Email.ToLower() && u.Id != model.UserId && !u.Deleted))
                return new UserResponseVM { Succeeded = false, Error = "Email is already in use by another user." };

            user.Email = model.Email;
            user.UserName = model.Email;
            user.AdditionalData = model.AdditionalData;
            // Handle the user's role update
            var currentRoles = await _userManager.GetRolesAsync(user);
            var newRole = model.Role.ToLower(); // assuming the role is part of the model

            if (!currentRoles.Contains(newRole)) // Check if role needs to be changed
            {
                // Remove the old role
                foreach (var role in currentRoles)
                {
                    await _userManager.RemoveFromRoleAsync(user, role);
                }

                // Add the new role
                await _userManager.AddToRoleAsync(user, newRole);
            }

            // Handle specific role updates
            switch (newRole)
            {
                case "admin":
                    if (user.Admin != null)
                    {
                        user.Admin.FullName = model.FullName!;
                        user.Admin.ContactNumber = model.ContactNumber!;
                        user.Admin.AdditionalData = model.AdditionalData;
                    }
                    else
                    {
                        // Create a new Admin entry if it does not exist
                        _context.Admins.Add(new Admin
                        {
                            UserID = user.Id,
                            FullName = model.FullName!,
                            ContactNumber = model.ContactNumber!,
                            AdditionalData = model.AdditionalData
                        });
                    }
                    break;
                case "doctor":
                    if (!await _context.Departments.AnyAsync(d => d.DepartmentID == model.DepartmentID))
                        return new UserResponseVM { Succeeded = false, Error = "Invalid Department ID." };

                    if (user.Doctor != null)
                    {
                        user.Doctor.FullName = model.FullName!;
                        user.Doctor.ContactNumber = model.ContactNumber!;
                        user.Doctor.DepartmentID = model.DepartmentID!.Value;
                        user.Doctor.Specialization = model.Specialization!;
                        user.Doctor.AdditionalData = model.AdditionalData;
                    }
                    else
                    {
                        _context.Doctors.Add(new Doctor
                        {
                            UserID = user.Id,
                            FullName = model.FullName!,
                            ContactNumber = model.ContactNumber!,
                            DepartmentID = model.DepartmentID!.Value,
                            Specialization = model.Specialization!,
                            AdditionalData = model.AdditionalData
                        });
                    }
                    break;
                case "nurse":
                    if (!await _context.Departments.AnyAsync(d => d.DepartmentID == model.DepartmentID))
                        return new UserResponseVM { Succeeded = false, Error = "Invalid Department ID." };

                    if (user.Nurse != null)
                    {
                        user.Nurse.FullName = model.FullName!;
                        user.Nurse.ContactNumber = model.ContactNumber!;
                        user.Nurse.DepartmentID = model.DepartmentID!.Value;
                        user.Nurse.AdditionalData = model.AdditionalData;
                    }
                    else
                    {
                        _context.Nurses.Add(new Nurse
                        {
                            UserID = user.Id,
                            FullName = model.FullName!,
                            ContactNumber = model.ContactNumber!,
                            DepartmentID = model.DepartmentID!.Value,
                            AdditionalData = model.AdditionalData
                        });
                    }
                    break;
                case "pharmacist":
                    if (!await _context.Pharmacies.AnyAsync(p => p.PharmacyID == model.PharmacyID))
                        return new UserResponseVM { Succeeded = false, Error = "Invalid Pharmacy ID." };

                    if (user.Pharmacist != null)
                    {
                        user.Pharmacist.FullName = model.FullName!;
                        user.Pharmacist.ContactNumber = model.ContactNumber!;
                        user.Pharmacist.PharmacyID = model.PharmacyID!.Value;
                        user.Pharmacist.AdditionalData = model.AdditionalData;
                    }
                    else
                    {
                        _context.Pharmacists.Add(new Pharmacist
                        {
                            UserID = user.Id,
                            FullName = model.FullName!,
                            ContactNumber = model.ContactNumber!,
                            PharmacyID = model.PharmacyID!.Value,
                            AdditionalData = model.AdditionalData
                        });
                    }
                    break;
                default:
                    return new UserResponseVM { Succeeded = false, Error = "Invalid role." };
            }

            // Save changes to the database
            await _context.SaveChangesAsync();

            return new UserResponseVM
            {
                Succeeded = true,
                User = user,
                Message = "User updated successfully."
            };
        }
        catch (Exception ex)
        {
            return new UserResponseVM
            {
                Succeeded = false,
                Error = ex.Message
            };
        }
    }

    public async Task<bool> DeleteAsync(string userId)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null) return false;

        user.Deleted = true;

        if (user.Admin != null) user.Admin.Deleted = true;
        if (user.Doctor != null) user.Doctor.Deleted = true;
        if (user.Nurse != null) user.Nurse.Deleted = true;
        if (user.Pharmacist != null) user.Pharmacist.Deleted = true;

        await _context.SaveChangesAsync();
        return true;
    }
}
