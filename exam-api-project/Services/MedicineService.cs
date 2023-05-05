using exam_api_project.models.Dtos;
using exam_api_project.models.Entities;
using exam_api_project.Repositories.Interfaces;
using exam_api_project.Services.Interfaces;

namespace exam_api_project.Services;

/// <summary>
///     Provides services for managing medicines.
/// </summary>
public class MedicineService :IMedicineService
{
    private readonly IMedicineRepository _medicineRepository;
    private readonly IMapper _mapper;

    /// <summary>
    ///     Constructor for dependency injection.
    /// </summary>
    /// <param name="medicineRepository">The medicine repository.</param>
    /// <param name="mapper">The mapper for mapping between DTOs and entity models.</param>
    public MedicineService(IMedicineRepository medicineRepository, IMapper mapper)
    {
        _medicineRepository = medicineRepository;
        _mapper = mapper;
    }
    
    /// <summary>
    ///     Creates a new medicine.
    /// </summary>
    /// <param name="medicine">The medicine data to create.</param>
    /// <returns>A MedicineReadDto containing the created medicine's data.</returns>
    public async Task<MedicineReadDto> CreateNewMedicineAsync(MedicineWriteDto medicine)
    {
        var result = await _medicineRepository.CreateNewMedicineAsync(_mapper.Map<MedicineModel>(medicine));
        return _mapper.Map<MedicineReadDto>(result);
    }

    /// <summary>
    ///     Gets a medicine by its ID.
    /// </summary>
    /// <param name="id">The ID of the medicine to retrieve.</param>
    /// <returns>A MedicineReadDto containing the requested medicine's data.</returns>
    public async Task<MedicineReadDto> GetMedicineByIdAsync(int id)
    {
        var result = await _medicineRepository.GetMedicineByIdAsync(id);
        return _mapper.Map<MedicineReadDto>(result);
    }

    /// <summary>
    ///     Gets all medicines.
    /// </summary>
    /// <returns>An IEnumerable of MedicineReadDto containing the data of all medicines.</returns>
    public async Task<IEnumerable<MedicineReadDto>> GetAllMedicineAsync()
    {
        var result = await _medicineRepository.GetAllMedicineAsync();
        return _mapper.Map<IEnumerable<MedicineReadDto>>(result);
    }

    /// <summary>
    ///     Updates a medicine by its ID.
    /// </summary>
    /// <param name="medicine">The updated medicine data.</param>
    /// <param name="id">The ID of the medicine to update.</param>
    /// <returns>A MedicineReadDto containing the updated medicine's data.</returns>
    public async Task<MedicineReadDto> UpdateMedicineByIdAsync(MedicineWriteDto medicine, int id)
    {
        var result = await _medicineRepository.UpdateExistingMedicineAsync(_mapper.Map<MedicineModel>(medicine), id);
        return _mapper.Map<MedicineReadDto>(result);
    }

    /// <summary>
    ///     Deletes a medicine by its ID.
    /// </summary>
    /// <param name="id">The ID of the medicine to delete.</param>
    /// <returns>A MedicineReadDto containing the deleted medicine's data.</returns>
    public async Task<MedicineReadDto> DeleteMedicineByIdAsync(int id)
    {
        var result = await _medicineRepository.DeleteMedicineByIdAsync(id);
        return _mapper.Map<MedicineReadDto>(result);
    }
}