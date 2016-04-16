using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace TopFashion
{
    public class AlertLogic
    {        
        SQLDBHelper sqlHelper;
        static AlertLogic instance;
        public static AlertLogic GetInstance()
        {
            if (instance == null)
                instance = new AlertLogic();

            return instance;
        }

        private AlertLogic()
        {
            sqlHelper = new SQLDBHelper();
        }

        public Alert GetAlert(int id)
        {
            string sql = "select * from TF_Alert where ID=" + id;
            DataTable dt = sqlHelper.Query(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                Alert element = new Alert();
                element.ID = id;
                element.提醒项目 = dt.Rows[0]["提醒项目"].ToString();
                element.提醒时间 = Convert.ToDateTime(dt.Rows[0]["提醒时间"]);
                element.提醒方式 = (提醒方式)Enum.ToObject(typeof(提醒方式), Convert.ToInt32(dt.Rows[0]["提醒方式"]));
                element.Flag = Convert.ToInt32(dt.Rows[0]["Flag"]);
                element.提醒对象 = dt.Rows[0]["提醒对象"].ToString();
                element.备注 = dt.Rows[0]["备注"].ToString();
                return element;
            }
            return null;
        }

        public List<Alert> GetAllAlerts()
        {
            List<Alert> elements = new List<Alert>();
            string sql = "select * from TF_Alert";
            DataTable dt = sqlHelper.Query(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Alert element = new Alert();
                    element.ID = Convert.ToInt32(dt.Rows[i]["ID"]);
                    element.提醒项目 = dt.Rows[i]["提醒项目"].ToString();
                    element.提醒时间 = Convert.ToDateTime(dt.Rows[i]["提醒时间"]);
                    element.提醒方式 = (提醒方式)Enum.ToObject(typeof(提醒方式), Convert.ToInt32(dt.Rows[i]["提醒方式"]));
                    element.Flag = Convert.ToInt32(dt.Rows[i]["Flag"]);
                    element.提醒对象 = dt.Rows[i]["提醒对象"].ToString();
                    element.备注 = dt.Rows[i]["备注"].ToString();
                    elements.Add(element);
                }
            }
            return elements;
        }

        public List<Alert> GetAlertsByType(int alertType, bool unRead = true)
        {
            List<Alert> elements = new List<Alert>();
            string sql = "select * from TF_Alert where 提醒方式=" + alertType + " and Flag=" + (unRead ? "0" : "1");
            DataTable dt = sqlHelper.Query(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Alert element = new Alert();
                    element.ID = Convert.ToInt32(dt.Rows[i]["ID"]);
                    element.提醒项目 = dt.Rows[i]["提醒项目"].ToString();
                    element.提醒时间 = Convert.ToDateTime(dt.Rows[i]["提醒时间"]);
                    element.提醒方式 = (提醒方式)Enum.ToObject(typeof(提醒方式), Convert.ToInt32(dt.Rows[i]["提醒方式"]));
                    element.Flag = Convert.ToInt32(dt.Rows[i]["Flag"]);
                    element.提醒对象 = dt.Rows[i]["提醒对象"].ToString();
                    element.备注 = dt.Rows[i]["备注"].ToString();
                    elements.Add(element);
                }
            }
            return elements;
        }

        public List<Alert> GetSysAlertsByUser(User user, bool unRead = true)
        {
            List<Alert> elements = new List<Alert>();
            string sql = "select * from TF_Alert where ','+提醒对象+',' like '%," + user.ID + ",%' and 提醒方式=0 and Flag=" + (unRead ? "0" : "1");
            DataTable dt = sqlHelper.Query(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Alert element = new Alert();
                    element.ID = Convert.ToInt32(dt.Rows[i]["ID"]);
                    element.提醒项目 = dt.Rows[i]["提醒项目"].ToString();
                    element.提醒时间 = Convert.ToDateTime(dt.Rows[i]["提醒时间"]);
                    element.提醒方式 = (提醒方式)Enum.ToObject(typeof(提醒方式), Convert.ToInt32(dt.Rows[i]["提醒方式"]));
                    element.Flag = Convert.ToInt32(dt.Rows[i]["Flag"]);
                    element.提醒对象 = dt.Rows[i]["提醒对象"].ToString();
                    element.备注 = dt.Rows[i]["备注"].ToString();
                    elements.Add(element);
                }
            }
            return elements;
        }

        public List<Alert> GetExecAlertsByUser(User user, bool unRead = true)
        {
            List<Alert> elements = new List<Alert>();
            string sql = "select * from TF_Alert where ','+提醒对象+',' like '%," + user.ID + ",%' and 提醒方式=3 and Flag=" + (unRead ? "0" : "1");
            DataTable dt = sqlHelper.Query(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Alert element = new Alert();
                    element.ID = Convert.ToInt32(dt.Rows[i]["ID"]);
                    element.提醒项目 = dt.Rows[i]["提醒项目"].ToString();
                    element.提醒时间 = Convert.ToDateTime(dt.Rows[i]["提醒时间"]);
                    element.提醒方式 = (提醒方式)Enum.ToObject(typeof(提醒方式), Convert.ToInt32(dt.Rows[i]["提醒方式"]));
                    element.Flag = Convert.ToInt32(dt.Rows[i]["Flag"]);
                    element.提醒对象 = dt.Rows[i]["提醒对象"].ToString();
                    element.备注 = dt.Rows[i]["备注"].ToString();
                    elements.Add(element);
                }
            }
            return elements;
        }

        public List<Alert> GetApprAlertsByUser(User user, bool unRead = true)
        {
            List<Alert> elements = new List<Alert>();
            string sql = "select * from TF_Alert where ','+提醒对象+',' like '%," + user.ID + ",%' and 提醒方式=4 and Flag=" + (unRead ? "0" : "1");
            DataTable dt = sqlHelper.Query(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Alert element = new Alert();
                    element.ID = Convert.ToInt32(dt.Rows[i]["ID"]);
                    element.提醒项目 = dt.Rows[i]["提醒项目"].ToString();
                    element.提醒时间 = Convert.ToDateTime(dt.Rows[i]["提醒时间"]);
                    element.提醒方式 = (提醒方式)Enum.ToObject(typeof(提醒方式), Convert.ToInt32(dt.Rows[i]["提醒方式"]));
                    element.Flag = Convert.ToInt32(dt.Rows[i]["Flag"]);
                    element.提醒对象 = dt.Rows[i]["提醒对象"].ToString();
                    element.备注 = dt.Rows[i]["备注"].ToString();
                    elements.Add(element);
                }
            }
            return elements;
        }

        public int AddAlert(Alert element)
        {
            string sql = "insert into TF_Alert (提醒项目, 提醒时间, 提醒方式, 提醒对象, Flag, 备注) values ('" + element.提醒项目 + "', '" + element.提醒时间 + "', " + (int)element.提醒方式 + ", '" + element.提醒对象 + "', " + element.Flag + ", '" + element.备注 + "'); select SCOPE_IDENTITY()";
            object obj = sqlHelper.ExecuteSqlReturn(sql);
            int R;
            if (obj != null && obj != DBNull.Value && int.TryParse(obj.ToString(), out R))
                return R;
            else
                return 0;
        }

        public bool UpdateAlert(Alert element)
        {
            string sql = "update TF_Alert set 提醒项目='" + element.提醒项目 + "', 提醒时间='" + element.提醒时间 + "', 提醒方式=" + (int)element.提醒方式 + ", 提醒对象='" + element.提醒对象 + "', Flag=" + element.Flag + ", 备注='" + element.备注 + "' where ID=" + element.ID;
            int r = sqlHelper.ExecuteSql(sql);
            return r > 0;
        }

        public bool SetFlag(int id, int flag)
        {
            string sql = "update TF_Alert set Flag=" + flag + " where ID=" + id;
            int r = sqlHelper.ExecuteSql(sql);
            return r > 0;
        }

        public bool DeleteAlert(Alert element)
        {
            string sql = "delete from TF_Alert where ID=" + element.ID;
            int r = sqlHelper.ExecuteSql(sql);
            return r > 0;
        }
        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool UpgradeList(List<Alert> list)
        {
            int errCount = 0;
            foreach (Alert element in list)
            {
                string sqlStr = "if exists (select 1 from TF_Alert where ID=" + element.ID + ") update TF_Alert set 提醒项目='" + element.提醒项目 + "', 提醒时间='" + element.提醒时间 + "', 提醒方式=" + (int)element.提醒方式 + ", 提醒对象='" + element.提醒对象 + "', Flag=" + element.Flag + ", 备注='" + element.备注 + "' where ID=" + element.ID + " else insert into TF_Alert (提醒项目, 提醒时间, 提醒方式, 提醒对象, Flag, 备注) values ('" + element.提醒项目 + "' ,'" + element.提醒时间 + "', " + (int)element.提醒方式 + ", '" + element.提醒对象 + "', " + element.Flag + ", '" + element.备注 + "')";
                try
                {
                    sqlHelper.ExecuteSql(sqlStr);
                }
                catch (Exception)
                {
                    errCount++;
                }
            }
            return errCount == 0;
        }

        /// <summary>
        /// 是否存在同名
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool ExistsName(string name)
        {
            return sqlHelper.Exists("select 1 from TF_Alert where 提醒项目='" + name + "'");
        }

        /// <summary>
        /// 是否存在出了自己以外的同名
        /// </summary>
        /// <param name="name"></param>
        /// <param name="myId"></param>
        /// <returns></returns>
        public bool ExistsNameOther(string name, int myId)
        {
            return sqlHelper.Exists("select 1 from TF_Alert where ID!=" + myId + " and 提醒项目='" + name + "'");
        }

        /// <summary>
        /// 是否存在指定条件的记录
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool ExistsWhere(string where)
        {
            if (!string.IsNullOrEmpty(where))
            {
                string w = where.Trim().ToLower();
                if (!w.StartsWith("where "))
                    w = "where " + w;
                return sqlHelper.Exists("select 1 from TF_Alert " + w);
            }
            return false;
        }

        public DataTable GetAlerts(string where)
        {
            DataTable dt = null;
            string w = "";
            if (!string.IsNullOrEmpty(where))
            {
                w = where.Trim().ToLower();
                if (!w.StartsWith("where "))
                    w = "where " + w;
            }
            string sql = "select * from TF_View_Alert " + w + " order by 提醒时间 desc";
            dt = sqlHelper.Query(sql);
            return dt;
        }
    }
}
