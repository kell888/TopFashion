using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.ComponentModel;
using System.Reflection;
using System.Security.Cryptography;

namespace TopFashion
{
    public static class CommonConsts
    {
        public static string DefaultConnString
        {
            get
            {
                return @"server=.;database=TF;user id=sa;password=damaodf";
            }
        }

        public static string DefaultBackupConnString
        {
            get
            {
                return @"server=.;database=backup;user id=sa;password=damaodf";
            }
        }
        /// <summary>
        /// 获取指定类型的描述（用于提取订单状态的枚举名字说明）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyName"></param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
        public static string GetDescription<T>(string propertyName, bool ignoreCase = true)
        {
            string desc = string.Empty;
            PropertyInfo[] peroperties = typeof(T).GetProperties(BindingFlags.Default);
            foreach (PropertyInfo property in peroperties)
            {
                if (ignoreCase)
                {
                    if (property.Name.Equals(propertyName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        object[] objs = property.GetCustomAttributes(typeof(DescriptionAttribute), true);
                        if (objs.Length > 0)
                        {
                            desc = ((DescriptionAttribute)objs[0]).Description;
                        }
                        break;
                    }
                }
                else
                {
                    if (property.Name.Equals(propertyName, StringComparison.InvariantCulture))
                    {
                        object[] objs = property.GetCustomAttributes(typeof(DescriptionAttribute), true);
                        if (objs.Length > 0)
                        {
                            desc = ((DescriptionAttribute)objs[0]).Description;
                        }
                        break;
                    }
                }
            }
            return desc;
        }
        /// <summary>
        /// 获取制定Http请求的远程客户端IP地址
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public static string GetIP(HttpRequest req)
        {
            return req.UserHostAddress;
        }
        /// <summary>
        /// 获取MD5密文的小写字符串
        /// </summary>
        /// <param name="orgin"></param>
        /// <returns></returns>
        public static string GetMD5(string orgin)
        {
            StringBuilder sb = new StringBuilder();
            using (MD5 md5 = MD5.Create())
            {
                byte[] buffer = Encoding.UTF8.GetBytes(orgin);
                byte[] hash = md5.ComputeHash(buffer);
                foreach (byte i in hash)
                {
                    sb.Append(i.ToString("x2"));
                }
            }
            return sb.ToString();
        }
        /// <summary>  
        /// 时间戳转为DateTime时间格式  
        /// </summary>  
        /// <param name="timeStamp">时间戳格式</param>  
        /// <returns>DateTime时间格式</returns>  
        public static DateTime TimeStampToDateTime(string timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp + "0000");
            TimeSpan toNow = new TimeSpan(lTime);
            return dtStart.Add(toNow);
        }
        /// <summary>  
        /// DateTime时间格式转换为时间戳格式  
        /// </summary>  
        /// <param name="time"> DateTime时间格式</param>  
        /// <returns>时间戳格式</returns>  
        public static string DateTimeToTimeStamp(System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            long lTime = (long)(time - startTime).TotalMilliseconds;
            return lTime.ToString();
        }
    }
}
