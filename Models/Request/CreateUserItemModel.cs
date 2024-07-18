using TestMVC.Models.Enum;

namespace TestMVC.Models.Request;

public class CreateUserItemModel
{
    public long Id { get; set; }
    public int Quantity { get; set; }
    public string Price { get; set; }

    public Category Category { get; set; }
}