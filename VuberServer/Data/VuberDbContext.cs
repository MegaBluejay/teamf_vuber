using Microsoft.EntityFrameworkCore;
using VuberCore.Entities;

namespace VuberServer.Data
{
    public class VuberDbContext : DbContext
    {
        public VuberDbContext(DbContextOptions<VuberDbContext> options)
            : base(options) { }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Ride> Rides { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("postgis");
        }
    }
}
