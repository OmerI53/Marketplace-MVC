using TestMVC.Models;
using TestMVC.Models.Entity;

namespace TestMVC.Services.PurchaseService;

public interface IPurchaseService
{
    Task<PurchasedItem> CreatePurchase(User buyerId, CartItem item);
    IEnumerable<PurchasedItem> GetPurchasesByUserId(string? userId);
}