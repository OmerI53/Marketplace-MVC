using TestMVC.Models;
using TestMVC.Models.Entity;

namespace TestMVC.Services.CartService;

public interface ICartService
{
    Task Purchase(List<CartItem> cart, User userId);
}