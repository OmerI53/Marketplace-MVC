using System.ComponentModel.DataAnnotations;

namespace TestMVC.Models.Request;

public class DeleteUserItem
{
    public required string ItemId { get; set; }
    [StringLength(36)]
    public required string SellerId { get; set; }
}