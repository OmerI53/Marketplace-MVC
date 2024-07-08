using Microsoft.AspNetCore.Mvc;
using TestMVC.Models;
using TestMVC.Services.DataService;

namespace TestMVC.Controllers;

public class DataController : Controller
{
    private readonly IDataService _dataService;

    public DataController(IDataService dataService)
    {
        _dataService = dataService;
    }

    [HttpPost]
    public IActionResult Rand()
    {
        throw new NotImplementedException();
    }
    
    public IActionResult Index()
    {
        return View();
    }
    
    public IActionResult Create()
    {
        return View();
    }

    public IActionResult Submit(TestData data)
    {
        _dataService.SaveData(data);
        return RedirectToAction("Index");
    }
    
}