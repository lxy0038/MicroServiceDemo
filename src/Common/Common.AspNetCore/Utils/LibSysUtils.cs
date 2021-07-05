// <copyright file="LibSysUtils.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Common.AspNetCore
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// 进位方式枚举.
    /// </summary>
    public enum LibRoundTypeEnum
    {
        /// <summary>
        /// 四舍五入.
        /// </summary>
        四舍五入 = 0,

        /// <summary>
        /// 向上进位.
        /// </summary>
        向上进位 = 1,

        /// <summary>
        /// 向下舍弃.
        /// </summary>
        向下舍弃 = 2,
    }
    /// <summary>
    /// 系统帮助类.
    /// </summary>
    public static class LibSysUtils
    {
        /// <summary>
        /// 复制对象.
        /// </summary>
        /// <param name="target">目标对象.</param>
        /// <param name="source">源对象.</param>
        /// <param name="noCopyFields">无复制的字段列表.</param>
        public static void CopyObject(object target, object source, params string[] noCopyFields)
        {
            if (target == null || source == null)
            {
                return;
            }

            string[] staticNoCopyFields = new string[] { "id", "flow_permit_status", "flow_send_status" };
            var targetType = target.GetType();
            var srcType = source.GetType();

            foreach (var p in srcType.GetProperties())
            {
                if (staticNoCopyFields != null && staticNoCopyFields.Any(t => string.Compare(p.Name, t, true) == 0))
                {
                    continue;
                }

                if (noCopyFields != null && noCopyFields.Any(t => string.Compare(p.Name, t, true) == 0))
                {
                    continue;
                }

                var targetP = targetType.GetProperty(p.Name);
                if (targetP != null && p.PropertyType == targetP.PropertyType)
                {
                    var value = p.GetValue(source, null);
                    try
                    {
                        targetP.SetValue(target, value);
                    }
                    catch
                    {
                    }
                }
            }
        }

        /// <summary>
        /// 对象转字符串.
        /// </summary>
        /// <param name="obj">对象.</param>
        /// <returns>字符串.</returns>
        public static string ToString(object obj)
        {
            if (obj == null)
            {
                return null;
            }

            try
            {
                return obj.ToString();
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 对象转Int值.
        /// </summary>
        /// <param name="obj">对象.</param>
        /// <returns>int值.</returns>
        public static int ToInt(object obj)
        {
            if (obj == null||string.IsNullOrEmpty(obj.ToString()))
            {
                return 0;
            }

            try
            {
                return Convert.ToInt32(obj);
            }
            catch
            {
            }

            return 0;
        }

        /// <summary>
        /// 对象转bool值.
        /// </summary>
        /// <param name="obj">对象.</param>
        /// <returns>bool值.</returns>
        public static bool ToBoolean(object obj)
        {
            if (obj == null || string.IsNullOrEmpty(obj.ToString()))
            {
                return false;
            }

            try
            {
                return Convert.ToBoolean(obj);
            }
            catch
            {
            }

            return false;
        }

        /// <summary>
        /// 对象转Decimal值.
        /// </summary>
        /// <param name="obj">对象.</param>
        /// <returns>Decimal值.</returns>
        public static decimal ToDecimal(object obj)
        {
            if (obj == null || string.IsNullOrEmpty(obj.ToString()))
            {
                return 0;
            }

            try
            {
                return Convert.ToDecimal(obj);
            }
            catch
            {
            }

            return 0;
        }

        /// <summary>
        /// 对象转DateTime值.
        /// </summary>
        /// <param name="obj">对象.</param>
        /// <returns>DateTime值.</returns>
        public static DateTime? ToDateTime(object obj)
        {
            if (obj == null ||string.IsNullOrEmpty(obj.ToString()))
            {
                return null;
            }

            try
            {
                return Convert.ToDateTime(obj);
            }
            catch
            {
            }

            return null;
        }

        /// <summary>
        /// 格式化小数点.
        /// </summary>
        /// <param name="qty">数.</param>
        /// <param name="len">小数点位数.</param>
        /// <param name="type">进位方式.</param>
        /// <returns>格式化后的数.</returns>
        public static decimal Round(decimal qty, int len, LibRoundTypeEnum type)
        {
            switch (type)
            {
                case LibRoundTypeEnum.向上进位:
                    if (len != 0)
                    {
                        string[] array2 = qty.ToString().Split('.');
                        if (array2.Length >= 2)
                        {
                            string text2 = array2[1].PadRight(6, '0');
                            qty = Convert.ToDecimal(array2[0] + "." + text2.Substring(0, len));
                        }
                    }
                    else
                    {
                        qty = Math.Ceiling(qty);
                    }

                    return qty;
                case LibRoundTypeEnum.向下舍弃:
                    if (len != 0)
                    {
                        string[] array = qty.ToString().Split('.');
                        if (array.Length >= 2)
                        {
                            string text = array[1].PadRight(6, '0');
                            qty = Convert.ToDecimal(array[0] + "." + text.Substring(0, len));
                            if (text[len] != '0')
                            {
                                qty += 1m / (decimal)Math.Pow(10.0, len);
                            }
                        }
                    }
                    else
                    {
                        qty = Math.Floor(qty);
                    }

                    return qty;
                default:
                    return Math.Round(qty, len, MidpointRounding.AwayFromZero);
            }
        }

        /// <summary>
        /// 获取GRpc序列化字符串.
        /// </summary>
        /// <param name="v">原始字符串.</param>
        /// <returns>转换后字符串.</returns>
        public static string GrpcSerial(string v)
        {
            if (v == null)
            {
                return string.Empty;
            }

            return v;
        }

        /// <summary>
        /// 获取GRpc反序列化字符串.
        /// </summary>
        /// <param name="v">原始字符串.</param>
        /// <returns>转换后字符串.</returns>
        public static string GrpcDeSerial(string v)
        {
            if (string.IsNullOrEmpty(v))
            {
                return null;
            }

            return v;
        }


        public static string GetRandomStrByLength(int startlength=1,int endlength = 100)
        {
            Random ran = new Random();
            int RandKey = ran.Next(startlength, endlength);
            StringBuilder sb = new StringBuilder();
            for (var j = 0; j < RandKey; j++)
            {
                Random ran1 = new Random();
                int RandKe1y1 = ran1.Next(65, 90);
                var r = NunToChar(RandKe1y1);
                sb.Append(r);
            }
            return sb.ToString();
        }
        private static string NunToChar(int number)
        {
            if (65 <= number && 90 >= number)
            {
                System.Text.ASCIIEncoding asciiEncoding = new System.Text.ASCIIEncoding();
                byte[] btNumber = new byte[] { (byte)number };
                return asciiEncoding.GetString(btNumber);
            }
            return "a";
        }

    }
}
