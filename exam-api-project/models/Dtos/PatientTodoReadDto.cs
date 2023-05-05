using exam_api_project.models.Entities;

namespace exam_api_project.models.Dtos;

public record PatientTodoReadDto
(
    int Id,
    PatientMedicineReadDto PatientMedicine,
    bool Done,
    DateTime PlannedTimeAtDay,
    int PatientModelId
);