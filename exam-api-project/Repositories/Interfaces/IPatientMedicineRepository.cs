using exam_api_project.models.Entities;
using exam_api_project.Utilities;

namespace exam_api_project.Repositories.Interfaces;

public interface IPatientMedicineRepository
{
    // CRUD operations for PatientMedicine table
    Task<PatientMedicineModel> CreatePatientMedicineAsync(PatientMedicineModel patientMedicine);
    Task<PatientMedicineModel> GetPatientMedicineByIdAsync(int id);
    Task<IEnumerable<PatientMedicineModel>> GetAllPatientMedicinesAsync(List<Filter> filters = null);
    Task<PatientMedicineModel> UpdatePatientMedicineByIdAsync(int id, PatientMedicineModel patientMedicine);
    Task<PatientMedicineModel> DeletePatientMedicineAsync(int id);
}