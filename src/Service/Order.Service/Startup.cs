using Common.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using NConsul.AspNetCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVAN.MES.Rabbitmq.Publish;

namespace Order.Service
{
    public class Startup : AspNetCoreStartupBase
    {
        public Startup(IConfiguration configuration) : base(configuration)
        {
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            this.RegisterServices(services);
            services.AddSingleton<Pubmq>();
            services.AddSingleton<MqConnection>();
            services.AddGrpc();
            var config = GetConsulConfig();
            if (config != null)
            {
                services.AddConsul(config.Address)
                    .AddGRPCHealthCheck($"{config.Ip}:{config.Port }/", config.IsUseTls)
                      .RegisterService(config.Name, config.Ip, config.Port, config.Tags);
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<HealthCheckRPCService>();
                endpoints.MapGrpcService<OrderRPCService>();

                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
                });
            });
        }
    }
}
