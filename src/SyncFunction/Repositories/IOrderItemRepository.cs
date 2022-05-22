using SharedKernel.Extensions;
using SyncFunction.Entities;
using SyncFunction.QueryObjects;

namespace SyncFunction.Repositories;

public interface IOrderItemRepository
{
    public Task<PaginatedQueryable<OrderItem>> GetOrdersSortedByBrandAndPriceAync(
        int pageSize,
        IQueryObject<OrderItem, bool> where);
}
