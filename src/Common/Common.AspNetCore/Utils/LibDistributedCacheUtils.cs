using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace Common.AspNetCore
{
    public static class LibDistributedCacheUtils
    {
        public static string Get(IServiceProvider provider, string key)
        {
            var distributedCache = GetDistributedCache(provider);
            return distributedCache.Get(key);
        }

        public static void Set(IServiceProvider provider, string key, object value, TimeSpan? expire)
        {
            var distributedCache = GetDistributedCache(provider);
            distributedCache.Set(key, value, expire); 
        }

        public static bool Exists(IServiceProvider provider, string key)
        {
            var distributedCache = GetDistributedCache(provider);
            return distributedCache.Exists(key);
        }

        public static async Task<bool> ExistsAsync(IServiceProvider provider, string key)
        {
            var distributedCache = GetDistributedCache(provider);
            return await distributedCache.ExistsAsync(key);
        }

        public static T Get<T>(IServiceProvider provider, string key)
        {
            var distributedCache = GetDistributedCache(provider);
            return distributedCache.Get<T>(key);
        }


        public static async Task<string> GetAsync(IServiceProvider provider, string key)
        {
            var distributedCache = GetDistributedCache(provider);
            return await distributedCache.GetAsync(key);
        }

        public static async Task<T> GetAsync<T>(IServiceProvider provider, string key)
        {
            var distributedCache = GetDistributedCache(provider);
            return await distributedCache.GetAsync<T>(key);
        }

        public static async Task SetAsync(IServiceProvider provider, string key, object obj, TimeSpan? expire)
        {
            var distributedCache = GetDistributedCache(provider);
            await distributedCache.SetAsync(key, obj, expire);
        }

        public static async Task DelAsync(IServiceProvider provider, params string[] keys)
        {
            var distributedCache = GetDistributedCache(provider);
            await distributedCache.DelAsync(keys);
        }


        public  static void Delete(IServiceProvider provider,string key)
        {
            var distributedCache = GetDistributedCache(provider);
            distributedCache.Del(key);
        }

        public static void HashSet(IServiceProvider provider, string key, string field, string value)
        {
            var distributedCache = GetDistributedCache(provider);
            distributedCache.HashSet(key,field,value);
        }

        public static string HashGet(IServiceProvider provider, string key, string field)
        {
            var distributedCache = GetDistributedCache(provider);
          return  distributedCache.HashGet(key, field);
        }
        public static bool Expire(IServiceProvider provider, string key, TimeSpan expire)
        { 
            var distributedCache = GetDistributedCache(provider);
            return distributedCache.Expire(key, expire);
        }
        public static void HashDel(IServiceProvider provider, string key, string field)
        {
            var distributedCache = GetDistributedCache(provider);
              distributedCache.HashDel(key, field);
        }

        public static Dictionary<string, string> HashGetAll(IServiceProvider provider, string key)
        {
            var distributedCache = GetDistributedCache(provider);
         return   distributedCache.HashGetAll(key);
        }


        private static ILibDistributedCacheManager GetDistributedCache(IServiceProvider provider)
        {
            ILibDistributedCacheManager clientFactory = null;
            try
            {
                if(provider!=null)
                    clientFactory = provider.GetService<ILibDistributedCacheManager>(); 
                
            }
            catch { }
            if (clientFactory == null) throw new Exception("IDistributedCache is null");
            return clientFactory;
        }
    }
}
