using Microsoft.AspNetCore.Mvc;
using TestMVC.Models;
using TestMVC.Services.UserItemService;

namespace TestMVC.Controllers;

public class UserItemController : Controller
{
    private readonly IUserItemService _service;

    public UserItemController(IUserItemService userItemService)
    {
        _service = userItemService;
    }

    public IActionResult Create(CreateUserItemModel request)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        var success = _service.CreateUserItem(request, userId);
        if (!success)
        {
            ModelState.AddModelError("exists","Item already exists in cart");
        }
        return RedirectToAction("Index", "User");
    }


    [HttpPost]
    public IActionResult ChangeQuantity(long itemId, bool increase)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        var result = _service.ChangeQuantity(itemId, userId, increase);
        if (result == false)
        {
            return NotFound();
        }

        return Ok();
    }
}