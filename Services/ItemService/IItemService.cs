using TestMVC.Models;

namespace TestMVC.Services.ItemService;

public interface IItemService
{
    Task<Item> CreateItem(Item request);
    List<Item> GetAllItems();
    Task GenerateRandomData();
    Task<IEnumerable<Item>?> GetItemsAlike(string? searchQuery);
    Task<Item?> GetItemById(long id);
    Task<IEnumerable<Item>> GetItemsByCategory(string? category);
}