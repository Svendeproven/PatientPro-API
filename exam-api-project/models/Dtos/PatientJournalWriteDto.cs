namespace exam_api_project.models.Dtos;

public record PatientJournalWriteDto(
    int Id,
    string Description,
    int PatientModelId);