using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using TestMVC.Models;

namespace TestMVC.Data;

public class User : IdentityUser
{
    [PersonalData]
    [Column(TypeName = "nvarchar(50)")]
    public string Name { get; set; }

    [PersonalData]
    [Column(TypeName = "nvarchar(50)")]
    public string Surname { get; set; }

    public ICollection<UserItem> UserItems { get; set; } = new List<UserItem>();
}