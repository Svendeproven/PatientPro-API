using exam_api_project.models.Dtos;

namespace exam_api_project.Services.Interfaces;

public interface IMedicineService
{
   // CRUD operations for Medicine
   public Task<MedicineReadDto> CreateNewMedicineAsync(MedicineWriteDto medicine);
   public Task<MedicineReadDto> GetMedicineByIdAsync(int id);
   public Task<IEnumerable<MedicineReadDto>> GetAllMedicineAsync();
   public Task<MedicineReadDto> UpdateMedicineByIdAsync(MedicineWriteDto medicine, int id);
   public Task<MedicineReadDto> DeleteMedicineByIdAsync(int id);
}