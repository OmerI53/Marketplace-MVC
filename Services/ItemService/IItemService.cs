using TestMVC.Models;

namespace TestMVC.Services.ItemService;

public interface IItemService
{
    Task<Item> CreateItem(Item request);
    List<Item> GetAllItems();
    Task GenerateRandomData();
    Task<IEnumerable<Item>?> GetItemsAlike(string? searchQuery);
}