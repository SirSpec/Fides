using SharedKernel.QueryObjects.Models;
using SyncFunction.Entities;
using System.Linq.Expressions;

namespace SyncFunction.QueryObjects.Mappers;

public class OrderItemMapper : IOrderItemMapper
{
    public Expression<Func<OrderItem, OrderedItem>> Query =>
        orderItem => new OrderedItem(
            new OrderedItemId(orderItem.OrderId, orderItem.ItemId),
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
