using System.ComponentModel.DataAnnotations;

namespace ImportScheduledJobs.Entities;

public class Store
{
    [Key]
    public int StoreId { get; set; }

    [MaxLength(255)]
    public string StoreName { get; set; }

    [MaxLength(25)]
    public string? Phone { get; set; }

    [MaxLength(255)]
    public string? Email { get; set; }

    [MaxLength(255)]
    public string? Street { get; set; }

    [MaxLength(255)]
    public string? City { get; set; }

    [MaxLength(10)]
    public string? State { get; set; }

    [MaxLength(5)]
    public string? ZipCode { get; set; }

    public Store(string storeName)
    {
        StoreName = storeName;
    }
}