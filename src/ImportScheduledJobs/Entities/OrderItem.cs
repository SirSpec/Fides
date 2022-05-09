using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
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

    [Required]
    public int ProductId { get; set; }

    [Required]
    public int Quantity { get; set; }

    [Required]
    [Precision(10, 2)]
    public decimal ListPrice { get; set; }

    [Required]
    [Precision(4, 2)]
    [DefaultValue(0)]
    public decimal Discount { get; set; }
}