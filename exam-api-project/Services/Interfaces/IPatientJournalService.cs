using exam_api_project.models.Dtos;
using exam_api_project.Utilities;

namespace exam_api_project.Services.Interfaces;

public interface IPatientJournalService
{
    // CRUD operations for Journal
    Task<PatientJournalReadDto> CreateJournalAsync(PatientJournalWriteDto patientJournal);
    Task<PatientJournalReadDto> GetJournalByIdAsync(int id);
    Task<IEnumerable<PatientJournalReadDto>> GetAllJournalsAsync(List<Filter> filters = null);
    Task<PatientJournalReadDto> UpdateJournalByIdAsync(int id, PatientJournalWriteDto patientJournal);
    Task<PatientJournalReadDto> DeleteJournalAsync(int id);
}