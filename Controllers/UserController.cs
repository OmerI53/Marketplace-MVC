using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestMVC.Services.ItemService;
using TestMVC.Services.PurchaseService;
using TestMVC.Services.UserService;

namespace TestMVC.Controllers;

[Authorize]
public class UserController : Controller
{
    private readonly IUserService _userService;
    private readonly IItemService _itemService;
    private readonly IPurchaseService _purchaseService;

    /// <inheritdoc />
    public UserController(IUserService userService, IItemService itemService, IPurchaseService purchaseService)
    {
        _userService = userService;
        _itemService = itemService;
        _purchaseService = purchaseService;
    }

    public async Task<IActionResult> Index()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var user = _userService.GetUserById(userId);

        foreach (var item in user!.UserItems)
        {
            item.Item = (await _itemService.GetItemById(item.ItemId)!)!;
        }

        user.Purchases = _purchaseService.GetPurchasesByUserId(userId).ToList();
        return View(user);
    }
}