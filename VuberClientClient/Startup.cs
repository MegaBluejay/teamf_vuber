using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using VuberClientClient.Controllers;
using VuberClientClient.Hubs;
using VuberCore.Hubs;

namespace VuberClientClient
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
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "VuberClientClient", Version = "v1" });
            });
            /*var creds = Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1")
                .GetBytes("SomeUserName" + ":" + "__NOPASS__"));
            var hubConnection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5005/client", options => options.Headers["Authorization"] = $"Basic {creds}")
                .WithAutomaticReconnect()
                .Build();
            hubConnection.StartAsync().Wait();
            var clientNotificationController = new ClientNotificationController();
            services.AddSingleton<IClientHub, ClientHubWrapper>(_ =>
                new ClientHubWrapper(hubConnection, clientNotificationController));*/
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "VuberClientClient v1"));

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
