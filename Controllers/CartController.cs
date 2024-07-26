using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TestMVC.Filters;
using TestMVC.Models;
using TestMVC.Models.Request;
using TestMVC.Services.CartService;
using TestMVC.Services.UserService;

namespace TestMVC.Controllers;

public class CartController : Controller
{
    private readonly ICartService _cartService;
    private readonly IUserService _userService;
    private const string CartCookieKey = "Cart";

    public CartController(ICartService cartService, IUserService userService)
    {
        _cartService = cartService;
        _userService = userService;
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

        HttpContext.Response.Cookies.Append(CartCookieKey, JsonConvert.SerializeObject(cart), GetCookieOptions());
        ManageCartCount(1);
        TempData["InfoMessage"] = "Item added to cart";
        return Json(new { success = true });
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
        HttpContext.Response.Cookies.Append(CartCookieKey, JsonConvert.SerializeObject(cart), GetCookieOptions());
        ManageCartCount(-item!.Quantity);

        TempData["InfoMessage"] = "Item removed from cart";
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult ClearCart()
    {
        HttpContext.Response.Cookies.Delete(CartCookieKey);
        HttpContext.Response.Cookies.Delete("CartCount");
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Purchase()
    {
        var cart = GetCart(HttpContext);
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = _userService.GetBaseUserById(userId);
            if (user == null)
            {
                TempData["ErrorMessage"] = "User not found";
                return RedirectToAction("Index");
            }

            await _cartService.Purchase(cart, user);
        }
        catch (Exception e)
        {
            return RedirectToAction("Index");
        }

        return ClearCart();
    }

    private static List<CartItem> GetCart(HttpContext context)
    {
        var cookie = context.Request.Cookies[CartCookieKey];
        if (cookie is not (null or "[null]")) return JsonConvert.DeserializeObject<List<CartItem>>(cookie)!;

        context.Response.Cookies.Delete(CartCookieKey);
        context.Response.Cookies.Append(CartCookieKey, "[]");
        return [];
    }

    private void ManageCartCount(int itemQuantity)
    {
        var count = HttpContext.Request.Cookies["CartCount"];

        if (count != null)
        {
            var newCount = Math.Max(int.Parse(count) + itemQuantity, 0);

            HttpContext.Response.Cookies.Delete("CartCount");
            HttpContext.Response.Cookies.Append("CartCount", newCount.ToString(),
                GetCookieOptions());
        }
        else
        {
            HttpContext.Response.Cookies.Append("CartCount", itemQuantity.ToString(), GetCookieOptions());
        }
    }

    private static CookieOptions GetCookieOptions()
    {
        return new CookieOptions { Expires = DateTime.Now.AddMinutes(30) };
    }
}