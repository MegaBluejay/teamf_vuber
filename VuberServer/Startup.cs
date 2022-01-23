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
using VuberServer.Data;
using VuberServer.Hubs;

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