using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace TestMVC.Data;

public class ApplicationUser : IdentityUser
{
    [PersonalData]
    [Column(TypeName = "nvarchar(50)")]
    public string Name { get; set; }


    [PersonalData]
    [Column(TypeName = "nvarchar(50)")]
    public string Surname { get; set; }
}