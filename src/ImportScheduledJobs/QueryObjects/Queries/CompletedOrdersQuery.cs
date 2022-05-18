using System.Linq.Expressions;
using ImportScheduledJobs.Entities;

namespace ImportScheduledJobs.QueryObjects.Queries;

public class CompletedOrdersQuery : ICompletedOrdersQuery
{
    public Expression<Func<OrderItem, bool>> Query =>
        orderItem => orderItem.Order.OrderStatus == OrderStatus.Completed;
}
