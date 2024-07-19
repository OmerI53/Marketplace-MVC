using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestMVC.Models.Entity;

public class UserItem
{
    [Key, Column(Order = 0)] public long ItemId { get; set; }
    public Item? Item { get; set; }

    [Key, Column(Order = 1)]
    [StringLength(36)]
    public required string SellerId { get; set; }

    public User? Seller { get; set; }

    public int Quantity { get; set; }
    public float Price { get; set; }
}