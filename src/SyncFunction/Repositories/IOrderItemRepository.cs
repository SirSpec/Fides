using SharedKernel.Extensions;
using SharedKernel.QueryObjects.Models;
using SyncFunction.Entities;
using SyncFunction.QueryObjects;

namespace SyncFunction.Repositories;

public interface IOrderItemRepository
{
    Task<PaginatedQueryable<OrderItem>> GetOrdersSortedByBrandAndPriceAync(
        int pageSize,
        IQueryObject<OrderItem, bool> where);

    Task<IEnumerable<OrderItem>> GetExistingOrderItemIds(
        IEnumerable<OrderedItem> orderedItems,
        IQueryObject<OrderItem, bool> where);
}
