using TestMVC.Data;
using TestMVC.Models;

namespace TestMVC.Services.UserService;

public interface IUserService
{
    List<User?> GetAllUsers();
    User? GetUserById(string? id);
    User? GetUserByName(string name);
    Task<User?> GetUserByData(string data);
}