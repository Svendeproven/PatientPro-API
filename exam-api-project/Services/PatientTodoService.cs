using exam_api_project.models.Dtos;
using exam_api_project.models.Entities;
using exam_api_project.Repositories.Interfaces;
using exam_api_project.Services.Interfaces;
using exam_api_project.Utilities;

namespace exam_api_project.Services;

/// <summary>
///     Provides services for managing patient todos.
/// </summary>
public class PatientTodoService : IPatientTodoService
{
    
    private readonly IPatientTodoRepository _patientTodoRepository;
    private readonly IMapper _mapper;

    /// <summary>
    ///     Constructor for dependency injection.
    /// </summary>
    /// <param name="patientTodoRepository">The patient todos repository.</param>
    /// <param name="mapper">The mapper for mapping between DTOs and entity models.</param>
    public PatientTodoService(IPatientTodoRepository patientTodoRepository, IMapper mapper)
    {
        _patientTodoRepository = patientTodoRepository;
        _mapper = mapper;
    }

    /// <summary>
    ///     Creates a new patient todos.
    /// </summary>
    /// <param name="patientTodo">The patient todos data to create.</param>
    /// <returns>A PatientTodoReadDto containing the created patient todos data.</returns>
    public async Task<PatientTodoReadDto> CreatePatientTodoAsync(PatientTodoWriteDto patientTodo)
    {
        var result = await _patientTodoRepository.CreatePatientTodoAsync(_mapper.Map<PatientTodoModel>(patientTodo));
        return _mapper.Map<PatientTodoReadDto>(result);
    }

    /// <summary>
    ///     Gets a patient todos by its ID.
    /// </summary>
    /// <param name="id">The ID of the patient todos to retrieve.</param>
    /// <returns>A PatientTodoReadDto containing the requested patient todos data.</returns>
    public async Task<PatientTodoReadDto> GetPatientTodoByIdAsync(int id)
    {
        var result = await _patientTodoRepository.GetPatientTodoByIdAsync(id);
        return _mapper.Map<PatientTodoReadDto>(result);
    }

    /// <summary>
    ///     Gets all patient todos.
    /// </summary>
    /// <param name="filters">Optional list of filters to apply.</param>
    /// <returns>An IEnumerable of PatientTodoReadDto containing the data of all patient todos.</returns>
    public async Task<IEnumerable<PatientTodoReadDto>> GetAllPatientTodosAsync(List<Filter> filters = null)
    {
        var result = await _patientTodoRepository.GetAllPatientTodosAsync(filters);
        return _mapper.Map<IEnumerable<PatientTodoReadDto>>(result);
    }

    /// <summary>
    ///     Updates a patient todos by its ID.
    /// </summary>
    /// <param name="id">The ID of the patient todos to update.</param>
    /// <param name="patientTodo">The updated patient todos data.</param>
    /// <returns>A PatientTodoReadDto containing the updated patient todos data, or null if not found.</returns>
    public async Task<PatientTodoReadDto> UpdatePatientTodoByIdAsync(int id, PatientTodoWriteDto patientTodo)
    {
        var result =
            await _patientTodoRepository.UpdatePatientTodoByIdAsync(id, _mapper.Map<PatientTodoModel>(patientTodo));
        return result != null ? _mapper.Map<PatientTodoReadDto>(result) : null;
    }

    /// <summary>
    ///     Deletes a patient todos by its ID.
    /// </summary>
    /// <param name="id">The ID of the patient todos to delete.</param>
    /// <returns>A PatientTodoReadDto containing the deleted patient todos data, or null if not found.</returns>
    public async Task<PatientTodoReadDto> DeletePatientTodoAsync(int id)
    {
        var result = await _patientTodoRepository.DeletePatientTodoAsync(id);
        return result != null ? _mapper.Map<PatientTodoReadDto>(result) : null;
    }
}