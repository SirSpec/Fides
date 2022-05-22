using System.Linq.Expressions;
using SyncFunction.Entities;

namespace SyncFunction.QueryObjects.Queries;

public class CompletedOrdersQuery : ICompletedOrdersQuery
{
    public Expression<Func<OrderItem, bool>> Query =>
        orderItem => orderItem.Order.OrderStatus == OrderStatus.Completed;
}
