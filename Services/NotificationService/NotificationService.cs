using Confluent.Kafka;
using TestMVC.Models.Entity;
using TestMVC.Models.Request;
using TestMVC.Repository;
using TestMVC.Services.UserService;

namespace TestMVC.Services.NotificationService;

public class NotificationService : INotificationService
{
    private readonly IGenericRepository<Notification> _repository;
    private readonly IUserService _userService;

    // ReSharper disable once ConvertToPrimaryConstructor
    public NotificationService(IGenericRepository<Notification> repository, IUserService userService)
    {
        _repository = repository;
        _userService = userService;
    }

    public async Task SendNotification(NotificationRequest message)
    {
        var user = _userService.GetUserByUsername(message.Username);
        if (user == null)
        {
            throw new Exception("User not found");
        }

        var notification = new Notification
        {
            Message = message.Message,
            Topic = message.Title,
            ReceiverId = user.Id,
            Receiver = user
        };
        await _repository.Insert(notification);
    }

    public IEnumerable<Notification> GetNotificationByUserId(string userId)
    {
        throw new NotImplementedException();
    }
}