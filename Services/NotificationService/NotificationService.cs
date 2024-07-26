using Confluent.Kafka;
using TestMVC.Models.Entity;
using TestMVC.Models.Request;
using TestMVC.Repository;

namespace TestMVC.Services.NotificationService;

public class NotificationService : INotificationService
{
    private readonly IGenericRepository<Notification> _repository;

    public NotificationService(IGenericRepository<Notification> repository)
    {
        _repository = repository;
    }

    public Task SendNotification(NotificationRequest message)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Notification> GetNotificationByUserId(string userId)
    {
        throw new NotImplementedException();
    }
}