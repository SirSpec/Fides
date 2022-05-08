
using Microsoft.EntityFrameworkCore;

namespace ImportScheduledJobs;

public class MyDbContext : DbContext
{
    public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
    {
    }
}