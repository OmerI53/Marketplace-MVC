using Microsoft.EntityFrameworkCore;
using TestMVC.Models;

namespace TestMVC.Repository;

public class Context(DbContextOptions<Context> options) : DbContext(options)
{
    public DbSet<UserItems> Data { get; init; }
    public DbSet<User> Users { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }
}