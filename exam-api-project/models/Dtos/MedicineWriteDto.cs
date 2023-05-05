namespace exam_api_project.models.Dtos;

public record MedicineWriteDto(
    string Title,
    string Description,
    string ActiveSubstance,
    decimal PricePrMg
);