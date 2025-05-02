using Hospital_Administration_System.ViewModels.Branch;

namespace Hospital_Administration_System.Services;

public class BranchService : GenericRepository<Branch>, IBranchRepository
{
    private readonly ApplicationDbContext _context;

    public BranchService(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Branch>> GetAllBranchesAsync()
    {
        return await _context.Branches
            .Include(b => b.Departments)
            .Where(b => !b.Deleted)
            .ToListAsync();
    }

    public async Task<BranchResponseVM> AddAsync(BranchCreateVM model)
    {
        if (_context.Branches.Any(b => b.Name.ToLower() == model.Name.ToLower() && !b.Deleted))
            return new BranchResponseVM
            {
                Succeeded = false,
                Error = "Branch with this name already exists",
            };

        var branch = new Branch
        {
            Name = model.Name,
            Location = model.Location,
            ContactNumber = model.ContactNumber,
            AdditionalData = model.AdditionalData
        };
        await _context.Branches.AddAsync(branch);
        await _context.SaveChangesAsync();
        return new BranchResponseVM
        {
            Succeeded = true,
            Message = "Branch created successfully",
            Branch = branch,
        };
    }

    public async Task<BranchResponseVM> UpdateAsync(BranchEditVM model)
    {
        var branch = await _context.Branches.FindAsync(model.BranchID);
        if (branch == null || branch.Deleted)
            return new BranchResponseVM
            {
                Succeeded = false,
                Error = "Branch not found",
            };
        if (_context.Branches.Any(b => b.Name.ToLower() == model.Name.ToLower() && b.BranchID != model.BranchID && !b.Deleted))
            return new BranchResponseVM
            {
                Succeeded = false,
                Error = "Branch with this name already exists",
            };
        branch.Name = model.Name;
        branch.Location = model.Location;
        branch.ContactNumber = model.ContactNumber;
        branch.AdditionalData = model.AdditionalData;
        _context.Branches.Update(branch);
        await _context.SaveChangesAsync();
        return new BranchResponseVM
        {
            Succeeded = true,
            Message = "Branch updated successfully",
            Branch = branch,
        };
    }

    public async Task<bool> DeleteAsync(int branchId)
    {
        var branch = await _context.Branches.FindAsync(branchId);
        if (branch == null || branch.Deleted)
            return false;
        branch.Deleted = true;
        _context.Branches.Update(branch);
        await _context.SaveChangesAsync();
        return true;
    }
}
