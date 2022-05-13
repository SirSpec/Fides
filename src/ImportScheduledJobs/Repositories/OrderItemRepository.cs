using ImportScheduledJobs.Entities;
using ImportScheduledJobs.Extensions;
using Microsoft.EntityFrameworkCore;

namespace ImportScheduledJobs.Repositories;

public class OrderItemRepository : IOrderItemRepository
{
    private readonly StoresDbContext storesDbContext;

    public OrderItemRepository(StoresDbContext storesDbContext) =>
        this.storesDbContext = storesDbContext;

    public async Task<PaginatedQueryable<OrderItem>> GetSoldItemsSortedByBrandAndPriceAync(int pageSize) =>
        await storesDbContext
            .OrderItems
            .Include(orderItem => orderItem.Order)
            .ThenInclude(order => order.Customer)
            .Include(orderItem => orderItem.Product)
            .ThenInclude(product => product.Brand)
            .Include(orderItem => orderItem.Product)
            .ThenInclude(product => product.Category)
            .Where(orderItem => orderItem.Order.OrderStatus == OrderStatus.Completed)
            .OrderBy(orderItem => orderItem.Product.BrandId)
            .ThenByDescending(orderItem => orderItem.ListPrice)
            .ToPaginatedAsync(pageSize);
}
