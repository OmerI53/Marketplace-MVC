using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
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

    public User? GetUserById(string? id)
    {
        var set = _repository.GetSet();
        //TODO: Ask if there is a better way to do this
        var users = set.Include(u => u.UserItems)
            .ThenInclude(ui => ui.Item)
            .Select(u => new User
            {
                Id = u.Id,
                Name = u.Name,
                UserItems = u.UserItems.Select(ui => new UserItem
                {
                    ItemId = ui.ItemId,
                    UserId = ui.UserId,
                    Quantity = ui.Quantity,
                    Price = ui.Price,
                    Item = new Item
                    {
                        Id = ui.Item.Id,
                        ItemName = ui.Item.ItemName,
                        Description = ui.Item.Description,
                        InStock = ui.Item.InStock,
                        Category = ui.Item.Category
                    }
                }).ToList()
            }).FirstOrDefault(u => u.Id == id);
        return users;
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