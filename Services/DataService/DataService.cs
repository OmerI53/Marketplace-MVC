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
}