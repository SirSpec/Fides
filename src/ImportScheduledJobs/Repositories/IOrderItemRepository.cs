using ImportScheduledJobs.Entities;
using ImportScheduledJobs.Extensions;
using ImportScheduledJobs.QueryObjects;

namespace ImportScheduledJobs.Repositories;

public interface IOrderItemRepository
{
    public Task<PaginatedQueryable<OrderItem>> GetOrdersSortedByBrandAndPriceAync(
        int pageSize,
        IQueryObject<OrderItem, bool> where);
}
