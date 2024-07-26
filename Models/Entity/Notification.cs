using System.ComponentModel.DataAnnotations;

namespace TestMVC.Models.Entity;

public class Notification
{
    [Key] public long Id;
    [StringLength(300)] public required string Message;
    public string? Topic;
    public required string ReceiverId;
    public required User Receiver;
    public bool IsRead;
}