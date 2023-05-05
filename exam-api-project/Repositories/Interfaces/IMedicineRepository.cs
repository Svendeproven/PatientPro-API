using exam_api_project.models.Entities;

namespace exam_api_project.Repositories.Interfaces;

public interface IMedicineRepository
{
    // async crud operations for medicine
    Task<MedicineModel> CreateNewMedicineAsync(MedicineModel medicine);
    Task<MedicineModel> UpdateExistingMedicineAsync(MedicineModel medicineWriteUpdate, int id);
    Task<MedicineModel> DeleteMedicineByIdAsync(int id);
    Task<List<MedicineModel>> GetAllMedicineAsync();
    Task<MedicineModel> GetMedicineByIdAsync(int id);
}