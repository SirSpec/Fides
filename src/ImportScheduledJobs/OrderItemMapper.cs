using ImportScheduledJobs.Entities;

namespace ImportScheduledJobs;

public record ProductDetails(string ProductName, string BrandName, string CategoryName);
public record ShippingAddress(string Street, string City, string State, string ZipCode);
public record OrderDetails(ShippingAddress ShippingAddress, DateTime? ShippedDate, int StaffId);
public record OrderedItem(OrderDetails OrderDetails, ProductDetails ProductDetails, int Quantity, decimal ListPrice);

public class OrderItemMapper
{
    public OrderedItem Map(OrderItem orderItem) =>
        new OrderedItem(
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