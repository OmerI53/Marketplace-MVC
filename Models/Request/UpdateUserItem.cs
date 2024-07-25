using System.ComponentModel.DataAnnotations;

namespace TestMVC.Models.Request;

public class UpdateUserItem
{
    public required string ItemId { get; set; }
    [StringLength(36)]
    public required string SellerId { get; set; }

    
    [RegularExpression(@"^[0-9.,]*$", ErrorMessage = "Invalid price format")]
    public string? Price { get; set; }
    public string? Quantity { get; set; }
}