using Hospital_Administration_System.ViewModels.Department;

namespace Hospital_Administration_System.Services;

public class DepartmentService : GenericRepository<Department>, IDepartmentRepository
{
    private readonly ApplicationDbContext _context;

    public DepartmentService(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Department>> GetAllDepartmentsAsync()
    {
        return await _context.Departments
            .Include(d => d.Branch)
            .Where(d => !d.Deleted)
            .ToListAsync();
    }

    public async Task<DepartmentResponseVM> AddAsync(DepartmentCreateVM model)
    {
        if (_context.Departments.Any(b => b.Name.ToLower() == model.Name.ToLower() && model.BranchID == b.BranchID && !b.Deleted))
            return new DepartmentResponseVM
            {
                Succeeded = false,
                Error = "Department with this name already exists in that Branch",
            };

        if (_context.Branches.Any(b => b.BranchID == model.BranchID && b.Deleted))
            return new DepartmentResponseVM
            {
                Succeeded = false,
                Error = "Branch not found",
            };

        if (model.HeadDoctorID != null && _context.Users.Any(u => u.Id == model.HeadDoctorID && u.Deleted))
            return new DepartmentResponseVM
            {
                Succeeded = false,
                Error = "Head Doctor not found",
            };

        var department = new Department
        {
            Name = model.Name,
            BranchID = model.BranchID,
            HeadDoctorID = model.HeadDoctorID,
            AdditionalData = model.AdditionalData
        };
        await _context.Departments.AddAsync(department);
        await _context.SaveChangesAsync();
        return new DepartmentResponseVM
        {
            Succeeded = true,
            Message = "Department created successfully",
            Department = department,
        };
    }

    public async Task<DepartmentResponseVM> UpdateAsync(DepartmentEditVM model)
    {
        var department = await _context.Departments.FindAsync(model.DepartmentID);
        if (department == null || department.Deleted)
            return new DepartmentResponseVM
            {
                Succeeded = false,
                Error = "Department not found",
            };
        if (_context.Departments.Any(b => b.Name.ToLower() == model.Name.ToLower() && model.BranchID == b.BranchID && b.DepartmentID != model.DepartmentID && !b.Deleted))
            return new DepartmentResponseVM
            {
                Succeeded = false,
                Error = "Department with this name already exists in that Branch",
            };
        if (_context.Branches.Any(b => b.BranchID == model.BranchID && b.Deleted))
            return new DepartmentResponseVM
            {
                Succeeded = false,
                Error = "Branch not found",
            };
        if (model.HeadDoctorID != null && _context.Users.Any(u => u.Id == model.HeadDoctorID && u.Deleted))
            return new DepartmentResponseVM
            {
                Succeeded = false,
                Error = "Head Doctor not found",
            };
        department.Name = model.Name;
        department.BranchID = model.BranchID;
        department.HeadDoctorID = model.HeadDoctorID;
        department.AdditionalData = model.AdditionalData;
        _context.Departments.Update(department);
        await _context.SaveChangesAsync();
        return new DepartmentResponseVM
        {
            Succeeded = true,
            Message = "Department updated successfully",
            Department = department,
        };
    }

    public async Task<bool> DeleteAsync(int departmentId)
    {
        var department = await _context.Departments.FindAsync(departmentId);
        if (department == null || department.Deleted)
            return false;
        department.Deleted = true;
        _context.Departments.Update(department);
        await _context.SaveChangesAsync();
        return true;
    }
}
