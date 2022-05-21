using System.ComponentModel.DataAnnotations;

namespace ImportScheduledJobs.Entities;

public class Customer
{
    [Key]
    public int CustomerId { get; set; }

    [MaxLength(255)]
    public string FirstName { get; set; }

    [MaxLength(255)]
    public string LastName { get; set; }

    [MaxLength(25)]
    public string? Phone { get; set; }

    [MaxLength(255)]
    public string Email { get; set; }

    [MaxLength(255)]
    public string? Street { get; set; }

    [MaxLength(50)]
    public string? City { get; set; }

    [MaxLength(25)]
    public string? State { get; set; }

    [MaxLength(5)]
    public string? ZipCode { get; set; }

    public Customer(string firstName, string lastName, string email)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
    }
}