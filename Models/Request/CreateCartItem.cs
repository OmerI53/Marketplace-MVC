namespace TestMVC.Models.Request;

public class CreateCartItem
{
    public long ItemId { get; set; }
    public required string SellerId { get; set; }
    public required string ItemName { get; set; }
    public required string Price { get; set; }
    public int Quantity { get; set; }
}