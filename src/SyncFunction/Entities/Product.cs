using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SyncFunction.Entities;

public class Product
{
    [Key]
    public int ProductId { get; set; }

    [MaxLength(255)]
    public string ProductName { get; set; }

    [ForeignKey("BrandId")]
    public Brand Brand { get; set; } = null!;
    public int BrandId { get; set; }

    [ForeignKey("CategoryId")]
    public Category Category { get; set; } = null!;
    public int CategoryId { get; set; }
    public int ModelYear { get; set; }

    [Precision(10, 2)]
    public decimal ListPrice { get; set; }

    public Product(string productName)
    {
        ProductName = productName;
    }
}