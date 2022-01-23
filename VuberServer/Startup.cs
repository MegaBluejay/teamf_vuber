using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging.Abstractions;
using VuberCore.Entities;
using VuberServer.Clients;
using VuberServer.Controllers;
using VuberServer.Data;
using VuberServer.Hubs;
using VuberServer.Strategies.CalculateNewRatingStrategies;
using VuberServer.Strategies.CalculatePriceStrategies;
using VuberServer.Strategies.CalculateRideDistanceStrategies;
using VuberServer.Strategies.CheckWorkloadLevelStrategies;
using VuberServer.Strategies.FindNearbyDriversStrategies;
using VuberServer.Strategies.FindRidesWithLookingStatusStrategies;
using VuberServer.Tools;

namespace VuberServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSignalR();
            var connectionString = Configuration.GetConnectionString("VuberDatabase");
            services.AddDbContext<VuberDbContext>(options =>
                options
                    .UseLazyLoadingProxies()
                    .UseNpgsql(connectionString,
                        npgsqlOptions=> npgsqlOptions.UseNetTopologySuite()));
            services.AddSingleton<IUserIdProvider, VuberUserIdProvider>();
            services.AddAuthentication()
                .AddBasicAuthentication<VuberCredentialVerifier>();
            services.AddSingleton<IVuberController, VuberController>(provider =>
                new VuberController(new VuberControllerOptionsBuilder()
                    .UseClientHubContext(provider.GetService<IHubContext<ClientHub, IClientClient>>())
                    .UseDriverHubContext(provider.GetService<IHubContext<DriverHub, IDriverClient>>())
                    .UseDbContext(provider.GetService<VuberDbContext>())
                    .UseLogger(new NullLogger<VuberController>())
                    .CalculateNewRatingStrategy(new ArithmeticalMeanCalculateNewRatingStrategy())
                    .CalculatePriceStrategy(new DefaultCalculatePriceStrategy(1,1,1,1))
                    .CheckWorkloadLevelStrategy(new DefaultCheckWorkloadLevelStrategy(10))
                    .FindRidesWithLookingStatusStrategy(new DefaultFindRidesWithLookingStatusStrategy())
                    .CalculateRideDistanceStrategy(new LinearRideDistanceStrategy())
                    .CalculateLengthStrategy(new LinearLengthStrategy())
                    .FindNearbyDriversStrategy(new FindNearbyDriversWithMinimalDistanceStrategy(100))
                    .Chronometer(new RealTimeChronometer())
                    .Options
                ));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<DriverHub>("/driver");
                endpoints.MapHub<ClientHub>("/client");
            });
        }
    }
}