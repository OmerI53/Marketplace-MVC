namespace TestMVC.Models.Request;

public class NotificationRequest
{
    public string? Username { get; set; }
    public string? Role { get; set; }
    public string Title { get; set; }
    public string Message { get; set; }
}