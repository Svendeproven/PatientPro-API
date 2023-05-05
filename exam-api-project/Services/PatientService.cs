using exam_api_project.models.Dtos;
using exam_api_project.models.Entities;
using exam_api_project.Repositories.Interfaces;
using exam_api_project.Services.Interfaces;

namespace exam_api_project.Services;
/// <summary>
///     Provides services for managing patients.
/// </summary>
public class PatientService : IPatientService
{
    
    private readonly IPatientRepository _patientRepository;
    private readonly IMapper _mapper;
    
    /// <summary>
    ///     Constructor for dependency injection.
    /// </summary>
    /// <param name="patientRepository">The patient repository.</param>
    /// <param name="mapper">The mapper for mapping between DTOs and entity models.</param>
    public PatientService(IPatientRepository patientRepository, IMapper mapper)
    {
        _patientRepository = patientRepository;
        _mapper = mapper;
    }

    /// <summary>
    ///     Creates a new patient.
    /// </summary>
    /// <param name="patientWriteDto">The patient data to create.</param>
    /// <returns>A PatientReadDto containing the created patient's data.</returns>
    public async Task<PatientReadDto> CreatePatientAsync(PatientWriteDto patientWriteDto)
    {
        var result = await _patientRepository.CreatePatientAsync(_mapper.Map<PatientModel>(patientWriteDto));
        return _mapper.Map<PatientReadDto>(result);
    }

    /// <summary>
    ///     Gets a patient by its social security number.
    /// </summary>
    /// <param name="socialSecurityNumber">The social security number of the patient to retrieve.</param>
    /// <returns>A PatientReadDto containing the requested patient's data.</returns>
    public async Task<PatientReadDto> GetPatientBySocialSecurityNumberAsync(string socialSecurityNumber)
    {
        var result = await _patientRepository.GetPatientBySocialSecurityNumberAsync(socialSecurityNumber);
        return _mapper.Map<PatientReadDto>(result);
    }

    /// <summary>
    ///     Gets all patients.
    /// </summary>
    /// <returns>An IEnumerable of PatientReadDto containing the data of all patients.</returns>
    public async Task<IEnumerable<PatientReadDto>> GetAllPatientsAsync()
    {
        var result = await _patientRepository.GetAllPatientsAsync();
        return _mapper.Map<IEnumerable<PatientReadDto>>(result);
    }

    /// <summary>
    ///     Updates a patient by its ID.
    /// </summary>
    /// <param name="id">The ID of the patient to update.</param>
    /// <param name="patient">The updated patient data.</param>
    /// <returns>A PatientReadDto containing the updated patient's data.</returns>
    public async Task<PatientReadDto> UpdatePatientByIdAsync(int id, PatientWriteDto patient)
    {
        var result = await _patientRepository.UpdatePatientByIdAsync(id, _mapper.Map<PatientModel>(patient));
        return _mapper.Map<PatientReadDto>(result);
    }

    /// <summary>
    ///     Deletes a patient by its ID.
    /// </summary>
    /// <param name="id">The ID of the patient to delete.</param>
    /// <returns>A PatientReadDto containing the deleted patient's data.</returns>
    public async Task<PatientReadDto> DeletePatientAsync(int id)
    {
        var result = await _patientRepository.DeletePatientAsync(id);
        return _mapper.Map<PatientReadDto>(result);
    }
}