using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;

namespace TopFashion
{
    public class SMSLogic
    {
        /// <summary>
        /// 群发短信的接口
        /// </summary>
        /// <param name="content"></param>
        /// <param name="mobiles"></param>
        /// <param name="greets"></param>
        /// <returns></returns>
        public static bool SendSMS(string content, List<string> mobiles, List<string> greets = null)
        {
            if (string.IsNullOrEmpty(content))
                return false;
            if (mobiles == null || mobiles.Count == 0)
                return false;
            if (greets != null && greets.Count != mobiles.Count)
            {
                return false;
            }
            try
            {
                string msg = content;
                for (int i = 0; i < mobiles.Count; i++)
                {
                    string mobile = mobiles[i];
                    if (greets != null)
                    {
                        msg = greets[i] + ": " + msg;
                    }
                    string err;
                    bool f = CallAssemblyToSendSMS(mobile, msg, out err);//调用发短信的基本接口
                    if (!f)
                        WriteLog.CreateLog("发送短信", "SMSLogic.SendSMS", "error", err);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static bool CallAssemblyToSendSMS(string mobile, string msg, out string err)
        {
            err = "";
            string SMSApi = Configs.SMSApi;
            if (SMSApi != "")
            {
                string[] apiStr = SMSApi.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                if (apiStr.Length == 2)
                {
                    string SendMethodName = apiStr[0].Trim();
                    string typeFullName = apiStr[1].Trim();
                    string assFilename = apiStr[2].Trim();
                    string assFile = AppDomain.CurrentDomain.BaseDirectory + assFilename;
                    if (File.Exists(assFile))
                    {
                        Assembly ass = Assembly.LoadFrom(assFile);
                        if (ass != null)
                        {
                            Type type = ass.GetType(typeFullName, false, true);
                            if (type != null)
                            {
                                BindingFlags bf = BindingFlags.Static | BindingFlags.Public;
                                object[] args = { mobile, msg };
                                try
                                {
                                    type.InvokeMember(SendMethodName, bf, null, null, args);
                                    return true;
                                }
                                catch (Exception e)
                                {
                                    err = "调用外部程序集接口失败：" + e.Message;
                                }
                            }
                        }
                    }
                }
            }
            if (err != "")
                err += "，向号码[" + mobile + "]发送短信失败！";
            else
                err = "向号码[" + mobile + "]发送短信失败！";
            return false;
        }
    }
}
