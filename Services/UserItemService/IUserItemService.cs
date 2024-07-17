using TestMVC.Models;

namespace TestMVC.Services.UserItemService;

public interface IUserItemService
{
    void CreateUserItem(CreateUserItemModel itemId, string? userId);
}