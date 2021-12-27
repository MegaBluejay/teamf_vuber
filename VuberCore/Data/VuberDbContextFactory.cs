using System.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace VuberCore.Data
{
    public class VuberDbContextFactory : IDesignTimeDbContextFactory<VuberDbContext>
    {
        public VuberDbContext CreateDbContext(string[] args)
        {
            ConnectionStringSettings connectionStringSettings = ConfigurationManager.ConnectionStrings["VuberDatabase"];
            return new VuberDbContext(
                new DbContextOptionsBuilder<VuberDbContext>()
                .UseLazyLoadingProxies()
                .UseNpgsql(connectionStringSettings.ConnectionString)
                .Options
            );
        }
    }
}
