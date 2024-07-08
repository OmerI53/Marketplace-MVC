using TestMVC.Models;
using TestMVC.Repository;

namespace TestMVC.Services.UserService;

public class UserService : IUserService
{
    private readonly IGenericRepository<User> _context;

    // ReSharper disable once ConvertToPrimaryConstructor
    public UserService(IGenericRepository<User> context)
    {
        _context = context;
    }

    public async Task<User?> CreateUser(User request)
    {
        var user = new User
        {
            Name = request.Name,
            Data = new List<TestData>()
        };
        await _context.Insert(user);
        return user;
    }

    public List<User> GetAllUsers()
    {
        /*
        var result = _context.Users
            .Select(u => new
            {
                User = u,
                Data = u.Data.Select(d => d.Data)
            })
            .ToList()
            .Select(item =>
            {
                item.User.Data = item.Data.Select(x => new TestData { Data = x }).ToList();
                return item.User;
            })
            .ToList();
        */
        var result = _context.GetAll()
            .Select(u => new
            {
                User = u,
                Data = u.Data.Select(d => d.Data)
            })
            .ToList()
            .Select(item =>
            {
                item.User.Data = item.Data.Select(x => new TestData { Data = x }).ToList();
                return item.User;
            })
            .ToList();

        return result;
    }

    public User? GetUserById(long id)
    {
        return _context.GetById(id);
    }

    public User? GetUserByName(string name)
    {
        return _context.GetAll().FirstOrDefault(u => u.Name == name);
    }

    public async Task<User?> GetUserByData(string data)
    {
#pragma warning disable CS8604 // Possible null reference argument.
        return _context.GetAll().FirstOrDefault(u => u.Data.Any(d => d.Data == data));
#pragma warning restore CS8604 // Possible null reference argument.
    }
}