using exam_api_project.models.Dtos;

namespace exam_api_project.Services.Interfaces;

public interface IPatientService
{
    // CRUD operations for patient
    public Task<PatientReadDto> CreatePatientAsync(PatientWriteDto result);
    public Task<PatientReadDto> GetPatientBySocialSecurityNumberAsync(string socialSecurityNumber);
    public Task<IEnumerable<PatientReadDto>> GetAllPatientsAsync();
    public Task<PatientReadDto> UpdatePatientByIdAsync(int id, PatientWriteDto patient);
    public Task<PatientReadDto> DeletePatientAsync(int id);
}