using System.Text;
using TestMVC.Models;
using TestMVC.Repository;
using TestMVC.Services.UserService;

namespace TestMVC.Services.ItemService;

public class ItemService(IGenericRepository<Item> repository, IUserService userService) : IItemService
{
    public Task<Item> CreateItem(Item request)
    {
        return repository.Insert(request);
    }

    public List<Item?> GetAllData()
    {
        return repository.GetAll().ToList();
    }

    public Item? GetDataById(long id)
    {
        return repository.GetById(id);
    }

    public Item? GetDataByVal(string val)
    {
        throw new NotImplementedException();
    }

    public async Task GenerateRandomData()
    {
        var random = new Random();
        var allUsers = userService.GetAllUsers();
        if (allUsers.Count == 0)
        {
            Console.WriteLine("No users found");
            return;
        }

        var user = allUsers[random.Next(allUsers.Count)];
        var data = new Item
        {
        };
        await repository.Insert(data);
    }

    private static string GenerateRandomText(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        var result = new StringBuilder(length);
        var random = new Random();

        for (var i = 0; i < length; i++)
        {
            result.Append(chars[random.Next(chars.Length)]);
        }

        return result.ToString();
    }
}