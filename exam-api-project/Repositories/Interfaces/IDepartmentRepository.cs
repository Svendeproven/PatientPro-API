using exam_api_project.models.Entities;
using exam_api_project.Utilities;

namespace exam_api_project.Repositories.Interfaces;

public interface IDepartmentRepository
{
    // CRUD Operation for Department
    public Task<DepartmentModel> CreateDepartmentAsync(DepartmentModel departmentWriteDto);
    public Task<IEnumerable<DepartmentModel>> GetAllDepartmentsAsync(List<Filter> filters = null);
    public Task<DepartmentModel> GetDepartmentByIdAsync(int id);
    public Task<DepartmentModel> UpdateDepartmentByIdAsync(int id, DepartmentModel departmentWriteDto);
    public Task<DepartmentModel> DeleteDepartmentByIdAsync(int id);
}