using Newtonsoft.Json;
using TestMVC.Models;

namespace TestMVC.Services.CartService;

public class CartService : ICartService
{
    private const string CartCookieKey = "Cart";

    public List<CartItem> GetCart(HttpContext context)
    {
        var cookie = context.Request.Cookies[CartCookieKey];
        return (cookie == null ? new List<CartItem>() : JsonConvert.DeserializeObject<List<CartItem>>(cookie))!;
    }

    public void AddToCart(HttpContext context, long itemId)
    {
        var cart = GetCart(context);
        //Todo fix this
        var item = new CartItem { ItemId = itemId, Quantity = 1 };
        var existingItem = cart.FirstOrDefault(x => x.ItemId == item.ItemId);
        if (existingItem != null)
        {
            existingItem.Quantity += item.Quantity;
        }
        else
        {
            cart.Add(item);
        }

        var cookieOptions = new CookieOptions { Expires = DateTime.Now.AddMinutes(30) };
        context.Response.Cookies.Append(CartCookieKey, JsonConvert.SerializeObject(cart), cookieOptions);
    }

    public void RemoveFromCart(HttpContext context, int productId)
    {
        var cart = GetCart(context);
        var item = cart.FirstOrDefault(x => x.ItemId == productId);
        if (item != null)
        {
            cart.Remove(item);
        }

        var cookieOptions = new CookieOptions { Expires = DateTime.Now.AddMinutes(30) };
        context.Response.Cookies.Append(CartCookieKey, JsonConvert.SerializeObject(cart), cookieOptions);
    }

    public void ClearCart(HttpContext context)
    {
        context.Response.Cookies.Delete(CartCookieKey);
    }

    public void Purchase(HttpContext httpContext)
    {
        var cart = GetCart(httpContext);
        if (cart.Count == 0)
        {
            return;
        }

        //TODO logic
        ClearCart(httpContext);
    }
}