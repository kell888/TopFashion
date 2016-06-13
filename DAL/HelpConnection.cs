using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace TopFashion
{
    public static class HelpConnection
    {
        static string sqlConnStr = Configs.ConnString;
        static string sqlBackupConnStr = Configs.BackupConnString;

        public static string ConnectionString
        {
            get
            {
                string _connectionString = sqlConnStr;
                return _connectionString;
            }
        }

        public static string BackupConnectionString
        {
            get
            {
                string _connectionString = sqlBackupConnStr;
                return _connectionString;
            }
        }

        public static SqlConnection GetConnection(bool backup)
        {
            if (backup)
            {
                if (string.IsNullOrEmpty(sqlBackupConnStr))
                    sqlBackupConnStr = CommonConsts.DefaultBackupConnString;//取默认的连接字符串
                SqlConnection sqlcon = new SqlConnection(sqlBackupConnStr);
                return sqlcon;
            }
            else
            {
                if (string.IsNullOrEmpty(sqlConnStr))
                    sqlConnStr = CommonConsts.DefaultConnString;//取默认的连接字符串
                SqlConnection sqlcon = new SqlConnection(sqlConnStr);
                return sqlcon;
            }
        }

        public static bool TestConnect(string connString)
        {
            SqlConnection conn = new SqlConnection(connString);
            try
            {
                conn.Open();
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                if (conn.State != ConnectionState.Closed)
                    conn.Close();
            }
        }
    }
}
