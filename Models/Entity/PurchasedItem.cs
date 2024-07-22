using System.ComponentModel.DataAnnotations;

namespace TestMVC.Models.Entity;

public class PurchasedItem
{
    [Key] public long Id { get; set; }

    [StringLength(36)] public required string BuyerId { get; set; }
    public required User Buyer { get; set; }

    public long ItemId { get; set; }
    public Item? Item { get; set; }
    [StringLength(36)] public required string SellerId { get; set; }

    public DateTime PurchaseDate { get; set; }
    public int Quantity { get; set; }
    public double PerItemPrice { get; set; }
}