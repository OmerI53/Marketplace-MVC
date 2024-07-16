using System.Text;
using TestMVC.Models;
using TestMVC.Repository;
using TestMVC.Services.UserService;

namespace TestMVC.Services.ItemService;

public class ItemService(IGenericRepository<UserItems> repository, IUserService userService) : IItemService
{
    public Task<UserItems> SaveData(UserItems request)
    {
        throw new NotImplementedException();
    }

    public List<UserItems?> GetAllData()
    {
        return repository.GetAll().ToList();
    }

    public UserItems? GetDataById(long id)
    {
        return repository.GetById(id);
    }

    public UserItems? GetDataByVal(string val)
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
        var data = new UserItems
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