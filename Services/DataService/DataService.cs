using System.Text;
using TestMVC.Models;
using TestMVC.Repository;
using TestMVC.Services.UserService;

namespace TestMVC.Services.DataService;

public class DataService(IGenericRepository<TestData> repository, IUserService userService) : IDataService
{
    public Task<TestData> SaveData(TestData request)
    {
        var data = new TestData
        {
            Data = request.Data,
            UserId = request.UserId
        };
        var savedData = repository.Insert(data);
        return savedData;
    }

    public List<TestData?> GetAllData()
    {
        return repository.GetAll().ToList();
    }

    public TestData? GetDataById(long id)
    {
        return repository.GetById(id);
    }

    public TestData? GetDataByVal(string val)
    {
        var data = repository.GetAll().FirstOrDefault(x => x?.Data == val);
        return data;
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
        var data = new TestData
        {
            Data = GenerateRandomText(5),
            UserId = user!.Id
        };
        await repository.Insert(data);
        Console.WriteLine($"Generated data for user {user.Name}");
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