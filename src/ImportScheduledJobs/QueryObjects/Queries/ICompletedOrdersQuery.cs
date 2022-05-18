using ImportScheduledJobs.Entities;

namespace ImportScheduledJobs.QueryObjects.Queries;

public interface ICompletedOrdersQuery : IQueryObject<OrderItem, bool>
{
}
