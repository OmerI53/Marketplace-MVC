using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestMVC.Models;
using TestMVC.Services.ItemService;

namespace TestMVC.Controllers;

[Authorize]
public class DataController(IItemService itemService) : Controller
{
    public async Task<IActionResult> Rand()
    {
        await itemService.GenerateRandomData();
        return RedirectToAction("Index");
    }

    public IActionResult Index()
    {
        var allData = itemService.GetAllData();
        return View(allData);
    }

    public IActionResult Create()
    {
        return View();
    }

    public IActionResult Submit(UserItems request)
    {
        itemService.SaveData(request);
        return RedirectToAction("Index");
    }
}