using TestMVC.Models;

namespace TestMVC.Services.DataService;

public interface IDataService
{
    Task<TestData> SaveData(TestData request);
    List<TestData?> GetAllData();
    TestData? GetDataById(long id);
    TestData? GetDataByVal(string val);
    Task GenerateRandomData();
}