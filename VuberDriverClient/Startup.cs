using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using VuberCore.Hubs;
using VuberDriverClient.Controllers;
using VuberDriverClient.Hubs;

namespace VuberDriverClient
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services
               .AddControllers()
               .AddNewtonsoftJson(options =>
               {
                   options.SerializerSettings.Formatting = Formatting.Indented;
               });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "VuberDriverClient", Version = "v1" });
            });
            var hubConnection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5003/driver")
                .WithAutomaticReconnect()
                .Build();
            var driverNotificationController = new DriverNotificationController();
            services.AddSingleton<IDriverHub, DriverHubWrapper>(_ =>
                new DriverHubWrapper(hubConnection, driverNotificationController));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "VuberDriverClient v1"));

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
