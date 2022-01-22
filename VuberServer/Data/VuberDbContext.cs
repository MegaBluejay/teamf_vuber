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
        
        public DbSet<Checkpoint> Checkpoints { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<PaymentCard> PaymentCards { get; set; }
    }
}
