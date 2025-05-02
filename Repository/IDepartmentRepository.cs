using Hospital_Administration_System.ViewModels.Department;

namespace Hospital_Administration_System.Repository;

public interface IDepartmentRepository : IRepository<Department>
{
    Task<IEnumerable<Department>> GetAllDepartmentsAsync();
    Task<DepartmentResponseVM> AddAsync(DepartmentCreateVM model);
    Task<DepartmentResponseVM> UpdateAsync(DepartmentEditVM model);
    Task<bool> DeleteAsync(int DepartmentId);
}
