using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Xml;
using System.Data.SqlClient;
using System.IO;

namespace TopFashion
{
    public static class Configs
    {
        public static bool SaveConnectionString(string name, SqlConnectionStringBuilder scsb, string configPath = null)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + AppDomain.CurrentDomain.FriendlyName + ".config";
            if (!string.IsNullOrEmpty(configPath))
                path = configPath;
            if (!File.Exists(path))
                return false;
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(path);
            XmlNode xNode;
            XmlElement xElem;
            xNode = xDoc.SelectSingleNode("//connectionStrings");
            if (xNode != null)
            {
                xElem = (XmlElement)xNode.SelectSingleNode("//add[@name='" + name + "']");
                if (xElem != null)
                {
                    xElem.SetAttribute("connectionString", string.Format("Data Source={0};User ID={1};Password={2};Initial Catalog={3}", scsb.DataSource, scsb.UserID, scsb.Password, scsb.InitialCatalog));
                    xDoc.Save(path);
                    return true;
                }
            }
            return false;
        }

        public static string GetConnectionString(string name, string configPath = null)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + AppDomain.CurrentDomain.FriendlyName + ".config";
            if (!string.IsNullOrEmpty(configPath))
                path = configPath;
            if (!File.Exists(path))
                return "";
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(path);
            XmlNode xNode;
            XmlElement xElem;
            xNode = xDoc.SelectSingleNode("//connectionStrings");
            if (xNode != null)
            {
                xElem = (XmlElement)xNode.SelectSingleNode("//add[@name='" + name + "']");
                if (xElem != null)
                {
                    string s = xElem.GetAttribute("connectionString");
                    return s;
                }
            }
            return "";
        }

        public static string ConnString
        {
            get
            {
                //string connString = string.Empty;
                //ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["connString"];
                //if (settings != null)
                //{
                //    connString = settings.ConnectionString;
                //}
                //return connString;
                return GetConnectionString("connString");
            }
        }

        public static string BackupConnString
        {
            get
            {
                //string connString = string.Empty;
                //ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["backupConnString"];
                //if (settings != null)
                //{
                //    connString = settings.ConnectionString;
                //}
                //return connString;
                return GetConnectionString("backupConnString");
            }
        }

        public static string SqlExcept
        {
            get
            {
                string val = string.Empty;
                string raw = ConfigurationManager.AppSettings["except"];
                if (!string.IsNullOrEmpty(raw))
                    val = raw;
                return val;
            }
        }

        public static string SqlOther
        {
            get
            {
                string val = string.Empty;
                string raw = ConfigurationManager.AppSettings["other"];
                if (!string.IsNullOrEmpty(raw))
                    val = raw;
                return val;
            }
        }

        public static string ExportSite
        {
            get
            {
                string val = string.Empty;
                string raw = ConfigurationManager.AppSettings["exportExcelSite"];
                if (!string.IsNullOrEmpty(raw))
                    val = raw;
                return val;
            }
        }

        public static string Auth
        {
            get
            {
                string val = string.Empty;
                string raw = ConfigurationManager.AppSettings["auth"];
                if (!string.IsNullOrEmpty(raw))
                    val = raw;
                return val;
            }
        }

        public static string RemoteAuth
        {
            get
            {
                string val = string.Empty;
                string raw = ConfigurationManager.AppSettings["RemoteAuth"];
                if (!string.IsNullOrEmpty(raw))
                    val = raw;
                return val;
            }
        }

        public static int StaffStatus
        {
            get
            {
                int val = 1;
                int R;
                string raw = ConfigurationManager.AppSettings["staffStatus"];
                if (!string.IsNullOrEmpty(raw) && int.TryParse(raw, out R))
                    val = R;
                return val;
            }
        }

        //public static int SmsAlertTypeStaff
        //{
        //    get
        //    {
        //        int val = 1;
        //        int R;
        //        string raw = ConfigurationManager.AppSettings["smsAlertTypeStaff"];
        //        if (!string.IsNullOrEmpty(raw) && int.TryParse(raw, out R))
        //            val = R;
        //        return val;
        //    }
        //}

        //public static int SmsAlertTypeMember
        //{
        //    get
        //    {
        //        int val = 2;
        //        int R;
        //        string raw = ConfigurationManager.AppSettings["smsAlertTypeMember"];
        //        if (!string.IsNullOrEmpty(raw) && int.TryParse(raw, out R))
        //            val = R;
        //        return val;
        //    }
        //}

        public static string MobileNumberRegx
        {
            get
            {
                string val = "^[1][358][0-9]{9}$";
                string raw = ConfigurationManager.AppSettings["mobileNumberRegx"];
                if (!string.IsNullOrEmpty(raw))
                    val = raw;
                return val;
            }
        }

        public static string AdminName
        {
            get
            {
                string val = "kell";
                string raw = ConfigurationManager.AppSettings["AdminName"];
                if (!string.IsNullOrEmpty(raw))
                    val = raw;
                return val;
            }
        }

        public static string SMSApi
        {
            get
            {
                string val = "";
                string raw = ConfigurationManager.AppSettings["SMSApi"];
                if (!string.IsNullOrEmpty(raw))
                    val = raw;
                return val;
            }
        }

        public static bool UseObsolete
        {
            get
            {
                bool val = false;
                string raw = ConfigurationManager.AppSettings["UseObsolete"];
                if (!string.IsNullOrEmpty(raw) && raw == "1")
                    val = true;
                return val;
            }
        }
    }
}
