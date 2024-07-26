using Microsoft.AspNetCore.Mvc;
using TestMVC.Models.Request;
using TestMVC.Services.NotificationService;

namespace TestMVC.Controllers;

public class NotificationController : Controller
{
    private readonly INotificationService _notificationService;

    public NotificationController( INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    [HttpPost("send")]
    public async Task Send(NotificationRequest request)
    {
        await _notificationService.SendNotification(request);
    }

    [HttpGet]
    public void GetNotificationByUserId(string userId)
    {
        var notifications = _notificationService.GetNotificationByUserId(userId);
    }
}