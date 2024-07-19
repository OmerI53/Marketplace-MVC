using System.Globalization;
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

    public int GetQuantity(long itemItemId)
    {
        return _repository.GetById(itemItemId)!.Quantity;
    }
}