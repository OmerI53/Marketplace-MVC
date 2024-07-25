using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestMVC.Filters;
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
    [ModelFilter]
    public IActionResult Create(CreateUserItemModel request)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        var success = _service.CreateUserItem(request, userId);
        if (!success)
        {
            TempData["ErrorMessage"] = "Item is already in collection\n.";
        }

        return RedirectToAction("Index", "User");
    }

    [Route("Edit")]
    [HttpGet]
    public IActionResult Edit([FromQuery] string itemId, [FromQuery] string sellerId)
    {
        var item = _service.GetByIdF(long.Parse(itemId), sellerId);
        if (item == null)
        {
            return NotFound();
        }

        return PartialView("_EditItem", item);
    }

    [Route("Save")]
    [HttpPost]
    [ModelFilter]
    public IActionResult Save(UpdateUserItem request)
    {
        _service.UpdateUserItem(request);
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

    [Route("DeleteUserItem")]
    [HttpPost]
    public IActionResult DeleteUserItem(DeleteUserItem request)
    {
        //var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        var result = _service.DeleteUserItem(long.Parse(request.ItemId), request.SellerId);
        if (result == false)
        {
            return NotFound();
        }

        return RedirectToAction("Index", "User");
    }
}