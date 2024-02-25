using Microsoft.EntityFrameworkCore;
using PizzaMauiApp.API.Orders.Models;

namespace PizzaMauiApp.API.Orders.DbOrders;

public class OrdersDbContext : DbContext
{
    public OrdersDbContext(DbContextOptions<OrdersDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<Order> Orders { get; set; }
    
    public DbSet<OrderStatusUpdate> OrdersStatusHistory { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<OrderItems>()
            .HasOne<Order>()
            .WithMany(p=>p.OrderItems)
            .HasForeignKey(p => p.OrderId)
            .IsRequired(false);
        
        modelBuilder.Entity<OrderStatusUpdate>()
            .HasOne<Order>()
            .WithMany(p=>p.OrdersStatusHistory)
            .HasForeignKey(p => p.OrderId)
            .IsRequired(false);
    }
}