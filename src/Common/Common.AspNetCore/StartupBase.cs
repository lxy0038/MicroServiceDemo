using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using DotNetCore.CAP.Transport;
using DotNetCore.CAP.Internal;
using System.Text;

namespace Common.AspNetCore
{
   public class AspNetCoreStartupBase
    {

        protected IConfiguration configuration;
        public AspNetCoreStartupBase(IConfiguration config)
        {
            this.configuration = config;

            var consulAddress = this.configuration["Consul:Address"];
            if (!string.IsNullOrEmpty(consulAddress))
            {
                LibServiceAddressManager.Start(consulAddress, this.configuration["Consul:Http"]);
            }
        }

        protected virtual ConsulConfig GetConsulConfig()
        {
            if (string.IsNullOrEmpty(configuration["Consul:Address"]) ||
                string.IsNullOrEmpty(configuration["Consul:Ip"])) return null;
            List<string> tags = new List<string>();
            string tag = configuration["Consul:Tag"];
            if (!string.IsNullOrEmpty(tag)) tags.Add(tag);
            tags.Add("EFService");
            ConsulConfig config = new ConsulConfig()
            {
                Address = configuration["Consul:Address"],
                Ip = configuration["Consul:Ip"],
                Name = configuration["ModuleCode"],
                Port = LibSysUtils.ToInt(configuration["Consul:Port"]),
                Protocol = configuration["Consul:Http"],
                Interval = LibSysUtils.ToInt(configuration["Consul:Interval"]),
                Timeout = LibSysUtils.ToInt(configuration["Consul:Timeout"]),
                DeregisterCriticalServiceAfter = LibSysUtils.ToInt(configuration["Consul:DeregisterCriticalServiceAfter"]),
            };
            if (string.IsNullOrEmpty(config.Protocol)) config.Protocol = "http";
            if (string.Compare(config.Protocol, "https", true) == 0) config.IsUseTls = true;
            if (config.Interval == 0) config.Interval = 5;
            if (config.Timeout == 0) config.Timeout = 10;
            if (config.DeregisterCriticalServiceAfter == 0) config.DeregisterCriticalServiceAfter = 5;
            config.Tags = tags.ToArray();
            return config;
        }

        protected virtual void RegisterDistributedCache(IServiceCollection services)
        {
            var redisConn = configuration["RedisConn"];
            if (string.IsNullOrEmpty(redisConn))
            {
                services.AddSingleton<ILibDistributedCacheManager, LibDefaultRedisCacheManager>();
                return;
            }
            services.AddSingleton<ILibDistributedCacheManager, LibRedisCacheManager>();
        }


        public virtual void RegisterBaseComponents(IServiceCollection services)
        {
            services.AddScoped<ILibServiceAddressManager, LibServiceAddressManager>(); 

        }

        public virtual void RegisterCap(IServiceCollection services)
        {
            var mqConn = this.configuration["MQ:Host"];
            if (string.IsNullOrEmpty(mqConn)) return;
            var user = this.configuration["MQ:User"];
            var password = this.configuration["MQ:Password"];
            var defaultGroup = this.configuration["MQ:DefaultGroup"];
            var defaultExchangeName = this.configuration["MQ:DefaultExchangeName"];
            var failedRetryCount = this.configuration["MQ:FailedRetryCount"];
            var producerThreadCount = this.configuration["MQ:ProducerThreadCount"];
            var consumerThreadCount = this.configuration["MQ:ConsumerThreadCount"];
            var useNoStore = LibSysUtils.ToBoolean(this.configuration["MQ:UseNoStore"]);
            var dbConnStr= this.configuration["MQ:DbConnStr"];
           // var param = new LibContextAccessParamForDI();
            if (!useNoStore&&string.IsNullOrEmpty(dbConnStr)) return;

          //  services.TryAddSingleton<IDispatcher, CapCustomDispatcher>();
          //  services.TryAddSingleton<IConsumerRegister, CapConsumerRegister>();
          //  services.TryAddSingleton<ILibBusManager, LibRabbitBusManager>();

           

            services.AddCap(x =>
            {
                //x.UseEntityFramework<LibContextAccess>(t => t.Schema = "public");
                //x.UseEntityFramework<lib>
                if (useNoStore)
                {
                    x.UseNoStorage();
                }
                else  
                    x.UsePostgreSql(dbConnStr); 
                x.UseRabbitMQ((x1) =>
                {
                    x1.HostName = mqConn;
                    if (!string.IsNullOrEmpty(password)) x1.Password = password;
                    if (!string.IsNullOrEmpty(user)) x1.UserName = user;
                    if (!string.IsNullOrEmpty(defaultExchangeName))
                        x1.ExchangeName = defaultExchangeName;
                });
                x.FailedRetryCount = string.IsNullOrEmpty(failedRetryCount) ? 3 : LibSysUtils.ToInt(failedRetryCount);
                x.ProducerThreadCount = string.IsNullOrEmpty(producerThreadCount) ? 1 : LibSysUtils.ToInt(producerThreadCount);
                x.ConsumerThreadCount = string.IsNullOrEmpty(consumerThreadCount) ? 1 : LibSysUtils.ToInt(consumerThreadCount);
                if (!string.IsNullOrEmpty(defaultGroup))
                    x.DefaultGroupName = defaultGroup; 
            });
        }
        public virtual void RegisterServices(IServiceCollection services)
        {
            //services.AddDbContext<LibContextAccess>();
            //services.RegisterBaseComponents();
            this.RegisterBaseComponents(services);
            this.RegisterDistributedCache(services);
            this.RegisterCap(services); 
            //this.DatabaseMigrate();
        }
    }
}
