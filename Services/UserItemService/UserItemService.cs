using System.Globalization;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using TestMVC.Models.Entity;
using TestMVC.Models.Request;
using TestMVC.Repository;

namespace TestMVC.Services.UserItemService;

public class UserItemService : IUserItemService
{
    private readonly IGenericRepository<UserItem> _repository;

    // ReSharper disable once ConvertToPrimaryConstructor
    public UserItemService(IGenericRepository<UserItem> repository)
    {
        _repository = repository;
    }

    public bool CreateUserItem(CreateUserItemModel request, string? userId)
    {
        var exists = _repository.FindAsync(item => item.ItemId == request.Id && item.SellerId == userId).Result
            .FirstOrDefault();
        if (exists != null)
        {
            return false;
        }

        var price = request.Price.Replace(".", CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator);
        var userItem = new UserItem
        {
            ItemId = request.Id,
            SellerId = userId!,
            Quantity = request.Quantity,
            Price = float.Parse(request.Price, CultureInfo.InvariantCulture),
        };

        _repository.Insert(userItem);
        return true;
    }

    public bool ChangeQuantity(long itemId, string? userId, bool increase)
    {
        var userItem = _repository.FindAsync(x => x.ItemId == itemId && x.SellerId == userId).Result.FirstOrDefault();

        if (userItem == null)
        {
            return false;
        }

        if (increase)
        {
            userItem.Quantity++;
        }
        else
        {
            userItem.Quantity--;
            if (userItem.Quantity <= 0)
            {
                _repository.Delete(userItem);
                return true;
            }
        }

        _repository.Update(userItem);
        return true;
    }

    public bool ChangeQuantity(long itemId, string? userId, int change)
    {
        var userItem = _repository.FindAsync(x => x.ItemId == itemId && x.SellerId == userId).Result.FirstOrDefault();

        if (userItem == null)
        {
            return false;
        }

        userItem.Quantity += change;

        switch (userItem.Quantity)
        {
            case < 0:
                throw new Exception("Quantity cannot be negative");
            case 0:
                _repository.Delete(userItem);
                return true;
            default:
                _repository.Update(userItem);
                return true;
        }
    }

    public bool DeleteUserItem(long itemId, string? userId)
    {
        var userItem = _repository.FindAsync(x => x.ItemId == itemId && x.SellerId == userId).Result.FirstOrDefault();
        if (userItem == null)
        {
            return false;
        }

        _repository.Delete(userItem);
        return true;
    }

    public UserItem? GetUserItemById(long itemId, string sellerId)
    {
        const string sql = "SELECT * FROM UserItems WHERE ItemId = @itemId AND SellerId = @sellerId";

        object[] parameters =
        [
            new MySqlParameter("@itemId", itemId),
            new MySqlParameter("@sellerId", sellerId)
        ];

        return _repository.ExecuteRawSql(sql, parameters).FirstOrDefault();
    }

    public UserItem? GetByIdF(long itemId, string sellerId)
    {
        var item = _repository.GetSet()
            .Include(ui => ui.Item)
            .Include(ui => ui.Seller)
            .Where(ui => ui.ItemId == itemId && ui.SellerId == sellerId)
            .Select(ui => new UserItem
            {
                ItemId = ui.ItemId,
                Item = new Item
                {
                    ItemName = ui.Item!.ItemName
                },
                Seller = new User
                {
                    UserName = ui.Seller!.UserName
                },
                SellerId = ui.SellerId,
                Quantity = ui.Quantity,
                Price = ui.Price
            }).FirstOrDefault();

        return item;
    }

    public int GetQuantity(long itemId, string sellerId)
    {
        var result = GetUserItemById(itemId, sellerId);
        if (result == null)
        {
            return -1;
        }

        return result.Quantity;
    }

    public void UpdateUserItem(UpdateUserItem item)
    {
        var userItem = GetUserItemById(long.Parse(item.ItemId), item.SellerId);
        if (userItem == null) return;
        if (item.Quantity != null)
        {
            userItem.Quantity = int.Parse(item.Quantity);
        }

        if (item.Price != null)
        {
            userItem.Price = float.Parse(item.Price, CultureInfo.InvariantCulture);
        }

        _repository.Update(userItem);
    }
}