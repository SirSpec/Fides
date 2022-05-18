using Nest;

namespace ImportScheduledJobs.Mappers;

public record ProductDetails(string ProductName, string BrandName, string CategoryName);
public record ShippingAddress(string Street, string City, string State, string ZipCode);
public record OrderDetails(ShippingAddress ShippingAddress, DateTime? ShippedDate, int StaffId);

[ElasticsearchType(IdProperty = nameof(Id))]
public record OrderedItem(
    string Id,
    OrderDetails OrderDetails,
    ProductDetails ProductDetails,
    int Quantity,
    decimal ListPrice);
