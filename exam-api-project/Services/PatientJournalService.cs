using exam_api_project.models.Dtos;
using exam_api_project.models.Entities;
using exam_api_project.Repositories.Interfaces;
using exam_api_project.Services.Interfaces;
using exam_api_project.Utilities;

namespace exam_api_project.Services;

/// <summary>
///     Provides services for managing patient journals.
/// </summary>
public class PatientJournalService : IPatientJournalService
{
    
    private readonly IPatientJournalRepository _patientJournalRepository;
    private readonly IMapper _mapper;


    /// <summary>
    ///     Constructor for dependency injection.
    /// </summary>
    /// <param name="patientJournalRepository">The patient journal repository.</param>
    /// <param name="mapper">The mapper for mapping between DTOs and entity models.</param>
    public PatientJournalService(IPatientJournalRepository patientJournalRepository, IMapper mapper)
    {
        _patientJournalRepository = patientJournalRepository;
        _mapper = mapper;
    }

    /// <summary>
    ///     Creates a new patient journal.
    /// </summary>
    /// <param name="patientJournal">The patient journal data to create.</param>
    /// <returns>A PatientJournalReadDto containing the created patient journal's data.</returns>
    public async Task<PatientJournalReadDto> CreateJournalAsync(PatientJournalWriteDto patientJournal)
    {
        var result =
            await _patientJournalRepository.CreatePatientJournalAsync(
                _mapper.Map<PatientJournalModel>(patientJournal));
        return _mapper.Map<PatientJournalReadDto>(result);
    }

    /// <summary>
    ///     Gets a patient journal by its ID.
    /// </summary>
    /// <param name="id">The ID of the patient journal to retrieve.</param>
    /// <returns>A PatientJournalReadDto containing the requested patient journal's data.</returns>
    public async Task<PatientJournalReadDto> GetJournalByIdAsync(int id)
    {
        var result = await _patientJournalRepository.GetPatientJournalByIdAsync(id);
        return _mapper.Map<PatientJournalReadDto>(result);
    }

    /// <summary>
    ///     Gets all patient journals.
    /// </summary>
    /// <param name="filters">A list of filters to apply to the query, if any.</param>
    /// <returns>An IEnumerable of PatientJournalReadDto containing the data of all patient journals.</returns>
    public async Task<IEnumerable<PatientJournalReadDto>> GetAllJournalsAsync(List<Filter> filters = null)
    {
        var result = await _patientJournalRepository.GetAllPatientJournalsAsync(filters);
        return _mapper.Map<IEnumerable<PatientJournalReadDto>>(result);
    }

    /// <summary>
    ///     Updates a patient journal by its ID.
    /// </summary>
    /// <param name="id">The ID of the patient journal to update.</param>
    /// <param name="patientJournal">The updated patient journal data.</param>
    /// <returns>A PatientJournalReadDto containing the updated patient journal's data.</returns>
    public async Task<PatientJournalReadDto> UpdateJournalByIdAsync(int id, PatientJournalWriteDto patientJournal)
    {
        var result =
            await _patientJournalRepository.UpdatePatientJournalByIdAsync(id,
                _mapper.Map<PatientJournalModel>(patientJournal));
        return _mapper.Map<PatientJournalReadDto>(result);
    }

    /// <summary>
    ///     Deletes a patient journal by its ID.
    /// </summary>
    /// <param name="id">The ID of the patient journal to delete.</param>
    /// <returns>A PatientJournalReadDto containing the deleted patient journal's data.</returns>
    public async Task<PatientJournalReadDto> DeleteJournalAsync(int id)
    {
        var result = await _patientJournalRepository.DeletePatientJournalAsync(id);
        return _mapper.Map<PatientJournalReadDto>(result);
    }
}