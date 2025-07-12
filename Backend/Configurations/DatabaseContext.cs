using Microsoft.EntityFrameworkCore;
using Backend.Models.Entities;

namespace Backend.Configurations;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuração do User
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        // Configuração do Product
        modelBuilder.Entity<Product>()
            .Property(p => p.Price)
            .HasPrecision(10, 2);

        // Configuração do Order
        modelBuilder.Entity<Order>()
            .HasOne(o => o.User)
            .WithMany(u => u.Orders)
            .HasForeignKey(o => o.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Order>()
            .Property(o => o.TotalAmount)
            .HasPrecision(10, 2);

        // Configuração do OrderItem
        modelBuilder.Entity<OrderItem>()
            .HasOne(i => i.Order)
            .WithMany(o => o.Items)
            .HasForeignKey(i => i.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<OrderItem>()
            .HasOne(i => i.Product)
            .WithMany()
            .HasForeignKey(i => i.ProductId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        modelBuilder.Entity<OrderItem>()
            .Property(i => i.UnitPrice)
            .HasPrecision(10, 2);

        modelBuilder.Entity<OrderItem>()
            .Property(i => i.TotalPrice)
            .HasPrecision(10, 2);
    }
} 