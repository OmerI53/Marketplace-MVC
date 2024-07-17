using TestMVC.Models;

namespace TestMVC.Services.UserItemService;

public interface IUserItemService
{
    bool CreateUserItem(CreateUserItemModel itemId, string? userId);
    bool ChangeQuantity(long itemId, string? userId, bool increase);
}