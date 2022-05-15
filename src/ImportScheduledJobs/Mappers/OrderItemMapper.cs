using System.Linq.Expressions;
using ImportScheduledJobs.Entities;

namespace ImportScheduledJobs.Mappers;

public class OrderItemMapper : IOrderItemMapper
{
    public Expression<Func<OrderItem, OrderedItem>> Query =>
        orderItem => new OrderedItem(
            orderItem.ItemId,
            new OrderDetails(
                new ShippingAddress(
                    orderItem.Order.Customer!.Street ?? string.Empty,
                    orderItem.Order.Customer!.City ?? string.Empty,
                    orderItem.Order.Customer!.State ?? string.Empty,
                    orderItem.Order.Customer!.ZipCode ?? string.Empty
                ),
                orderItem.Order.ShippedDate,
                orderItem.Order.StaffId
            ),
            new ProductDetails(
                orderItem.Product.ProductName,
                orderItem.Product.Brand.BrandName,
                orderItem.Product.Category.CategoryName
            ),
            orderItem.Quantity,
            orderItem.ListPrice
        );
}
