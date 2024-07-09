using TestMVC.Models;

namespace TestMVC.Services.UserService;

public interface IUserService
{
    Task<User?> CreateUser(User request);
    List<User?> GetAllUsers();
    Task<User> GetUserById(long id);
    User? GetUserByName(string name);
    Task<User?> GetUserByData(string data);
}