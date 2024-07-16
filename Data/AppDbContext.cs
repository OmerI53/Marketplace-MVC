using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TestMVC.Models;

namespace TestMVC.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options)
    : IdentityDbContext<User>(options)
{
    public DbSet<Item> Items { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<User>()
            .HasMany(u => u.Items)
            .WithMany(i => i.Users)
            .UsingEntity(j => j.ToTable("UserItems"));
    }
}