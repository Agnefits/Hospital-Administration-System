using Hospital_Administration_System.ViewModels.Branch;

namespace Hospital_Administration_System.Repository;

public interface IBranchRepository: IRepository<Branch>
{
    Task<IEnumerable<Branch>> GetAllBranchesAsync();
    Task<BranchResponseVM> AddAsync(BranchCreateVM model);
    Task<BranchResponseVM> UpdateAsync(BranchEditVM model);
    Task<bool> DeleteAsync(int branchId);
}
