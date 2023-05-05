namespace exam_api_project.models.Dtos;

public record MedicineReadDto(
    int Id,
    string Title,
    string Description,
    string ActiveSubstance,
    decimal PricePrMg);