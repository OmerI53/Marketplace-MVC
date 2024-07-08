using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestMVC.Models;

[Table("data")]
public class TestData
{
    public long? Id { get; set; }

    [Required]
    [StringLength(100, ErrorMessage = "Data length can't be more than 100 characters.")]
    public string? Data { get; set; }
    public long? UserId { get; set; }
    public User User { get; set; }
}