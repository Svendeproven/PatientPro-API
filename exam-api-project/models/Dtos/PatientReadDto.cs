using exam_api_project.models.Entities;

namespace exam_api_project.models.Dtos;

public record PatientReadDto(
    int Id,
    string Name,
    string SocialSecurityNumber
    );