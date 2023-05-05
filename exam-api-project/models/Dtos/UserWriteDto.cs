namespace exam_api_project.models.Dtos;

// Record for caring data about user when creating new user
public record UserWriteDto(
    string Name
    , string Password
    , string Email
    , string Role
    , string JobTitle
    , int DepartmentModelId
);