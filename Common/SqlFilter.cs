using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Configuration;
using System.IO;

namespace TopFashion
{
    /// <summary>
    /// SQL过滤类
    /// </summary>
    public class SqlFilter : IHttpModule
    {
        /// <summary>
        /// 是否检查ViewState
        /// </summary>
        public static bool CheckViewState;
        /// <summary>
        /// 是否检查Cookies
        /// </summary>
        public static bool CheckCookies;
        /// <summary>
        /// 实现接口的模块名
        /// </summary>
        public String ModuleName
        {
            get { return "SqlFilter"; }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="application"></param>
        public void Init(HttpApplication application)
        {
            application.AuthenticateRequest += new EventHandler(application_AuthenticateRequest);
        }
        /// <summary>
        /// 释放
        /// </summary>
        public void Dispose()
        {

        }

        /// <summary>
        /// 当有数据时交时，触发事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void application_AuthenticateRequest(object sender, EventArgs e)
        {
            HttpContext context = ((HttpApplication)sender).Context;

            if (!context.Request.RawUrl.ToLower().StartsWith("/pages/systemmanage/our.aspx") && !context.Request.RawUrl.ToLower().StartsWith("/pages/systemmanage/wechatindex.aspx"))//排除关于我们和微信首页的信息提交造成的SQL注入
            {
                //遍历Post参数，隐藏域除外
                foreach (string i in context.Request.Form)
                {
                    if (i == "__VIEWSTATE" && !CheckViewState) continue;
                    if (this.IsUnsafe(context.Request.Form[i]))
                    {
                        Log("[" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "]Form: " + context.Request.Form[i] + " (IP:" + CommonConsts.GetIP(context.Request) + ")");
                        context.Response.Redirect("/Error.aspx?err=Form");
                        context.Response.End();
                    }
                }
                //遍历Get参数。
                foreach (string i in context.Request.QueryString)
                {
                    if (this.IsUnsafe(context.Request.QueryString[i]))
                    {
                        Log("[" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "]QueryString: " + context.Request.QueryString[i] + " (IP:" + CommonConsts.GetIP(context.Request) + ")");
                        context.Response.Redirect("/Error.aspx?err=QueryString");
                        context.Response.End();
                    }
                }
                if (CheckCookies)
                {
                    //遍历Cookies
                    foreach (string i in context.Request.Cookies)
                    {
                        if (this.IsUnsafe(context.Request.Cookies[i].Value))
                        {
                            Log("[" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "]Cookies: " + context.Request.Cookies[i].Value + " (IP:" + CommonConsts.GetIP(context.Request) + ")");
                            context.Response.Redirect("/Error.aspx?err=Cookies");
                            context.Response.End();
                        }
                        if (context.Request.Cookies[i].Values != null && context.Request.Cookies[i].Values.Count > 0)
                        {
                            for (int j = 0; j < context.Request.Cookies[i].Values.Count; j++)
                            {
                                if (this.IsUnsafe(context.Request.Cookies[i].Values[j]))
                                {
                                    Log("[" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "]Cookies: " + context.Request.Cookies[i].Values[j] + " (IP:" + CommonConsts.GetIP(context.Request) + ")");
                                    context.Response.Redirect("/Error.aspx?err=Cookies");
                                    context.Response.End();
                                }
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 字符串转化为16进制
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string Str2Hex(string str)
        {
            string sHex = string.Empty;
            for (int i = 0; i < str.Length; i++)
            {
                sHex += String.Format("{0:X}", (int)str.Substring(i, 1)[0]);
            }
            return sHex;
        }
        /// <summary>
        /// 16进制转化为字符串
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        public string Hex2Str(string hex)
        {
            string str = string.Empty;
            for (int i = 0; i < hex.Length; i += 2)
            {
                str += (char)Convert.ToInt32(hex.Substring(i, 2), 16);
            }
            return str;
        }

        /// <summary>
        /// SQL注入过滤
        /// </summary>
        /// <param name="sqlText">要过滤的SQL字符串</param>
        /// <returns>如果参数存在不安全字符，则返回true</returns>
        public bool IsUnsafe(string sqlText)
        {
            string word = "exec|insert|select|delete|update|truncate|declare|cmd|and|or|union|join";
            if (sqlText == null)
                return false;
            List<string> words = new List<string>(word.Split('|'));
            string except = Configs.SqlExcept;
            if (!string.IsNullOrEmpty(except))
            {
                string[] excepts = except.Split('|');
                List<string> exp = new List<string>();
                foreach (string t in excepts)
                {
                    exp.Add(t.ToLower());
                }
                for (int s = words.Count - 1; s > -1; s--)
                {
                    if (exp.Contains(words[s]))
                        words.RemoveAt(s);
                }
            }
            foreach (string i in words)
            {
                int index = sqlText.ToLower().IndexOf(i + " ");
                if (index > -1)
                {
                    int testNumeric;
                    if (index > 0 && (sqlText.Substring(index - 1, 1) == ";" || sqlText.Substring(index - 1, 1) == " " || sqlText.Substring(index - 1, 1) == "'" || int.TryParse(sqlText.Substring(index - 1, 1), out testNumeric)))
                    {
                        return true;
                    }
                    else if (index == 0)
                    {
                        return true;
                    }
                }
            }
            string other = Configs.SqlOther;
            if (!string.IsNullOrEmpty(other))
            {
                foreach (string s in other.Split('|'))
                {
                    if (sqlText.ToLower().IndexOf(s.ToLower()) > -1)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// 生成的注入日志路径
        /// </summary>
        public static string ErrText = AppDomain.CurrentDomain.BaseDirectory + "\\SqlInjectRecords.log";
        /// <summary>
        /// 记录下注入的信息到日志中
        /// </summary>
        /// <param name="message"></param>
        public void Log(string message)
        {
            using (FileStream fs = new FileStream(ErrText, FileMode.Append, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.WriteLine(message);
                }
            }
        }
    }
}
