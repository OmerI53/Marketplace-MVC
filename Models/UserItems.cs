using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestMVC.Models;

[Table("data")]
public class UserItems
{
    public long? Id { get; set; }

    [Required]
    [StringLength(100, ErrorMessage = "Data length can't be more than 100 characters.")]
    public string? ItemName { get; set; }

    [StringLength(300, ErrorMessage = "Data length can't be more than 300 characters.")]
    public string? Description { get; set; }

    [Required] public long? ItemPrice { get; set; }
}