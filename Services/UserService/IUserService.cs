using TestMVC.Data;
using TestMVC.Models;

namespace TestMVC.Services.UserService;

public interface IUserService
{
    List<ApplicationUser?> GetAllUsers();
    Task<ApplicationUser> GetUserById(long id);
    ApplicationUser? GetUserByName(string name);
    Task<ApplicationUser?> GetUserByData(string data);
}