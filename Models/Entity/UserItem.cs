using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TestMVC.Data;

namespace TestMVC.Models;

public class UserItem
{
    [Key, Column(Order = 0)] public long ItemId { get; set; }
    public Item Item { get; set; }

    [Key, Column(Order = 1)] public string UserId { get; set; }
    public User User { get; set; }

    public int Quantity { get; set; }
    public float Price { get; set; }
}