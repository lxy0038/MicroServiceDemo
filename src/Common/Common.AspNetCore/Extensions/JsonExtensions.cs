// <copyright file="JsonExtensions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Common.AspNetCore
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Newtonsoft.Json;

    /// <summary>
    /// Json 表达式.
    /// </summary>
    public static class JsonExtensions
    {
        /// <summary>
        /// 对象转字符串.
        /// </summary>
        /// <param name="obj">对象.</param>
        /// <returns>字符串.</returns>
        public static string ToJson(this object obj)
        {
            if (obj == null)
            {
                return string.Empty;
            }

            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver(),
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            };
            return JsonConvert.SerializeObject(obj, settings);
        }

        /// <summary>
        /// 字符串解析出对象.
        /// </summary>
        /// <typeparam name="T">解析对象类型.</typeparam>
        /// <param name="str">字符串.</param>
        /// <returns>对象.</returns>
        public static T ToObjectFromJson<T>(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return default;
            }

            return JsonConvert.DeserializeObject<T>(str);
        }
    }
}
