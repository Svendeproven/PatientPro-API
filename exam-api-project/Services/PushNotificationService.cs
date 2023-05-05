using exam_api_project.Repositories.Interfaces;
using exam_api_project.Services.Interfaces;
using FirebaseAdmin.Messaging;
using Microsoft.CodeAnalysis.Elfie.Serialization;

namespace exam_api_project.Services;

public class PushNotificationService : IPushNotificationService
{
    private readonly IDeviceRepository _deviceRepository;
    
    public PushNotificationService(IDeviceRepository deviceRepository)
    {
        _deviceRepository = deviceRepository;
    }
    

    public async Task SendPushNotificationToUsersByDepartmentIdAsync(string title, string message, int id)
    {
        var devices = await _deviceRepository.GetDevicesByDepartmentIdAsync(id);
        var tokens = devices.Select(d => d.Token).ToList();
        await SendPushNotificationAsync(title, message, tokens);
    }
    
    public async Task SendPushNotificationAsync(string title, string message, List<string> tokens)
    {
        var m = new MulticastMessage
        {
            Apns = new ApnsConfig
            {
                Aps = new Aps
                {
                    Badge = 0,
                    Sound = "default"
                }
            },

            Notification = new Notification
            {
                Body = message,
                Title = title
            },
            // for the phone
            Tokens = tokens
        };

        // Send a message to the device corresponding to the provided
        // registration token.
        var response = await FirebaseMessaging.DefaultInstance.SendMulticastAsync(m);
        // Response is a message ID string.
        Console.WriteLine("Successfully sent message: " + response);
    }
}