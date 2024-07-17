namespace TestMVC.Models;

public class CreateUserItemModel
{
    public long Id { get; set; }
    public int Quantity { get; set; }
    public long Price { get; set; }

    public Category Category { get; set; }
}