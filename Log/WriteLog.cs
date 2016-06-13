using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Net;
using System.Management;
using System.Windows.Forms;

namespace TopFashion
{
    public static class WriteLog
    {
        /// <summary>
        /// 取得一个文本文件的编码方式。如果无法在文件头部找到有效的前导符，Encoding.Default将被返回。
        /// </summary>
        /// <param name="fileName">文件名。</param>
        /// <returns></returns>

        public static Encoding GetEncoding(string fileName)
        {
            return GetEncoding(fileName, Encoding.Default);
        }

        /// <summary>
        /// 取得一个文本文件流的编码方式。
        /// </summary>
        /// <param name="stream">文本文件流。</param>
        /// <returns></returns>
        public static Encoding GetEncoding(FileStream stream)
        {
            return GetEncoding(stream, Encoding.Default);
        }

        // 取得一个文本文件的编码方式。
        //默认编码方式。当该方法无法从文件的头部取得有效的前导符时，将返回该编码方式。
        public static Encoding GetEncoding(string fileName, Encoding defaultEncod)
        {
            FileStream fStream = new FileStream(fileName, FileMode.Open);
            Encoding targetEncoding = GetEncoding(fStream, defaultEncod);
            fStream.Close();
            return targetEncoding;
        }

        // 取得一个文本文件流的编码方式。
        //默认编码方式。当该方法无法从文件的头部取得有效的前导符时，将返回该编码方式。
        public static Encoding GetEncoding(FileStream fStream, Encoding defaultEncoding)
        {
            Encoding targetEncoding = defaultEncoding;
            if (fStream != null && fStream.Length >= 2)
            {
                //保存文件流的前4个字节
                byte byte1 = 0;
                byte byte2 = 0;
                byte byte3 = 0;
                byte byte4 = 0;

                //保存当前Seek位置
                long origPos = fStream.Seek(0, SeekOrigin.Begin);
                fStream.Seek(0, SeekOrigin.Begin);
                int nByte = fStream.ReadByte();
                byte1 = Convert.ToByte(nByte);
                byte2 = Convert.ToByte(fStream.ReadByte());
                if (fStream.Length >= 3)
                {
                    byte3 = Convert.ToByte(fStream.ReadByte());
                }

                if (fStream.Length >= 4)
                {
                    byte4 = Convert.ToByte(fStream.ReadByte());
                }

                if (byte1 == 0xFE && byte2 == 0xFF)//UnicodeBe
                {
                    targetEncoding = Encoding.BigEndianUnicode;
                }

                if (byte1 == 0xFF && byte2 == 0xFE && byte3 != 0xFF)//Unicode
                {
                    targetEncoding = Encoding.Unicode;
                }

                if (byte1 == 0xEF && byte2 == 0xBB && byte3 == 0xBF)//UTF8
                {
                    targetEncoding = Encoding.UTF8;
                }
                //恢复Seek位置　　　

                fStream.Seek(origPos, SeekOrigin.Begin);
            }
            return targetEncoding;
        }

        /// <summary>
        /// 插入数据到系统日志表
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="addr">日志出处</param>
        /// <param name="logType">日志类型</param>
        /// <param name="msg">日志内容描述</param>
        public static void CreateLog(string userName, string addr, string logType, string msg)
        {
            string ipStr = "";
            string hostInfo = Dns.GetHostName();
            IPAddress[] ips = Dns.GetHostAddresses(hostInfo);
            foreach (IPAddress ip in ips)
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    ipStr = ip.ToString();
                }
            }
            string macStr = "";
            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection moc = mc.GetInstances();
            foreach (ManagementObject mo in moc)
            {
                if (mo["IPEnabled"].ToString() == "True")
                {
                    macStr = mo["MacAddress"].ToString();
                }
            }
            string sqlstr = "insert into TF_SystemLog(UserName,UserIP,UserMac,Addr,LogType,Msg) values (@userName,@userIP,@userMac,@addr,@logType,@msg)";

