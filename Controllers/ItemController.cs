using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestMVC.Models;
using TestMVC.Services.ItemService;

namespace TestMVC.Controllers;

[Authorize]
public class ItemController(IItemService itemService) : Controller
{
    public async Task<IActionResult> Rand()
    {
        await itemService.GenerateRandomData();
        return RedirectToAction("Index", "Admin");
    }

    public IActionResult Index()
    {
        var allData = itemService.GetAllItems();
        return View(allData);
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> Details([FromRoute] long id)
    {
        var item = await itemService.GetItemById(id);
        if (item == null)
        {
            return NotFound();
        }

        return View(item);
    }

    [HttpGet]
    public async Task<List<Item>> GetItemsByCategory([FromQuery] string? category)
    {
        var categories = await itemService.GetItemsByCategory(category);
        return categories.ToList();
    }
}