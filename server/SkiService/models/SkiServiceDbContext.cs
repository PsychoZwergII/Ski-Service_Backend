using Microsoft.EntityFrameworkCore;

namespace SkiService.Models
{
    public class SkiServiceDbContext : DbContext
    {
        public SkiServiceDbContext(DbContextOptions<SkiServiceDbContext> options) : base(options) { }

        public DbSet<Users> Users { get; set; }
        public DbSet<Userroles> Userroles { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Service> Service { get; set; }
    }
}
