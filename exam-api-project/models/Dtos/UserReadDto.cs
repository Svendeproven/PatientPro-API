using exam_api_project.models.Entities;

namespace exam_api_project.models.Dtos;

public record UserReadDto(
    int Id
    , string Name
    , string Email
    , string Role
    , DateTime CreatedAt
    , DateTime UpdatedAt
    , string JobTitle
    , int DepartmentModelId
);