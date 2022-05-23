using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SharedKernel.Extensions;
using SharedKernel.QueryObjects.Models;
using SyncFunction.Entities;
using SyncFunction.QueryObjects;
using System.Linq.Expressions;

namespace SyncFunction.Repositories;

public class OrderItemRepository : IOrderItemRepository
{
    private readonly ILogger<OrderItemRepository> _logger;
    private readonly StoresDbContext _storesDbContext;

    public OrderItemRepository(ILogger<OrderItemRepository> logger, StoresDbContext storesDbContext)
    {
        _logger = logger;
        _storesDbContext = storesDbContext;
    }

    public async Task<PaginatedQueryable<OrderItem>> GetOrdersSortedByBrandAndPriceAync(
        int pageSize,
        IQueryObject<OrderItem, bool> where)
    {
        try
        {
            return await _storesDbContext
                .OrderItems
                .AsNoTracking()
                .Include(orderItem => orderItem.Order)
                .ThenInclude(order => order.Customer)
                .Include(orderItem => orderItem.Product)
                .ThenInclude(product => product.Brand)
                .Include(orderItem => orderItem.Product)
                .ThenInclude(product => product.Category)
                .AsNoTracking()
                .Where(where.Query)
                .OrderBy(orderItem => orderItem.Product.BrandId)
                .ThenByDescending(orderItem => orderItem.ListPrice)
                .ToPaginatedAsync(pageSize);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Unexpected error.");
            return PaginatedQueryable<OrderItem>.Empty();
        }
    }

    public async Task<IEnumerable<OrderItem>> GetExistingOrderItemIds(
        IEnumerable<OrderedItem> orderedItems,
        IQueryObject<OrderItem, bool> where)
    {
        try
        {
            Expression<Func<OrderItem, bool>> predicate = _ => false;
            foreach (var id in orderedItems.Select(orderedItem => orderedItem.Id))
                predicate = PredicateBuilderExtensions.Or(
                    predicate,
                    orderItem => orderItem.OrderId == id.OrderId && orderItem.ItemId == id.ItemId);

            var whereExpression = PredicateBuilderExtensions.And(where.Query, predicate);

            return await _storesDbContext
                .OrderItems
                .AsNoTracking()
                .Where(whereExpression)
                .ToListAsync();
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Unexpected error.");
            return Enumerable.Empty<OrderItem>();
        }
    }
}
