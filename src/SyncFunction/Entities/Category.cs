using System.ComponentModel.DataAnnotations;

namespace SyncFunction.Entities;

public class Category
{
    [Key]
    public int CategoryId { get; set; }

    [MaxLength(255)]
    public string CategoryName { get; set; }

    public Category(string categoryName)
    {
        CategoryName = categoryName;
    }
}