using exam_api_project.models.Dtos;
using exam_api_project.models.Entities;
using exam_api_project.Repositories.Interfaces;
using exam_api_project.Services.Interfaces;
using exam_api_project.Utilities;

namespace exam_api_project.Services;

/// <summary>
/// CRUD Service for Department 
/// </summary>
public class DepartmentService :IDepartmentService
{
    private readonly IDepartmentRepository _departmentRepository;
    private readonly IMapper _mapper;

    /// <summary>
    ///     Constructor for dependency injection.
    /// </summary>
    /// <param name="departmentRepository">The department repository.</param>
    /// <param name="mapper">The AutoMapper instance.</param>
    public DepartmentService(IDepartmentRepository departmentRepository, IMapper mapper)
    {
        _departmentRepository = departmentRepository;
        _mapper = mapper;
    }
    
    /// <summary>
    ///     Creates a new department asynchronously.
    /// </summary>
    /// <param name="department">A DepartmentWriteDto containing the department data to create.</param>
    /// <returns>A Task representing the asynchronous operation, with a DepartmentReadDto containing the created department data.</returns>
    public async Task<DepartmentReadDto> CreateDepartmentAsync(DepartmentWriteDto department)
    {
        
        var result = await _departmentRepository.CreateDepartmentAsync(_mapper.Map<DepartmentModel>(department));
        return _mapper.Map<DepartmentReadDto>(result);
    }

    /// <summary>
    ///     Gets a department by ID asynchronously.
    /// </summary>
    /// <param name="id">The ID of the department to retrieve.</param>
    /// <returns>A Task representing the asynchronous operation, with a DepartmentReadDto containing the department data if found, or null if not found.</returns>
    public async Task<DepartmentReadDto> GetDepartmentByIdAsync(int id)
    {
        var result = await _departmentRepository.GetDepartmentByIdAsync(id);
        return _mapper.Map<DepartmentReadDto>(result);
    }

    /// <summary>
    ///     Gets all departments asynchronously.
    /// </summary>
    /// <returns>A Task representing the asynchronous operation, with a list of DepartmentReadDto objects containing the department data.</returns>
    // Get all departments
    public async Task<IEnumerable<DepartmentReadDto>> GetAllDepartmentsAsync(List<Filter> filters = null)
    {
        var result = await _departmentRepository.GetAllDepartmentsAsync(filters);
        return _mapper.Map<IEnumerable<DepartmentReadDto>>(result);
    }

    /// <summary>
    ///     Updates a department by ID asynchronously.
    /// </summary>
    /// <param name="department">A DepartmentWriteDto containing the updated department data.</param>
    /// <param name="id">The ID of the department to update.</param>
    /// <returns>A Task representing the asynchronous operation, with a DepartmentReadDto containing the updated department data if found, or null if not found.</returns>
    public async Task<DepartmentReadDto> UpdateDepartmentByIdAsync(DepartmentWriteDto department, int id)
    {
        var result = await _departmentRepository.UpdateDepartmentByIdAsync(id ,_mapper.Map<DepartmentModel>(department));
        return _mapper.Map<DepartmentReadDto>(result);
    }

    /// <summary>
    ///     Deletes a department by ID asynchronously.
    /// </summary>
    /// <param name="id">The ID of the department to delete.</param>
    /// <returns>A Task representing the asynchronous operation, with a DepartmentReadDto containing the deleted department data if found, or null if not found.</returns>
    public async Task<DepartmentReadDto> DeleteDepartmentByIdAsync(int id)
    {
        var result = await _departmentRepository.DeleteDepartmentByIdAsync(id);
        return _mapper.Map<DepartmentReadDto>(result);
    }
}