using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestMVC.Models;

[Table("users")]
public class User
{
    [Key] public long Id { get; init; }

    [Required]
    [StringLength(50, ErrorMessage = "Name length can't be more than 50 characters.")]
    public string? Name { get; init; }
    public ICollection<TestData> Data { get; set; }
}