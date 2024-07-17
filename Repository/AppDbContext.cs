using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TestMVC.Data;
using TestMVC.Models;

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
            .HasKey(ui => new { ui.ItemId, ui.UserId });

        modelBuilder.Entity<UserItem>()
            .HasOne(ui => ui.Item)
            .WithMany(i => i.UserItems)
            .HasForeignKey(ui => ui.ItemId);

        modelBuilder.Entity<UserItem>()
            .HasOne(ui => ui.User)
            .WithMany(u => u.UserItems)
            .HasForeignKey(ui => ui.UserId);
    }
}