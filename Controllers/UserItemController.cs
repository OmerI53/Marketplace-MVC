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
        _service.CreateUserItem(request, userId);
        return RedirectToAction("Index", "Home");
    }
}