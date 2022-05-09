using ImportScheduledJobs.Entities;
using Microsoft.EntityFrameworkCore;

namespace ImportScheduledJobs;

public class StoresDbContext : DbContext
{
    public StoresDbContext(DbContextOptions<StoresDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<Order>()
            .Property(entity => entity.OrderStatus)
            .HasConversion<int>();

        modelBuilder
            .Entity<Order>()
            .HasOne(entity => entity.Staff)
            .WithOne()
            .HasForeignKey<Order>(entity => entity.StaffId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder
            .Entity<Order>()
            .HasIndex(entity => entity.StaffId)
            .IsUnique(false);

        modelBuilder
            .Entity<Stock>()
            .HasKey(entity => new { entity.StoreId, entity.ProductId });

        modelBuilder
            .Entity<OrderItem>()
            .HasKey(entity => new { entity.OrderId, entity.ItemId });

        modelBuilder
            .Entity<Staff>()
            .HasIndex(entity => entity.Email)
            .IsUnique();

        modelBuilder
            .Entity<Staff>()
            .HasOne(entity => entity.Manager)
            .WithOne()
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder
            .Entity<Staff>()
            .HasIndex(entity => entity.ManagerId)
            .IsUnique(false);
    }

    public DbSet<Brand> Brands => Set<Brand>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Staff> Staffs => Set<Staff>();
    public DbSet<Stock> Stocks => Set<Stock>();
    public DbSet<Store> Stores => Set<Store>();
}