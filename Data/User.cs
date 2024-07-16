using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using TestMVC.Models;

namespace TestMVC.Data;

public class User : IdentityUser
{
    
    
    [PersonalData]
    [Column(TypeName = "nvarchar(50)")]
    public required string Name { get; set; }


    [PersonalData]
    [Column(TypeName = "nvarchar(50)")]
    public required string Surname { get; set; }
    
    // ReSharper disable once PropertyCanBeMadeInitOnly.Global
    public ICollection<Item>? Items { get; set; }
}