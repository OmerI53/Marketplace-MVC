using Microsoft.Data.SqlClient;
using TestMVC.Models;
using TestMVC.Repository;

namespace TestMVC.Services.UserItemService;

public class UserItemService : IUserItemService
{
    private readonly IGenericRepository<UserItem> _repository;

    // ReSharper disable once ConvertToPrimaryConstructor
    public UserItemService(IGenericRepository<UserItem> repository)
    {
        _repository = repository;
    }

    public void CreateUserItem(CreateUserItemModel request, string? userId)
    {
        var userItem = new UserItem
        {
            ItemId = request.Id,
            UserId = userId!,
            Quantity = request.Quantity,
            Price = request.Price
        };

        _repository.Insert(userItem);
    }
}