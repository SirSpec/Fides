using ImportScheduledJobs.Entities;
using ImportScheduledJobs.Extensions;
using ImportScheduledJobs.QueryObjects;
using Microsoft.EntityFrameworkCore;

namespace ImportScheduledJobs.Repositories;

public class OrderItemRepository : IOrderItemRepository
{
    private readonly StoresDbContext _storesDbContext;

    public OrderItemRepository(StoresDbContext storesDbContext) =>
        _storesDbContext = storesDbContext;

    public async Task<PaginatedQueryable<OrderItem>> GetOrdersSortedByBrandAndPriceAync(
        int pageSize,
        IQueryObject<OrderItem, bool> where) =>
            await _storesDbContext
                .OrderItems
                .AsNoTracking()
                .Include(orderItem => orderItem.Order)
                .ThenInclude(order => order.Customer)
                .Include(orderItem => orderItem.Product)
                .ThenInclude(product => product.Brand)
                .Include(orderItem => orderItem.Product)
                .ThenInclude(product => product.Category)
                .Where(where.Query)
                .OrderBy(orderItem => orderItem.Product.BrandId)
                .ThenByDescending(orderItem => orderItem.ListPrice)
                .ToPaginatedAsync(pageSize);
}
