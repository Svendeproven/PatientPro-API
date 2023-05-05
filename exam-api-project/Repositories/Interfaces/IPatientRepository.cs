using exam_api_project.models.Entities;

namespace exam_api_project.Repositories.Interfaces;

public interface IPatientRepository
{
    // CRUD operations for patient
    public Task<PatientModel> CreatePatientAsync(PatientModel patient);
    public Task<PatientModel> GetPatientBySocialSecurityNumberAsync(string socialSecurityNumber);
    public Task<IEnumerable<PatientModel>> GetAllPatientsAsync();
    public Task<PatientModel> UpdatePatientByIdAsync(int id, PatientModel patient);
    public Task<PatientModel> DeletePatientAsync(int id);
}