using Newtonsoft.Json;
using TestMVC.Models;
using TestMVC.Services.UserItemService;

namespace TestMVC.Services.CartService;

public class CartService : ICartService
{
    private readonly IUserItemService _userItemService;

    public CartService(IUserItemService userItemService)
    {
        _userItemService = userItemService;
    }

    public void Purchase(List<CartItem> cart)
    {
        ValidatePurchaseCount(cart);
        foreach (var item in cart)
        {
            for (var i = 0; i < item.Quantity; i++)
            {
                var success = _userItemService.ChangeQuantity(item.ItemId, item.SellerId,false);
                if (!success)
                {
                    throw new Exception("Failed to purchase item");
                }
            }
        }
    }

    private void ValidatePurchaseCount(List<CartItem> cart)
    {
        if ((from item in cart let itemQuantity = _userItemService.GetQuantity(item.ItemId,item.SellerId) where item.Quantity > itemQuantity select item).Any())
        {
            throw new Exception("Not enough items in stock");
        }
    }
}