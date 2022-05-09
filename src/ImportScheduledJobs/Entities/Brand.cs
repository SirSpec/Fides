using System.ComponentModel.DataAnnotations;

namespace ImportScheduledJobs.Entities;

public class Brand
{
    [Key]
    public int BrandId { get; set; }

    [MaxLength(255)]
    public string BrandName { get; set; }

    public Brand(string brandName)
    {
        BrandName = brandName;
    }
}