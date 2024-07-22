using TestMVC.Models;
using TestMVC.Models.Request;

namespace TestMVC.Services.UserItemService;

public interface IUserItemService
{
    bool CreateUserItem(CreateUserItemModel itemId, string? userId);
    bool ChangeQuantity(long itemId, string? userId, bool increase);
    bool ChangeQuantity(long itemId, string? userId, int change);
    bool DeleteUserItem(long itemId, string? userId);
    int GetQuantity(long itemId, string sellerId);
}