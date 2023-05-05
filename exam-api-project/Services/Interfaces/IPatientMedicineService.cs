using exam_api_project.models.Dtos;
using exam_api_project.Utilities;

namespace exam_api_project.Services.Interfaces;

public interface IPatientMedicineService
{
    // CRUD operations for PatientMedicine table
    Task<PatientMedicineReadDto> CreatePatientMedicineAsync(PatientMedicineWriteDto patientMedicine);
    Task<PatientMedicineReadDto> GetPatientMedicineByIdAsync(int id);
    Task<IEnumerable<PatientMedicineReadDto>> GetAllPatientMedicinesAsync(List<Filter> filters = null);
    Task<PatientMedicineReadDto> UpdatePatientMedicineByIdAsync(int id, PatientMedicineWriteDto patientMedicine);
    Task<PatientMedicineReadDto> DeletePatientMedicineByIdAsync(int id);
}