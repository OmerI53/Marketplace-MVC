using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TestMVC.Models.Enum;

namespace TestMVC.Models.Entity;

[Table("Item")]
public class Item
{
    [Key] public long? Id { get; set; }

    [Required] public Category Category { get; set; }

    [Required]
    [StringLength(100, ErrorMessage = "Data length can't be more than 100 characters.")]
    public required string ItemName { get; set; }

    [StringLength(300, ErrorMessage = "Data length can't be more than 300 characters.")]
    public string? Description { get; set; }

    public bool InStock { get; set; }
    public ICollection<UserItem>? UserItems { get; set; }

    // ReSharper disable once CollectionNeverUpdated.Global
    public ICollection<PurchasedItem>? PurchasedItems { get; set; }
}