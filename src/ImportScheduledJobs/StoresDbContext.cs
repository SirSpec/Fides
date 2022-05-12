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
        modelBuilder.Entity<Order>(entity =>
        {
            entity
                .HasOne(entity => entity.Staff)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);

            entity
                .HasIndex(entity => entity.StaffId)
                .IsUnique(false);
        });

        modelBuilder
            .Entity<Stock>()
            .HasKey(entity => new { entity.StoreId, entity.ProductId });

        modelBuilder
            .Entity<OrderItem>()
            .HasKey(entity => new { entity.OrderId, entity.ItemId });

        modelBuilder.Entity<Staff>(entity =>
        {
            entity
                .HasIndex(entity => entity.Email)
                .IsUnique();

            entity
                .HasIndex(entity => entity.ManagerId)
                .IsUnique(false);

            entity
                .HasOne(entity => entity.Manager)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);
        });
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