using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestMVC.Models.Request;
using TestMVC.Services.UserItemService;

namespace TestMVC.Controllers;

[Authorize]
[Route("UserItem")]
public class UserItemController : Controller
{
    private readonly IUserItemService _service;

    /// <inheritdoc />
    public UserItemController(IUserItemService userItemService)
    {
        _service = userItemService;
    }

    [Route("Create")]
    [HttpPost]
    public IActionResult Create(CreateUserItemModel request)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        var success = _service.CreateUserItem(request, userId);
        if (!success)
        {
            TempData["ErrorMessage"] = "Item is already in collection,\nincrease/decrease quantity from the menu.";
        }

        return RedirectToAction("Index", "User");
    }

    [Route("ChangeQuantity")]
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

    [Route("{itemId:long}")]
    [HttpDelete]
    public IActionResult DeleteUserItem([FromRoute] long itemId)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        var result = _service.DeleteUserItem(itemId, userId);
        if (result == false)
        {
            return NotFound();
        }

        return Ok();
    }
}