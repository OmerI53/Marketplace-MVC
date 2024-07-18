using Microsoft.AspNetCore.Mvc;
using TestMVC.Models;
using TestMVC.Services.CartService;

namespace TestMVC.Controllers;

public class CartController : Controller
{
    private readonly ICartService _cartService;

    public CartController(CartService cartService)
    {
        _cartService = cartService;
    }

    public IActionResult Index()
    {
        var cart = _cartService.GetCart(HttpContext);
        return View(cart);
    }

    [HttpPost]
    public IActionResult AddToCart(int itemId)
    {
        _cartService.AddToCart(HttpContext, itemId);
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult RemoveFromCart(int productId)
    {
        _cartService.RemoveFromCart(HttpContext, productId);
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult ClearCart()
    {
        _cartService.ClearCart(HttpContext);
        return RedirectToAction("Index");
    }

    public IActionResult Purchase()
    {
        _cartService.Purchase(HttpContext);
        return RedirectToAction("Index");
    }
}