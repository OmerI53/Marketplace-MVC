namespace TestMVC.Models;

public class CartItem
{
    public long ItemId { get; set; }
    public string UserId { get; set; }
    public string ItemName { get; set; }
    public float Price { get; set; }
    public int Quantity { get; set; }
}