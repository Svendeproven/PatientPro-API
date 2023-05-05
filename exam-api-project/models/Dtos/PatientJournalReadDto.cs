namespace exam_api_project.models.Dtos;

public record PatientJournalReadDto(
    int Id,
    string Description,
    DateTime CreatedAt
);