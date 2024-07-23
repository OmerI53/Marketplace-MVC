using Bogus;
using Microsoft.EntityFrameworkCore;
using TestMVC.Models.Entity;
using TestMVC.Models.Enum;
using TestMVC.Repository;

namespace TestMVC.Services.ItemService;

public class ItemService(IGenericRepository<Item> repository) : IItemService
{
    public Task<Item> CreateItem(Item request)
    {
        return repository.Insert(request);
    }

    public List<Item> GetAllItems()
    {
        return repository.GetAll()!.ToList();
    }

    public async Task GenerateRandomData()
    {
        var random = new Random();
        var vals = Enum.GetValues(typeof(Category));
        var cat = (Category)(vals.GetValue(random.Next(vals.Length)) ?? Category.Other);

        var faker = new Faker();

        var item = new Item
        {
            ItemName = GenerateItemName(faker, cat),
            Description = GenerateDescription(faker, cat),

            Category = cat
        };

        await repository.Insert(item);
    }

    public async Task<IEnumerable<Item>?> GetItemsAlike(string? searchQuery)
    {
        IEnumerable<Item>? items;
        if (searchQuery == null)
        {
            items = repository.GetAll();
        }
        else
        {
            items = await repository.FindAsync(x => x.ItemName.Contains(searchQuery));
        }

        var context = repository.GetContext();
        var itemsAlike = items!.ToList();

        foreach (var i in itemsAlike.Where(i => context.UserItems.Any(ui => ui.ItemId == i.Id)))
        {
            i.InStock = true;
        }

        return itemsAlike;
    }

    public Task<Item?> GetItemById(long id)
    {
        var set = repository.GetSet();
        var items = set.Include(u => u.UserItems)!
            .ThenInclude(ui => ui.Seller)
            .Select(i => new Item
            {
                Id = i.Id,
                ItemName = i.ItemName,
                Description = i.Description,
                Category = i.Category,
                UserItems = i.UserItems!.Select(ui => new UserItem
                {
                    ItemId = ui.ItemId,
                    SellerId = ui.SellerId,
                    Quantity = ui.Quantity,
                    Price = ui.Price,
                    Seller = new User
                    {
                        Id = ui.Seller!.Id,
                        Name = ui.Seller.Name
                    }
                }).ToList()
            }).FirstOrDefault(u => u.Id == id);
        return Task.FromResult(items);
    }

    public async Task<IEnumerable<Item>> GetItemsByCategory(string? category)
    {
        if (category == null)
        {
            return new List<Item>();
        }

        var c = Enum.Parse(typeof(Category), category);
        return await repository.FindAsync(x => x.Category.Equals(c));
    }

    public IEnumerable<Item> ApplyFilters(IEnumerable<Item> itemsQuery, Category? category, bool inStock)
    {
        if (category != null)
        {
            itemsQuery = itemsQuery.Where(i => i.Category.Equals(category)).ToList();
        }

        if (inStock)
        {
            itemsQuery = itemsQuery.Where(i => i.InStock).ToList();
        }

        return itemsQuery;
    }

    #region Generate Random Data

    private static string GenerateItemName(Faker faker, Category category)
    {
        return category switch
        {
            Category.Device => faker.Commerce.ProductName(),
            Category.Clothes => faker.Commerce.ProductMaterial(),
            Category.Food => faker.Commerce.ProductAdjective() + " Food",
            Category.Other => faker.Commerce.ProductName(),
            _ => "Unknown Item"
        };
    }

    private static string GenerateDescription(Faker faker, Category category)
    {
        return category switch
        {
            Category.Device => faker.Commerce.ProductDescription(),
            Category.Clothes => faker.Commerce.ProductDescription(),
            Category.Food => faker.Commerce.ProductDescription(),
            Category.Other => faker.Commerce.ProductDescription(),
            _ => "No Description"
        };
    }

    private static int GeneratePrice(Faker faker, Category category)
    {
        return category switch
        {
            Category.Device => faker.Random.Int(300, 2000),
            Category.Clothes => faker.Random.Int(20, 500),
            Category.Food => faker.Random.Int(5, 100),
            Category.Other => faker.Random.Int(10, 300),
            _ => faker.Random.Int(1, 1000)
        };
    }

    #endregion
}