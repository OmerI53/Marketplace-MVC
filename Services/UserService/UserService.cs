using TestMVC.Models;
using TestMVC.Repository;

namespace TestMVC.Services.UserService;

public class UserService : IUserService
{
    private readonly IGenericRepository<User> _repository;

    // ReSharper disable once ConvertToPrimaryConstructor
    public UserService(IGenericRepository<User> repository)
    {
        _repository = repository;
    }

    public async Task<User?> CreateUser(User request)
    {
        var user = new User
        {
            Name = request.Name,
            Data = new List<TestData>()
        };
        await _repository.Insert(user);
        return user;
    }

    public List<User?> GetAllUsers()
    {
        return _repository.GetAll().ToList();
    }

    public async Task<User> GetUserById(long id)
    {
        return await _repository.GetByIdWithIncludesAsync(id, u => u.Data);
    }

    public User? GetUserByName(string name)
    {
        return _repository.GetAll().FirstOrDefault(u => u.Name == name);
    }

    public Task<User?> GetUserByData(string data)
    {
#pragma warning disable CS8604 // Possible null reference argument.
        return Task.FromResult(_repository.GetAll().FirstOrDefault(u => u.Data.Any(d => d.Data == data)));
#pragma warning restore CS8604 // Possible null reference argument.
    }
}