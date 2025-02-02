using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;
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
            Receiver = user,
            IsRead = false
        };
        await _repository.Insert(notification);
    }

    public async Task<IEnumerable<Notification>> GetNotificationByUserId(string userId)
    {
        return await _repository.FindAsync(n => n.ReceiverId == userId && !n.IsRead);
    }
}