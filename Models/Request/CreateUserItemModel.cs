using System.ComponentModel.DataAnnotations;
using TestMVC.Models.Enum;

namespace TestMVC.Models.Request;

public class CreateUserItemModel
{
    public long Id { get; set; }
    public int Quantity { get; set; }
    

    [RegularExpression(@"^\d{1,3}(,\d{3})*(\.\d+)?$", ErrorMessage = "Invalid price format")]
    public required string Price { get; set; }

    public Category Category { get; set; }
}