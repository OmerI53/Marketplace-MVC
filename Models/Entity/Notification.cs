using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestMVC.Models.Entity;

public class Notification
{
    [Key]
    [Column("Id")]
    public long Id { get; set; }

    [StringLength(300)]
    [Column("Message")]
    public required string Message { get; set; }

    [Column("Topic")]
    public required string Topic { get; set; }

    [Column("ReceiverId")]
    // ReSharper disable once EntityFramework.ModelValidation.UnlimitedStringLength
    public required string ReceiverId { get; set; }

    [ForeignKey("ReceiverId")]
    [Column("ReceiverUser")]
    public required User Receiver { get; set; }

    [Column("IsRead")]
    public required bool IsRead { get; set; }
}
