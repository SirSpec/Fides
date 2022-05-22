using SyncFunction.Entities;
using System.Linq.Expressions;

namespace SyncFunction.QueryObjects.Queries;

public class CompletedOrdersQuery : ICompletedOrdersQuery
{
    public Expression<Func<OrderItem, bool>> Query =>
        orderItem => orderItem.Order.OrderStatus == OrderStatus.Completed;
}
