using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace Common.AspNetCore
{
    public class LibRedisCacheManager: ILibDistributedCacheManager
    {
        private readonly IConfiguration configuration;
        public LibRedisCacheManager(IConfiguration configuration)
        {
            this.configuration = configuration;
            var redisConn = configuration["RedisConn"];
            var csredis = new CSRedis.CSRedisClient(redisConn);
            RedisHelper.Initialization(csredis);
        }

        private string GetMd5Key(string key)
        {
            if (key.Length < 32) return key;
            return Hash_MD5_32( key);
        }
        private static string Hash_MD5_32(string word, bool toUpper = true)
        {

            System.Security.Cryptography.MD5CryptoServiceProvider MD5CSP
                = new System.Security.Cryptography.MD5CryptoServiceProvider();

            byte[] bytValue = System.Text.Encoding.UTF8.GetBytes(word);
            byte[] bytHash = MD5CSP.ComputeHash(bytValue);
            MD5CSP.Clear();

            //根据计算得到的Hash码翻译为MD5码
            string sHash = "", sTemp = "";
            for (int counter = 0; counter < bytHash.Length; counter++)
            {
                long i = bytHash[counter] / 16;
                if (i > 9)
                {
                    sTemp = ((char)(i - 10 + 0x41)).ToString();
                }
                else
                {
                    sTemp = ((char)(i + 0x30)).ToString();
                }
                i = bytHash[counter] % 16;
                if (i > 9)
                {
                    sTemp += ((char)(i - 10 + 0x41)).ToString();
                }
                else
                {
                    sTemp += ((char)(i + 0x30)).ToString();
                }
                sHash += sTemp;
            }

            //根据大小写规则决定返回的字符串
            return toUpper ? sHash : sHash.ToLower(); 
        }
        public string Get(string key)
        {
            return RedisHelper.Get(GetMd5Key(key));
        }

        public T Get<T>(string key)
        {
            return RedisHelper.Get<T>(GetMd5Key(key)); 
        }


        public async Task<string> GetAsync(string key)
        { 
            return await RedisHelper.GetAsync(GetMd5Key(key));
        }

        public async Task<T> GetAsync<T>(string key)
        {
            return await RedisHelper.GetAsync<T>(GetMd5Key(key));
        }


        public void Set(string key, object obj, TimeSpan? expire)
        {
            if (expire == null)
                expire = new TimeSpan(30, 0, 0, 0);
            RedisHelper.Set(GetMd5Key(key), obj, expire.Value);
        }
        public async Task SetAsync(string key, object obj, TimeSpan? expire)
        {
            if (expire == null)
                expire = new TimeSpan(30, 0, 0, 0);
            await RedisHelper.SetAsync(GetMd5Key(key), obj, expire.Value);
        }


        public void Del(params string[] keys)
        {
            if (keys != null)
                RedisHelper.Del(keys.Select(t => GetMd5Key(t)).ToArray());
        }
        public async Task DelAsync(params string[] keys)
        {
            if(keys!=null)
            await RedisHelper.DelAsync( keys.Select(t=> GetMd5Key(t)).ToArray());
        }


        public bool Exists(string key)
        {
            return RedisHelper.Exists(GetMd5Key(key));
        }

        public async Task<bool> ExistsAsync(string key)
        {
            return await RedisHelper.ExistsAsync(GetMd5Key(key));
        }


        public void HashSet(string key, string field, string value)
        {
            RedisHelper.HSet(GetMd5Key(key), field, value);
        }
        public string HashGet(string key, string field)
        {
            return RedisHelper.HGet(GetMd5Key(key), field);
        }
        public void HashDel(string key, string field)
        {
            RedisHelper.HDel(GetMd5Key(key), field);
        }
        public bool Expire(string key, TimeSpan expire)
        {
            return RedisHelper.Expire(GetMd5Key(key), expire);
        }
        public Dictionary<string, string> HashGetAll(string key)
        {
            return RedisHelper.HGetAll(GetMd5Key(key)); 
        }

    }

    public class LibDefaultRedisCacheManager : ILibDistributedCacheManager
    {
        public string Get(string key)
        {
            return null;
        }

        public T Get<T>(string key)
        {
            return default(T);
        }


        public Task<string> GetAsync(string key)
        {
            return Task.Run(() =>
            {
                return "";
            });
        }

        public Task<T> GetAsync<T>(string key)
        {
            return Task.Run(() =>
            {
                return default(T);
            });
        }


        public void Set(string key, object obj, TimeSpan? expire)
        {
        }
        public Task SetAsync(string key, object obj, TimeSpan? expire)
        {
            return Task.Run(() =>
            {
                
            });
        }
        public bool Expire(string key, TimeSpan expire)
        {
            return true;
        }

        public void Del(params string[] keys)
        {
           
        }
        public  Task DelAsync(params string[] keys)
        {
            return Task.Run(() =>
            {

            });
        }


        public bool Exists(string key)
        {
            return false;
        }

        public Task<bool> ExistsAsync(string key)
        {
            return Task.Run(() =>
            {
                return false;
            });
        }


        public void HashSet(string key, string field, string value)
        { 
        }
        public void HashDel(string key, string field)
        {
            //RedisHelper.HDel(key, field);
        }

        public string HashGet(string key, string field)
        {
            return string.Empty;
        }

        public Dictionary<string, string> HashGetAll(string key)
        {
            return new Dictionary<string, string>();
        }
    }

}
