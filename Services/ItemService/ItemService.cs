using Bogus;
using TestMVC.Models;
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
        var cat = (Category)vals.GetValue(random.Next(vals.Length));

        var faker = new Faker();

        var item = new Item
        {
            ItemName = GenerateItemName(faker, cat),
            Description = GenerateDescription(faker, cat),
            ItemPrice = GeneratePrice(faker, cat),
            Category = cat
        };

        await repository.Insert(item);
    }

    public async Task<IEnumerable<Item>?> GetItemsAlike(string? searchQuery)
    {
        if (searchQuery == null)
        {
            return repository.GetAll();
        }

        return await repository.FindAsync(x => x.ItemName.Contains(searchQuery));
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