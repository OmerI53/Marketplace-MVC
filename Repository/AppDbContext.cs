using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TestMVC.Models.Entity;

namespace TestMVC.Repository;

public class AppDbContext(DbContextOptions<AppDbContext> options)
    : IdentityDbContext<User>(options)
{
    public DbSet<Item> Items { get; set; }
    public DbSet<UserItem> UserItems { get; set; }
    public DbSet<PurchasedItem> PurchasedItems { get; set; }
    public DbSet<Notification> Notifications { get; set; }

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

        modelBuilder.Entity<PurchasedItem>()
            .HasOne(pi => pi.Item)
            .WithMany(i => i.PurchasedItems)
            .HasForeignKey(pi => pi.ItemId);

        modelBuilder.Entity<PurchasedItem>()
            .HasOne(pi => pi.Buyer)
            .WithMany(b => b.Purchases)
            .HasForeignKey(pi => pi.BuyerId);

        modelBuilder.Entity<Notification>()
            .HasKey(n=>n.Id);
        
        modelBuilder.Entity<Notification>()
            .HasOne(n => n.Receiver)
            .WithMany(u => u.Notifications)
            .HasForeignKey(n=>n.ReceiverId);
    }
}