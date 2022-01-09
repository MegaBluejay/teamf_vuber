using Microsoft.EntityFrameworkCore;
using VuberCore.Entities;

namespace VuberCore.Data
{
    public class VuberDbContext : DbContext
    {
        public VuberDbContext(DbContextOptions<VuberDbContext> options)
            : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Ride> Rides { get; set; }
    }
}
