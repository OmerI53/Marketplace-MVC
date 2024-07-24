using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestMVC.Models.Entity;
using TestMVC.Models.Enum;
using TestMVC.Models.Request;
using TestMVC.Services.ItemService;

namespace TestMVC.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly IItemService _itemService;

    /// <inheritdoc />
    public HomeController(IItemService itemService)
    {
        _itemService = itemService;
    }

    public async Task<IActionResult> Index(string? searchQuery, Category? category, bool inStock, int page = 1,
        int pageSize = 10)
    {
        ViewBag.SearchQuery = searchQuery!;
        ViewBag.Category = category?.ToString()!;
        ViewBag.InStock = inStock;
        
        var itemsQuery = await _itemService.GetItemsAlike(searchQuery);
        if (itemsQuery == null)
        {
            return View(new List<Item>());
        }

        var query = itemsQuery.ToList();
        var filtered = _itemService.ApplyFilters(query, category, inStock);

        var enumerable = filtered.ToList();
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