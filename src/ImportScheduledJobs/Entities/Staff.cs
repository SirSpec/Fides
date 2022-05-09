using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ImportScheduledJobs.Entities;

public class Staff
{
    [Key]
    public int StaffId { get; set; }

    [Required]
    [MaxLength(50)]
    public string FirstName { get; set; }

    [Required]
    [MaxLength(50)]
    public string LastName { get; set; }

    [Required]
    [MaxLength(255)]
    public string Email { get; set; }

    [MaxLength(25)]
    public string? Phone { get; set; }

    [Required]
    public int Active { get; set; }

    [ForeignKey("StoreId")]
    public Store Store { get; set; } = null!;

    [Required]
    public int StoreId { get; set; }

    [ForeignKey("ManagerId")]
    public Staff? Manager { get; set; }
    public int? ManagerId { get; set; }

    public Staff(string firstName, string lastName, string email)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
    }
}