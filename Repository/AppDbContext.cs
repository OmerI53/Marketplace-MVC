using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TestMVC.Models;
using TestMVC.Models.Entity;

namespace TestMVC.Repository;

public class AppDbContext(DbContextOptions<AppDbContext> options)
    : IdentityDbContext<User>(options)
{
    public DbSet<Item> Items { get; set; }
    public DbSet<UserItem> UserItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<UserItem>()
            .HasKey(ui => new { ui.ItemId, UserId = ui.SellerId });

        modelBuilder.Entity<UserItem>()
            .HasOne(ui => ui.Item)
            .WithMany(i => i.UserItems)
            .HasForeignKey(ui => ui.ItemId);

        modelBuilder.Entity<UserItem>()
            .HasOne(ui => ui.Seller)
            .WithMany(u => u.UserItems)
            .HasForeignKey(ui => ui.SellerId);
    }
}