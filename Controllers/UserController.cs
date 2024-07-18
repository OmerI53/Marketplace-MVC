using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestMVC.Services.ItemService;
using TestMVC.Services.UserService;

namespace TestMVC.Controllers;

[Authorize]
public class UserController : Controller
{
    private readonly IUserService _userService;
    private readonly IItemService _itemService;

    public UserController(IUserService userService, IItemService itemService)
    {
        _userService = userService;
        _itemService = itemService;
    }

    public async Task<IActionResult> Index()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var user = _userService.GetUserById(userId);

        foreach (var item in user.UserItems)
        {
            item.Item = (await _itemService.GetItemById(item.ItemId)!)!;
        }

        return View(user);
    }
}