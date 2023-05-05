using exam_api_project.models.Dtos;
using exam_api_project.models.Entities;
using exam_api_project.Repositories.Interfaces;
using exam_api_project.Services.Interfaces;
using exam_api_project.Utilities;

namespace exam_api_project.Services;

/// <summary>
///     Provides services for managing patient-medicine.
/// </summary>
public class PatientMedicineService : IPatientMedicineService
{
   
    private readonly IPatientMedicineRepository _patientMedicineRepository;
    private readonly IPushNotificationService _pushNotificationService;
    private readonly IMapper _mapper;

    /// <summary>
    ///     Constructor for dependency injection.
    /// </summary>
    /// <param name="patientMedicineRepository">The patient-medicine repository.</param>
    /// <param name="mapper">The mapper for mapping between DTOs and entity models.</param>
    /// <param name="pushNotificationService">A service that sends push notifications via fire-base </param>
    public PatientMedicineService(IPatientMedicineRepository patientMedicineRepository, IMapper mapper, IPushNotificationService pushNotificationService)
    {
        _patientMedicineRepository = patientMedicineRepository;
        _mapper = mapper;
        _pushNotificationService = pushNotificationService;
    }

    /// <summary>
    ///     Creates a new patient-medicine relationship.
    /// </summary>
    /// <param name="patientMedicine">The patient-medicine data to create.</param>
    /// <returns>A PatientMedicineReadDto containing the created patient-medicine's data.</returns>
    public async Task<PatientMedicineReadDto> CreatePatientMedicineAsync(PatientMedicineWriteDto patientMedicine)
    {
        var result =
            await _patientMedicineRepository.CreatePatientMedicineAsync(
                _mapper.Map<PatientMedicineModel>(patientMedicine));
        
// Send push notification to the users about the creation
        await _pushNotificationService.SendPushNotificationToUsersByDepartmentIdAsync("Ny medicin tildelt", $"{result.Patient.Name} er blevet tildelt {result.Medicine.Title}", result.Patient.DepartmentModelId);
        
        return _mapper.Map<PatientMedicineReadDto>(result);
    }

    /// <summary>
    ///     Gets a patient-medicine relationship by its ID.
    /// </summary>
    /// <param name="id">The ID of the patient-medicine relationship to retrieve.</param>
    /// <returns>A PatientMedicineReadDto containing the requested patient-medicine's data.</returns>
    public async Task<PatientMedicineReadDto> GetPatientMedicineByIdAsync(int id)
    {
        var result = await _patientMedicineRepository.GetPatientMedicineByIdAsync(id);
        return _mapper.Map<PatientMedicineReadDto>(result);
    }

    /// <summary>
    ///     Gets all patient-medicine relationships.
    /// </summary>
    /// <param name="filters">A list of filters to apply to the query, if any.</param>
    /// <returns>An IEnumerable of PatientMedicineReadDto containing the data of all patient-medicine relationships.</returns>
    public async Task<IEnumerable<PatientMedicineReadDto>> GetAllPatientMedicinesAsync(List<Filter> filters = null)
    {
        var result = await _patientMedicineRepository.GetAllPatientMedicinesAsync(filters);
        return _mapper.Map<IEnumerable<PatientMedicineReadDto>>(result);
    }

    /// <summary>
    ///     Updates a patient-medicine relationship by its ID.
    /// </summary>
    /// <param name="id">The ID of the patient-medicine relationship to update.</param>
    /// <param name="patientMedicine">The updated patient-medicine data.</param>
    /// <returns>A PatientMedicineReadDto containing the updated patient-medicine's data, or null if not found.</returns>
    public async Task<PatientMedicineReadDto> UpdatePatientMedicineByIdAsync(int id,
        PatientMedicineWriteDto patientMedicine)
    {
        var result =
            await _patientMedicineRepository.UpdatePatientMedicineByIdAsync(id,
                _mapper.Map<PatientMedicineModel>(patientMedicine));
        return result != null ? _mapper.Map<PatientMedicineReadDto>(result) : null;
    }

    /// <summary>
    ///     Deletes a patient-medicine relationship by its ID.
    /// </summary>
    /// <param name="id">The ID of the patient-medicine relationship to delete.</param>
    /// <returns>A PatientMedicineReadDto containing the deleted patient-medicine's data, or null if not found.</returns>
    public async Task<PatientMedicineReadDto> DeletePatientMedicineByIdAsync(int id)
    {
        var result = await _patientMedicineRepository.DeletePatientMedicineAsync(id);
        return result != null ? _mapper.Map<PatientMedicineReadDto>(result) : null;
    }
}