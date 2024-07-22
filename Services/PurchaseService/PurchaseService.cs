using Microsoft.EntityFrameworkCore;
using TestMVC.Models;
using TestMVC.Models.Entity;
using TestMVC.Repository;

namespace TestMVC.Services.PurchaseService;

public class PurchaseService : IPurchaseService
{
    private readonly IGenericRepository<PurchasedItem> _repository;

    public PurchaseService(IGenericRepository<PurchasedItem> repository)
    {
        _repository = repository;
    }

    public async Task<PurchasedItem> CreatePurchase(User buyer, CartItem item)
    {
        var purchase = new PurchasedItem
        {
            BuyerId = buyer.Id,
            Buyer = buyer,
            SellerId = item.SellerId,
            ItemId = item.ItemId,
            PurchaseDate = DateTime.Today,
            Quantity = item.Quantity,
            PerItemPrice = item.Price
        };
        var saved = await _repository.Insert(purchase);
        return saved;
    }

    public IEnumerable<PurchasedItem> GetPurchasesByUserId(string? userId)
    {
        var purchases = _repository.GetSet()
            .Include(pi => pi.Item)
            .Include(pi => pi.Buyer)
            .Where(pi => pi.BuyerId == userId)
            .Select(pi => new PurchasedItem
            {
                Id = pi.Id,
                BuyerId = pi.BuyerId,
                Buyer = pi.Buyer,
                ItemId = pi.ItemId,
                Item = new Item
                {
                    Id = pi.Item!.Id,
                    ItemName = pi.Item.ItemName,
                    Description = pi.Item.Description,
                    Category = pi.Item.Category,
                },
                SellerId = pi.SellerId,
                PurchaseDate = pi.PurchaseDate,
                Quantity = pi.Quantity,
                PerItemPrice = pi.PerItemPrice
            }).ToList();
        return purchases;
    }
}