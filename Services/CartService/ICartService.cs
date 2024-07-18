using TestMVC.Models;

namespace TestMVC.Services.CartService;

public interface ICartService
{
    void Purchase(List<CartItem> cart);
}