using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace TestMVC.Data;

public class AppUser : IdentityUser
{
    [Required]
    public string? Name { get; set; }
}