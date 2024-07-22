using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace TestMVC.Models.Entity;

public class User : IdentityUser
{
    [PersonalData]
    [Column(TypeName = "nvarchar(50)")]
    public string? Name { get; set; }

    [PersonalData]
    [Column(TypeName = "nvarchar(50)")]
    public string? Surname { get; set; }

    public ICollection<UserItem> UserItems { get; set; } = new List<UserItem>();

    // ReSharper disable once CollectionNeverUpdated.Global
    public ICollection<PurchasedItem> Purchases { get; set; } = new List<PurchasedItem>();
}