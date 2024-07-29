using Confluent.Kafka;
using TestMVC.Models.Entity;
using TestMVC.Models.Request;

namespace TestMVC.Services.NotificationService;

public interface INotificationService
{
    Task SendNotification(NotificationRequest message);
    Task<IEnumerable<Notification>> GetNotificationByUserId(string userId);
}