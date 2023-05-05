using exam_api_project.models.Entities;
using exam_api_project.Utilities;

namespace exam_api_project.Repositories.Interfaces;

public interface IPatientJournalRepository
{
    // CRUD operations for journal
    public Task<PatientJournalModel> CreatePatientJournalAsync(PatientJournalModel patientJournal);
    public Task<PatientJournalModel> GetPatientJournalByIdAsync(int id);
    public Task<IEnumerable<PatientJournalModel>> GetAllPatientJournalsAsync(List<Filter> filters = null);
    public Task<PatientJournalModel> UpdatePatientJournalByIdAsync(int id, PatientJournalModel patientJournal);
    public Task<PatientJournalModel> DeletePatientJournalAsync(int id);
}