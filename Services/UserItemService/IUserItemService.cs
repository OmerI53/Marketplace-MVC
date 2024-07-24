using TestMVC.Models;
using TestMVC.Models.Entity;
using TestMVC.Models.Request;

namespace TestMVC.Services.UserItemService;

public interface IUserItemService
{
    bool CreateUserItem(CreateUserItemModel itemId, string? userId);
    bool ChangeQuantity(long itemId, string? userId, bool increase);
    bool ChangeQuantity(long itemId, string? userId, int change);
    bool DeleteUserItem(long itemId, string? userId);
    UserItem? GetUserItemById(long itemId, string sellerId);
    UserItem? GetByIdF(long itemId, string sellerId);
    int GetQuantity(long itemId, string sellerId);
    void UpdateUserItem(UpdateUserItem item);
}