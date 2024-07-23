using TestMVC.Models;
using TestMVC.Models.Entity;
using TestMVC.Models.Enum;

namespace TestMVC.Services.ItemService;

public interface IItemService
{
    Task<Item> CreateItem(Item request);
    List<Item> GetAllItems();
    Task GenerateRandomData();
    Task<IEnumerable<Item>?> GetItemsAlike(string? searchQuery);
    Task<Item?> GetItemById(long id);
    Task<IEnumerable<Item>> GetItemsByCategory(string? category);
    IEnumerable<Item> ApplyFilters(IEnumerable<Item> itemsQuery, Category? category, bool inStock);
}