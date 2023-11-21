using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using DeliveryBot.Db.Models;

namespace DeliveryBot.Db.DbContexts;

public class ApplicationDbContext : IdentityDbContext<IdentityUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<CompanyEmployee> CompanyEmployees { get; set; }
    public DbSet<Robot> Robots { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<OrderProduct> OrderProducts { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Delivery> Deliveries { get; set; }
    public DbSet<Address> Addresses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>()
            .Property(order => order.PlacedDateTime)
            .HasColumnType("timestamp with time zone");

        modelBuilder.Entity<Delivery>()
            .Property(delivery => delivery.DeliveredDateTime)
            .HasColumnType("timestamp with time zone");

        modelBuilder.Entity<Delivery>()
            .Property(delivery => delivery.ShippedDateTime)
            .HasColumnType("timestamp with time zone");

        modelBuilder.HasPostgresEnum<OrderStatus>();
        modelBuilder.HasPostgresEnum<RobotStatus>();

        modelBuilder.HasPostgresExtension("postgis");

        base.OnModelCreating(modelBuilder);
    }
}