            SqlParameter[] parms = new SqlParameter[]
            { 
                new SqlParameter("@userName", userName),
                new SqlParameter("@userIP", ipStr),
                new SqlParameter("@userMac", macStr),
                new SqlParameter("@addr", addr),
                new SqlParameter("@logType", logType),
                new SqlParameter("@msg", msg)
            };
            ExecuteSql(sqlstr, parms);
        }

        public static DataTable GetLogs(int index, int size, string logType, out int allCount)
        {
            allCount = 0;
            string sql = "select COUNT(*) from TF_SystemLog";
            if (!string.IsNullOrEmpty(logType))
                sql = "select COUNT(*) from TF_SystemLog where LogType='" + logType + "'";
            object obj = GetSingle(sql);
            if (obj != null && obj != DBNull.Value)
            {
                allCount = Convert.ToInt32(obj);
            }
            string sqlstr = "";
            if (!string.IsNullOrEmpty(logType))
                sqlstr = "select top " + size + " UserName,UserIP,UserMac,Addr,LogType,Msg,LogTime from TF_SystemLog where LogType='" + logType + "' and ID not in (select top " + Convert.ToString(size * index) + " ID from TF_SystemLog where LogType='" + logType + "' order by ID desc) order by ID desc";
            else
                sqlstr = "select top " + size + " UserName,UserIP,UserMac,Addr,LogType,Msg,LogTime from TF_SystemLog where ID not in (select top " + Convert.ToString(size * index) + " ID from TF_SystemLog order by ID desc) order by ID desc";
            return Query(sqlstr);
        }

        public static List<string> GetLogTypes()
        {
            List<string> types = new List<string>();
            string sqlstr = "select distinct LogType from TF_SystemLog";
            DataTable dt = Query(sqlstr);
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    types.Add(dt.Rows[i][0].ToString());
                }
            }
            return types;
        }

        public static void ClearLog()
        {
            string sqlstr = "delete from TF_SystemLog";
            ExecuteSql(sqlstr);
        }

        private static object GetSingle(string SQLString)
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["connString"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(SQLString, connection))
                {
                    try
                    {
                        connection.Open();
                        object obj = cmd.ExecuteScalar();
                        if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                        {
                            return null;
                        }
                        else
                        {
                            return obj;
                        }
                    }
                    catch (System.Data.SqlClient.SqlException e)
                    {
                        connection.Close();
                        MessageBox.Show("数据库操作失败：" + e.Message);
                        return -1;
                    }
                }
            }
        }

        private static DataTable Query(string SQLString)
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["connString"].ConnectionString))
            {
                DataTable dt = new DataTable();
                try
                {
                    connection.Open();
                    SqlDataAdapter command = new SqlDataAdapter(SQLString, connection);
                    command.Fill(dt);
                    connection.Close();
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    MessageBox.Show("数据库操作失败：" + ex.Message);
                }
                return dt;
            }
        }
        /// <summary>
        /// 执行SQL语句，返回影响的记录数
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <returns>影响的记录数</returns>
        private static int ExecuteSql(string SQLString, params SqlParameter[] cmdParms)
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["connString"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                        int rows = cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                        return rows;
                    }
                    catch (System.Data.SqlClient.SqlException e)
                    {
                        return -1;
                    }
                }
            }
        }
        private static void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, string cmdText, SqlParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            if (trans != null)
                cmd.Transaction = trans;
            cmd.CommandType = CommandType.Text;//cmdType;
            if (cmdParms != null)
            {
                foreach (SqlParameter parameter in cmdParms)
                {
                    if (parameter != null)
                        if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) &&
                            (parameter.Value == null))
                        {
                            parameter.Value = DBNull.Value;
                        }
                    cmd.Parameters.Add(parameter);
                }
            }
        }

        /// <summary>
        /// 生成TXT文件,记录事件处理结果和错误信息
        /// </summary>
        /// <param name="filePath">错误出处的类和方法</param>
        /// <param name="addr">错误类型</param>
        /// <param name="content">错误内容</param>
        [Obsolete("WinForm程序里，不再用该本地方法", false)]
        public static void CreateLog(string errorSource, string errorType, string errorContent)
        {
            DateTime dt = DateTime.Now;
            string time = dt.ToString("yyyy-MM-dd_HH_mm_ss");
            Random rand = new Random();
            string num = rand.Next(10000, 99999).ToString();
            string filename = time + "_" + num + ".txt"; //文件命名，随机数加当前时间

            string path = "LogReporter";
            if (errorType.Equals("error", StringComparison.InvariantCultureIgnoreCase))
                path = AppDomain.CurrentDomain.BaseDirectory + "LogReporter\\Error";
            else if (errorType.Equals("log", StringComparison.InvariantCultureIgnoreCase))
                path = AppDomain.CurrentDomain.BaseDirectory + "LogReporter\\Log";
            else
                path = AppDomain.CurrentDomain.BaseDirectory + "LogReporter\\" + errorType;
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            string filepath = path + "\\" + filename;
            StreamWriter sWrite = new StreamWriter(filepath, false, Encoding.UTF8);
            sWrite.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+ "[" + errorSource + "]>>" + errorContent);
            sWrite.Close();
        }

        //读取日志信息
        public static string ReadLog(string filepath)
        {
            string content = string.Empty;
            //string[] directorys=Directory.GetDirectories(filepath,"*.txt",SearchOption.AllDirectories);
            string[] files = Directory.GetFiles(filepath, "*.txt");
            if (files.Length > 0)
            {
                foreach (string fl in files)
                {
                    FileInfo fi = new FileInfo(fl);
                    StreamReader sread = new StreamReader(fl, Encoding.UTF8);
                    content += sread.ReadToEnd();
                    sread.Close();
                }
            }
            return content;
        }
    }
}
