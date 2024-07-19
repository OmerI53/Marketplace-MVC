using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TestMVC.Models;
using TestMVC.Models.Entity;
using TestMVC.Models.Request;
using TestMVC.Services.ItemService;

namespace TestMVC.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly IItemService _itemService;
    private readonly UserManager<User> _userManager;

    public AdminController(IItemService itemService, UserManager<User> userManager)
    {
        _itemService = itemService;
        _userManager = userManager;
    }

    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> SubmitItem(Item item)
    {
        await _itemService.CreateItem(item);
        return View("Index");
    }

    [HttpPost]
    public async Task<IActionResult> EditUser(EditUserModel model)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByIdAsync(model.UserId!);
            if (user != null)
            {
                user.UserName = model.UserName;
                var role = await _userManager.GetRolesAsync(user);
                if (role.Count > 0)
                {
                    await _userManager.RemoveFromRoleAsync(user, role[0]);
                    await _userManager.AddToRoleAsync(user, model.UserRole!);
                }

                await _userManager.UpdateAsync(user);
                return Json(new { success = true });
            }
        }

        return Json(new { success = false, message = "Invalid data" });
    }
}