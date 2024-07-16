using TestMVC.Data;
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

    public List<User?> GetAllUsers()
    {
        return _repository.GetAll().ToList();
    }

    public async Task<User> GetUserById(long id)
    {
        return await _repository.GetByIdWithIncludesAsync(id, u => u.Items ?? new List<Item>());
    }

    public User? GetUserByName(string name)
    {
        return _repository.GetAll().FirstOrDefault(u => u!.Name == name);
    }

    public Task<User?> GetUserByData(string data)
    {
        throw new NotImplementedException();
    }
}