using Microsoft.AspNetCore.Mvc;
using TestMVC.Models.Entity;
using TestMVC.Models.Request;
using TestMVC.Services.NotificationService;

namespace TestMVC.Controllers;

public class NotificationController : Controller
{
    private readonly INotificationService _notificationService;

    public NotificationController(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    [HttpPost("send")]
    public async Task<IActionResult> Send(NotificationRequest request)
    {
        await _notificationService.SendNotification(request);
        var returnUrl = HttpContext.Request.Headers.Referer.ToString();
        TempData["InfoMessage"] = "Notification sent";
        return Redirect(returnUrl);
    }

    [HttpGet]
    public async Task<List<Notification>> GetNotificationByUserId(string userId)
    {
        var notifications = await _notificationService.GetNotificationByUserId(userId);
        return notifications.ToList();
    }

    public IActionResult Details()
    {
        throw new NotImplementedException();
    }
}