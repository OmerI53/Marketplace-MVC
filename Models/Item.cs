using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TestMVC.Data;

namespace TestMVC.Models;

[Table("item")]
public class Item
{
    [Key] public long? Id { get; set; }

    [Required] public Category Category { get; set; }
    
    [Required]
    [StringLength(100, ErrorMessage = "Data length can't be more than 100 characters.")]
    public required string ItemName { get; set; }

    [StringLength(300, ErrorMessage = "Data length can't be more than 300 characters.")]
    public string? Description { get; set; }

    [Required] public long? ItemPrice { get; set; }

    public ICollection<User>? Users { get; set; }
}