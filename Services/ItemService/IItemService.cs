using TestMVC.Models;

namespace TestMVC.Services.ItemService;

public interface IItemService
{
    Task<UserItems> SaveData(UserItems request);
    List<UserItems?> GetAllData();
    Task GenerateRandomData();
}