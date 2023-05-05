namespace exam_api_project.models.Dtos;

public record UserLoginDto(
    string Email
    , string Password
    , string DeviceToken
);