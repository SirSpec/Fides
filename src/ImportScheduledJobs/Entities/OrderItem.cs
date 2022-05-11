using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ImportScheduledJobs.Entities;

public class OrderItem
{
    [ForeignKey("OrderId")]
    public Order Order { get; set; } = null!;

    public int OrderId { get; set; }
    public int ItemId { get; set; }

    [ForeignKey("ProductId")]
    public Product Product { get; set; } = null!;

    public int ProductId { get; set; }
    public int Quantity { get; set; }

    [Precision(10, 2)]
    public decimal ListPrice { get; set; }

    [Precision(4, 2)]
    [DefaultValue(0)]
    public decimal Discount { get; set; }
}