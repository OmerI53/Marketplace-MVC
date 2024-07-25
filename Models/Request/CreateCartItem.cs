using System.ComponentModel.DataAnnotations;

namespace TestMVC.Models.Request;

public class CreateCartItem
{
    public long ItemId { get; set; }
    public required string SellerId { get; set; }
    public required string ItemName { get; set; }
    
    [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Invalid price format")]
    public required string Price { get; set; }
    public int Quantity { get; set; }
}