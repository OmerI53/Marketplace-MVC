using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestMVC.Models;
using TestMVC.Models.Entity;
using TestMVC.Models.Request;
using TestMVC.Services.ItemService;

namespace TestMVC.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IItemService _itemService;

    public HomeController(ILogger<HomeController> logger, IItemService itemService)
    {
        _logger = logger;
        _itemService = itemService;
    }

    public async Task<IActionResult> Index(string? searchQuery, int page = 1, int pageSize = 10)
    {
        var itemsQuery = await _itemService.GetItemsAlike(searchQuery);
        if (itemsQuery == null)
        {
            return View(new List<Item>());
        }

        var enumerable = itemsQuery.ToList();
        var paginatedItems = enumerable
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var totalItems = enumerable.Count;
        var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

        ViewBag.TotalPages = totalPages;
        ViewBag.CurrentPage = page;

        return View(paginatedItems);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}