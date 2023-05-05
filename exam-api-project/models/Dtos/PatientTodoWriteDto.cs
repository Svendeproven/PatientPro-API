namespace exam_api_project.models.Dtos;

public record PatientTodoWriteDto
(
    int PatientMedicineModelId, // Foreign key
    int UserModelId, // Foreign key
    bool Done,
    int PatientModelId,
    DateTime PlannedTimeAtDay
);