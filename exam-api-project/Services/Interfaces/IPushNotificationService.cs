namespace exam_api_project.Services.Interfaces;

public interface IPushNotificationService
{
    public Task SendPushNotificationToUsersByDepartmentIdAsync(string title, string message, int id);
    public Task SendPushNotificationAsync(string title, string message, List<string> tokens);
}