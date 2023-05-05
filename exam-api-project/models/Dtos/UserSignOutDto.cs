namespace exam_api_project.models.Dtos;

public record UserSignOutDto(
    int UserId,
    string DeviceToken
    );