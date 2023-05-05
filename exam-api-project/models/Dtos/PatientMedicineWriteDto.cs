namespace exam_api_project.models.Dtos;

public record PatientMedicineWriteDto(
    int PatientModelId,
    int MedicineModelId,
    double Amount,
    string Unit
);