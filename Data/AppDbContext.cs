using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TestMVC.Models;

namespace TestMVC.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<AppUser>(options)
{
    public DbSet<UserItems> UserItems { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }
}