using ImportScheduledJobs.Entities;

namespace ImportScheduledJobs.Mappers;

public class OrderItemMapper : IOrderItemMapper
{
    public OrderedItem Map(OrderItem orderItem) =>
        new OrderedItem(
            ItemId: orderItem.ItemId,
            OrderDetails: new OrderDetails(
                new ShippingAddress(
                    Street: orderItem.Order.Customer?.Street ?? string.Empty,
                    City: orderItem.Order.Customer?.City ?? string.Empty,
                    State: orderItem.Order.Customer?.State ?? string.Empty,
                    ZipCode: orderItem.Order.Customer?.ZipCode ?? string.Empty
                ),
                ShippedDate: orderItem.Order.ShippedDate,
                StaffId: orderItem.Order.StaffId
            ),
            ProductDetails: new ProductDetails(
                ProductName: orderItem.Product.ProductName,
                BrandName: orderItem.Product.Brand.BrandName,
                CategoryName: orderItem.Product.Category.CategoryName
            ),
            Quantity: orderItem.Quantity,
            ListPrice: orderItem.ListPrice
        );
}