using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TestMVC.Models;
using TestMVC.Models.Request;
using TestMVC.Services.CartService;

namespace TestMVC.Controllers;

public class CartController : Controller
{
    private readonly ICartService _cartService;
    private const string CartCookieKey = "Cart";

    public CartController(ICartService cartService)
    {
        _cartService = cartService;
    }

    public IActionResult Index()
    {
        var cart = GetCart(HttpContext);
        return View(cart);
    }

    [HttpPost]
    public IActionResult AddToCart([FromBody] CreateCartItem? request)
    {
        var cart = GetCart(HttpContext);

        if (request == null)
        {
            TempData["ErrorMessage"] = "Error occurred while adding item to cart.\nPlease try again.";
            return NotFound();
        }


        CartItem? existingItem = null;
        if (cart.Count > 0)
        {
            existingItem = cart.FirstOrDefault(x => x.ItemId == request.ItemId && x.SellerId == request.SellerId);
        }

        if (existingItem != null)
        {
            existingItem.Quantity += request.Quantity;
        }
        else
        {
            var item = new CartItem
            {
                ItemId = request.ItemId,
                SellerId = request.SellerId,
                ItemName = request.ItemName,
                Price = Math.Round(double.Parse(request.Price), 2),
                Quantity = request.Quantity
            };
            cart.Add(item);
        }

        var cookieOptions = new CookieOptions { Expires = DateTime.Now.AddMinutes(30) };
        HttpContext.Response.Cookies.Append(CartCookieKey, JsonConvert.SerializeObject(cart), cookieOptions);
        return RedirectToAction("Details", "Item");
    }

    [HttpPost]
    public IActionResult RemoveFromCart(int itemId, string userId)
    {
        var cart = GetCart(HttpContext);
        var item = cart.FirstOrDefault(x => x.ItemId == itemId && x.SellerId == userId);
        if (item != null)
        {
            cart.Remove(item);
        }

        HttpContext.Response.Cookies.Delete(CartCookieKey);
        var cookieOptions = new CookieOptions { Expires = DateTime.Now.AddMinutes(30) };
        HttpContext.Response.Cookies.Append(CartCookieKey, JsonConvert.SerializeObject(cart), cookieOptions);
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult ClearCart()
    {
        HttpContext.Response.Cookies.Delete(CartCookieKey);
        return RedirectToAction("Index");
    }

    public IActionResult Purchase()
    {
        var cart = GetCart(HttpContext);
        try
        {
            _cartService.Purchase(cart);
        }
        catch (Exception e)
        {
            TempData["ErrorMessage"] = e.Message;
        }

        return RedirectToAction("Index");
    }

    private static List<CartItem> GetCart(HttpContext context)
    {
        var cookie = context.Request.Cookies[CartCookieKey];
        if (cookie is not (null or "[null]")) return JsonConvert.DeserializeObject<List<CartItem>>(cookie)!;

        //A null item is Added to the card
        context.Response.Cookies.Delete(CartCookieKey);
        context.Response.Cookies.Append(CartCookieKey, "[]");
        return [];
    }
}