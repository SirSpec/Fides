using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ImportScheduledJobs.Entities;

public class Order
{
    [Key]
    public int OrderId { get; set; }

    [ForeignKey("CustomerId")]
    public Customer? Customer { get; set; }
    public int? CustomerId { get; set; }
    public OrderStatus OrderStatus { get; set; }
    public DateTime OrderDate { get; set; }
    public DateTime RequiredDate { get; set; }
    public DateTime? ShippedDate { get; set; }

    [ForeignKey("StoreId")]
    public Store Store { get; set; } = null!;
    public int StoreId { get; set; }

    [ForeignKey("StaffId")]
    public Staff Staff { get; set; } = null!;
    public int StaffId { get; set; }
}