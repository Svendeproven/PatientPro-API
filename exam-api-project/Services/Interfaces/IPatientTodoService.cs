using exam_api_project.models.Dtos;
using exam_api_project.Utilities;

namespace exam_api_project.Services.Interfaces;

public interface IPatientTodoService
{
    // CRUD operations for patient
    public Task<PatientTodoReadDto> CreatePatientTodoAsync(PatientTodoWriteDto patientTodo);
    public Task<PatientTodoReadDto> GetPatientTodoByIdAsync(int id);
    public Task<IEnumerable<PatientTodoReadDto>> GetAllPatientTodosAsync(List<Filter> filters = null);
    public Task<PatientTodoReadDto> UpdatePatientTodoByIdAsync(int id, PatientTodoWriteDto patientTodo);
    public Task<PatientTodoReadDto> DeletePatientTodoAsync(int id);
}