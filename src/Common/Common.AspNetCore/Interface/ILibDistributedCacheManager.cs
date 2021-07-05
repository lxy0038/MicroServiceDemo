// <copyright file="ILibDistributedCacheManager.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Common.AspNetCore
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// 分布式缓存接口定义.
    /// </summary>
    public interface ILibDistributedCacheManager
    {
        /// <summary>
        /// 根据键值获取缓存字符串.
        /// </summary>
        /// <param name="key">键值.</param>
        /// <returns>缓存字符串.</returns>
        string Get(string key);

        /// <summary>
        /// 根据键值获取缓存对象.
        /// </summary>
        /// <typeparam name="T">反向解析对象类型.</typeparam>
        /// <param name="key">键值.</param>
        /// <returns>缓存对象.</returns>
        T Get<T>(string key);

        /// <summary>
        /// 根据键值异步获取缓存字符串.
        /// </summary>
        /// <param name="key">键值.</param>
        /// <returns>缓存字符串.</returns>
        Task<string> GetAsync(string key);

        /// <summary>
        /// 根据键值获取缓存对象.
        /// </summary>
        /// <typeparam name="T">反向解析对象类型.</typeparam>
        /// <param name="key">键值.</param>
        /// <returns>缓存对象.</returns>
        Task<T> GetAsync<T>(string key);

        /// <summary>
        /// 设置缓存及缓存过期时间.
        /// </summary>
        /// <param name="key">键值.</param>
        /// <param name="obj">缓存对象.</param>
        /// <param name="expire">过期时间.</param>
        void Set(string key, object obj, TimeSpan? expire);

        /// <summary>
        /// 异步设置缓存及缓存过期时间.
        /// </summary>
        /// <param name="key">键值.</param>
        /// <param name="obj">缓存对象.</param>
        /// <param name="expire">过期时间.</param>
        /// <returns>异步类.</returns>
        Task SetAsync(string key, object obj, TimeSpan? expire);

        /// <summary>
        /// 依据键值批次删除缓存.
        /// </summary>
        /// <param name="keys">删除键值列表.</param>
        public void Del(params string[] keys);

        /// <summary>
        /// 依据键值异步批次删除缓存.
        /// </summary>
        /// <param name="keys">删除键值列表.</param>
        /// <returns>异步类.</returns>
        Task DelAsync(params string[] keys);

        /// <summary>
        /// 判断键值是否存在缓存.
        /// </summary>
        /// <param name="key">键值.</param>
        /// <returns>是否存在.</returns>
        bool Exists(string key);

        /// <summary>
        /// 异步判断键值是否存在缓存.
        /// </summary>
        /// <param name="key">键值.</param>
        /// <returns>是否存在.</returns>
        Task<bool> ExistsAsync(string key);

        /// <summary>
        ///  设置哈希值.
        /// </summary>
        /// <param name="key">键值.</param>
        /// <param name="field">字段.</param>
        /// <param name="value">值.</param>
        void HashSet(string key, string field, string value);

        /// <summary>
        /// 获取哈希值.
        /// </summary>
        /// <param name="key">键值.</param>
        /// <param name="field">字段.</param>
        /// <returns>值.</returns>
        string HashGet(string key, string field);

        /// <summary>
        /// 删除哈希值.
        /// </summary>
        /// <param name="key">键值.</param>
        /// <param name="field">字段.</param>
        void HashDel(string key, string field);

        /// <summary>
        /// 获取批量哈希值.
        /// </summary>
        /// <param name="key">键值.</param>
        /// <returns>批量哈希值.</returns>
        Dictionary<string, string> HashGetAll(string key);


        bool Expire(string key, TimeSpan expire);
    }
}
