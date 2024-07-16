using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestMVC.Services.UserService;

namespace TestMVC.Controllers;

[Authorize]
public class UserController : Controller
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<IActionResult> Index()
    {
        var user = await _userService.GetUserById(1);
        return View(user);
    }
}