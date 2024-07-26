using Microsoft.EntityFrameworkCore;
using TestMVC.Models.Entity;
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

    public List<User> GetAllUsers()
    {
        return _repository.GetAll()!.ToList();
    }

    public User? GetUserById(string? id)
    {
        var set = _repository.GetSet();
        //TODO: Ask if there is a better way to do this
        var users = set.Include(u => u.UserItems)
            .ThenInclude(ui => ui.Item)
            .Where(u => u.Id == id)
            .Select(u => new User
            {
                Id = u.Id,
                Name = u.Name,
                UserItems = u.UserItems.Select(ui => new UserItem
                {
                    ItemId = ui.ItemId,
                    SellerId = ui.SellerId,
                    Quantity = ui.Quantity,
                    Price = ui.Price,
                    Item = new Item
                    {
                        Id = ui.Item!.Id,
                        ItemName = ui.Item.ItemName,
                        Description = ui.Item.Description,
                        InStock = ui.Item.InStock,
                        Category = ui.Item.Category
                    }
                }).ToList()
            }).FirstOrDefault();
        return users;
    }
    
    public User? GetBaseUserById(string? userId)
    {
        return _repository.GetById(userId);
    }

    public User? GetUserByName(string name)
    {
        return _repository.GetAll()!.FirstOrDefault(u => u!.Name == name);
    }

    public Task<User?> GetUserByData(string data)
    {
        throw new NotImplementedException();
    }

    public User? GetUserByUsername(string? username)
    {
        return _repository.FindAsync(user=>user.Name==username).Result.FirstOrDefault();
    }
}