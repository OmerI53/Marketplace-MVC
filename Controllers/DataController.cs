using Microsoft.AspNetCore.Mvc;
using TestMVC.Models;
using TestMVC.Services.DataService;

namespace TestMVC.Controllers;

public class DataController(IDataService dataService) : Controller
{
    public async Task<IActionResult> Rand()
    {
        await dataService.GenerateRandomData();
        return RedirectToAction("Index");
    }

    public IActionResult Index()
    {
        var allData = dataService.GetAllData();
        return View(allData);
    }

    public IActionResult Create()
    {
        return View();
    }

    public IActionResult Submit(TestData request)
    {
        dataService.SaveData(request);
        return RedirectToAction("Index");
    }
}