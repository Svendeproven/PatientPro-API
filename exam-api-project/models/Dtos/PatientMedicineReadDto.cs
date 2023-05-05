namespace exam_api_project.models.Dtos;

public record PatientMedicineReadDto(
    int Id,
    PatientReadDto Patient,
    MedicineReadDto Medicine,
    double Amount,
    string Unit
);