using exam_api_project.models.Entities;
using exam_api_project.Utilities;

namespace exam_api_project.Repositories.Interfaces;

public interface IPatientTodoRepository
{
    // CRUD operations for patienttodo
    public Task<PatientTodoModel> CreatePatientTodoAsync(PatientTodoModel patientTodo);
    public Task<PatientTodoModel> GetPatientTodoByIdAsync(int id);
    public Task<IEnumerable<PatientTodoModel>> GetAllPatientTodosAsync(List<Filter> filters = null);
    public Task<PatientTodoModel> UpdatePatientTodoByIdAsync(int id, PatientTodoModel patientTodo);
    public Task<PatientTodoModel> DeletePatientTodoAsync(int id);
}