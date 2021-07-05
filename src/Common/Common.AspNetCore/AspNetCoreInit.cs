using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace Common.AspNetCore
{
    public static class AspNetCoreInit
    {
        
        
        public static void Init(IConfiguration configuration,IServiceCollection services)
        {
            var redisConn = configuration["RedisConn"];
            if (string.IsNullOrEmpty(redisConn))
            {
                services.AddSingleton<ILibDistributedCacheManager, LibDefaultRedisCacheManager>();
                return;
            }
            services.AddSingleton<ILibDistributedCacheManager, LibRedisCacheManager>();
        }

    }
}
