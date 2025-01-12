using Microsoft.EntityFrameworkCore;

namespace SkiService.Models
{

public class SkiServiceDbContext : DbContext
{
    public SkiServiceDbContext(DbContextOptions<SkiServiceDbContext> options) : base(options) { }

    public DbSet<Employee> Employees { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Service> Services { get; set; }
}
}