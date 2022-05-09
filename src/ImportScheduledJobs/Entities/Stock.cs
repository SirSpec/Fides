using System.ComponentModel.DataAnnotations.Schema;

namespace ImportScheduledJobs.Entities;

public class Stock
{
    [ForeignKey("StoreId")]
    public Store? Store { get; set; }
    public int? StoreId { get; set; }

    [ForeignKey("ProductId")]
    public Product? Product { get; set; }
    public int? ProductId { get; set; }
    public int? Quantity { get; set; }
}
