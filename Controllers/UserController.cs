using Microsoft.AspNetCore.Mvc;

namespace TestMVC.Controllers;

public class UserController:Controller
{
    public IActionResult Index()
    {
        return View();
    }
    
    public IActionResult Collection()
    {
        return View();
    }
}