using exam_api_project.models.Entities;

namespace exam_api_project.models.Dtos;

public record PatientWriteDto(
    string Name,
    string SocialSecurityNumber,
    int DepartmentModelId
);