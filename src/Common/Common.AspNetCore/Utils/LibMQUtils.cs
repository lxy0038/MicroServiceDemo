using DotNetCore.CAP;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace Common.AspNetCore
{
    public static class LibMQUtils
    { 

        public static async Task PublishAsync<T>(IServiceProvider provider, string name, T contentObj, string callbackName = null, CancellationToken cancellationToken = default)
        {
            if (!CheckCap(provider)) return;
            var capBus = GetCapPublisher(provider);
            await capBus.PublishAsync<T>(name, contentObj, callbackName, cancellationToken);
        }


        public static async Task PublishAsync<T>(IServiceProvider provider, string name, T contentObj, IDictionary<string, string> headers, CancellationToken cancellationToken = default)
        {
            if (!CheckCap(provider)) return;
            var capBus = GetCapPublisher(provider);

            await capBus.PublishAsync<T>(name, contentObj, headers, cancellationToken);
        }


        public static void Publish<T>(IServiceProvider provider, string name, T contentObj, string callbackName = null)
        {
            if (!CheckCap(provider)) return;
            var capBus = GetCapPublisher(provider);
            capBus.Publish<T>(name, contentObj, callbackName);
        }

        public static void Publish<T>(IServiceProvider provider, string name, T contentObj, IDictionary<string, string> headers)
        {
            if (!CheckCap(provider)) return;
            var capBus = GetCapPublisher(provider);
            capBus.Publish<T>(name, contentObj, headers);
        }

        private static bool CheckCap(IServiceProvider provider)
        {
            if (provider != null)
            {
                var config = provider.GetService<IConfiguration>();
                return !string.IsNullOrEmpty(config["MQ:Host"]);
            }
            return false;
        }

        private static ICapPublisher GetCapPublisher(IServiceProvider provider)
        {
            ICapPublisher capPublisher = null;
            try
            {
                if (provider != null)
                    capPublisher = provider.GetService<ICapPublisher>(); 
            }
            catch { }
            if (capPublisher == null) throw new Exception("capPublisher null");
            return capPublisher;
        }

    }
}
