using TestMVC.Models;

namespace TestMVC.Services.CartService;

public interface ICartService
{
    List<CartItem> GetCart(HttpContext context);
    void AddToCart(HttpContext context, long itemId);
    void RemoveFromCart(HttpContext context, int productId);
    void ClearCart(HttpContext context);
    void Purchase(HttpContext httpContext);
}