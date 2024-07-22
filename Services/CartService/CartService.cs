using TestMVC.Models;
using TestMVC.Models.Entity;
using TestMVC.Services.PurchaseService;
using TestMVC.Services.UserItemService;

namespace TestMVC.Services.CartService;

public class CartService : ICartService
{
    private readonly IUserItemService _userItemService;
    private readonly IPurchaseService _purchaseService;

    public CartService(IUserItemService userItemService, IPurchaseService purchaseService)
    {
        _userItemService = userItemService;
        _purchaseService = purchaseService;
    }

    public async Task Purchase(List<CartItem> cart, User user)
    {
        ValidatePurchaseCount(cart);
        foreach (var item in cart)
        {
            var success = _userItemService.ChangeQuantity(item.ItemId, item.SellerId, -item.Quantity);
            if (success)
            { 
                await _purchaseService.CreatePurchase(user,item);
            }
            else
            {
                throw new Exception("Failed to purchase item");
            }
        }
    }

    private void ValidatePurchaseCount(List<CartItem> cart)
    {
        if ((from item in cart
                let itemQuantity = _userItemService.GetQuantity(item.ItemId, item.SellerId)
                where item.Quantity > itemQuantity
                select item).Any())
        {
            throw new Exception("Not enough items in stock");
        }
    }
}