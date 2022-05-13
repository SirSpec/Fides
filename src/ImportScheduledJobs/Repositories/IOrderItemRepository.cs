using ImportScheduledJobs.Entities;
using ImportScheduledJobs.Extensions;

namespace ImportScheduledJobs.Repositories;

public interface IOrderItemRepository
{
    public Task<PaginatedQueryable<OrderItem>> GetSoldItemsSortedByBrandAndPriceAync(int pageSize);
}
