using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestMVC.Services.ItemService;

namespace TestMVC.Controllers;

[Authorize]
public class ItemController(IItemService itemService) : Controller
{
    public async Task<IActionResult> Rand()
    {
        await itemService.GenerateRandomData();
        return RedirectToAction("Index","Admin");
    }

    public IActionResult Index()
    {
        var allData = itemService.GetAllItems();
        return View(allData);
    }
}