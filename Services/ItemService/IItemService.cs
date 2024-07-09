using TestMVC.Models;

namespace TestMVC.Services.ItemService;

public interface IItemService
{
    Task<UserItems> SaveData(UserItems request);
    List<UserItems?> GetAllData();
    UserItems? GetDataById(long id);
    UserItems? GetDataByVal(string val);
    Task GenerateRandomData();
}