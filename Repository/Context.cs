using Microsoft.EntityFrameworkCore;
using TestMVC.Models;

namespace TestMVC.Repository;

public class Context(DbContextOptions<Context> options): DbContext(options)
{
    public DbSet<TestData> Data { get; init; }
    public DbSet<User> Users { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("users");

            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasColumnName("name")
                .HasMaxLength(256)
                .IsRequired(false);

            entity.HasMany(e => e.Data)
                .WithOne(d => d.User)
                .HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<TestData>(entity =>
        {
            entity.ToTable("data");

            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)
                .HasColumnName("id");
            entity
                .Property(e => e.Data)
                .HasColumnName("data")
                .HasMaxLength(256)
                .IsRequired();
            entity
                .Property(e => e.UserId)
                .HasColumnName("userId");

            entity.HasOne(d => d.User)
                .WithMany(u => u.Data)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("fk_user");
        });
    }
}