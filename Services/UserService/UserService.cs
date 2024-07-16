using TestMVC.Data;
using TestMVC.Models;
using TestMVC.Repository;

namespace TestMVC.Services.UserService;

public class UserService : IUserService
{
    private readonly IGenericRepository<ApplicationUser> _repository;

    // ReSharper disable once ConvertToPrimaryConstructor
    public UserService(IGenericRepository<ApplicationUser> repository)
    {
        _repository = repository;
    }

    public List<ApplicationUser?> GetAllUsers()
    {
        return _repository.GetAll().ToList();
    }

    public async Task<ApplicationUser> GetUserById(long id)
    {
        return await _repository.GetByIdWithIncludesAsync(id, u => u.Items ?? new List<Item>());
    }

    public ApplicationUser? GetUserByName(string name)
    {
        return _repository.GetAll().FirstOrDefault(u => u!.Name == name);
    }

    public Task<ApplicationUser?> GetUserByData(string data)
    {
        throw new NotImplementedException();
    }
}