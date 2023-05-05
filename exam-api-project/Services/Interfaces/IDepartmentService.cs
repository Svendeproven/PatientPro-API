using exam_api_project.models.Dtos;
using exam_api_project.Utilities;

namespace exam_api_project.Services.Interfaces;

public interface IDepartmentService
{
    // CRUD operations for Department
    public Task<DepartmentReadDto> CreateDepartmentAsync(DepartmentWriteDto department);
    public Task<DepartmentReadDto> GetDepartmentByIdAsync(int id);
    public Task<IEnumerable<DepartmentReadDto>> GetAllDepartmentsAsync(List<Filter> filters = null);
    public Task<DepartmentReadDto> UpdateDepartmentByIdAsync(DepartmentWriteDto department, int id);
    public Task<DepartmentReadDto> DeleteDepartmentByIdAsync(int id);
}