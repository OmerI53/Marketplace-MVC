namespace TestMVC.Models;

public class CartItem
{
    public long ItemId { get; set; }
    public required string SellerId { get; set; }
    public required string ItemName { get; set; }
    public double Price { get; set; }
    public int Quantity { get; set; }
}