using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace TopFashion
{
    public class FollowupTypeLogic
    {        
        SQLDBHelper sqlHelper;
        static FollowupTypeLogic instance;
        public static FollowupTypeLogic GetInstance()
        {
            if (instance == null)
                instance = new FollowupTypeLogic();

            return instance;
        }

        private FollowupTypeLogic()
        {
            sqlHelper = new SQLDBHelper();
        }

        public FollowupType GetFollowupType(int id)
        {
            string sql = "select * from TF_FollowupType where ID=" + id;
            DataTable dt = sqlHelper.Query(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                FollowupType element = new FollowupType();
                element.ID = id;
                element.方式 = dt.Rows[0]["方式"].ToString();
                element.Flag = Convert.ToBoolean(dt.Rows[0]["Flag"]);
                element.备注 = dt.Rows[0]["备注"].ToString();
                return element;
            }
            return null;
        }

        public List<FollowupType> GetAllFollowupTypes()
        {
            List<FollowupType> elements = new List<FollowupType>();
            string sql = "select * from TF_FollowupType";
            DataTable dt = sqlHelper.Query(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    FollowupType element = new FollowupType();
                    element.ID = Convert.ToInt32(dt.Rows[i]["ID"]);
                    element.方式 = dt.Rows[i]["方式"].ToString();
                    element.Flag = Convert.ToBoolean(dt.Rows[i]["Flag"]);
                    element.备注 = dt.Rows[i]["备注"].ToString();
                    elements.Add(element);
                }
            }
            return elements;
        }

        public int AddFollowupType(FollowupType element)
        {
            string sql = "insert into TF_FollowupType (方式, Flag, 备注) values ('" + element.方式 + "', " + (element.Flag ? "1" : "0") + ", '" + element.备注 + "'); select SCOPE_IDENTITY()";
            object obj = sqlHelper.ExecuteSqlReturn(sql);
            int R;
            if (obj != null && obj != DBNull.Value && int.TryParse(obj.ToString(), out R))
                return R;
            else
                return 0;
        }

        public bool UpdateFollowupType(FollowupType element)
        {
            string sql = "update TF_FollowupType set 方式='" + element.方式 + "', Flag=" + (element.Flag ? "1" : "0") + ", 备注='" + element.备注 + "' where ID=" + element.ID;
            int r = sqlHelper.ExecuteSql(sql);
            return r > 0;
        }

        public bool DeleteFollowupType(FollowupType element)
        {
            string sql = "delete from TF_FollowupType where ID=" + element.ID;
            int r = sqlHelper.ExecuteSql(sql);
            return r > 0;
        }
        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool UpgradeList(List<FollowupType> list)
        {
            int errCount = 0;
            foreach (FollowupType element in list)
            {
                string sqlStr = "if exists (select 1 from TF_FollowupType where ID=" + element.ID + ") update TF_FollowupType set 方式='" + element.方式 + "', Flag=" + (element.Flag ? "1" : "0") + ", 备注='" + element.备注 + "' where ID=" + element.ID + " else insert into TF_FollowupType (方式, Flag, 备注) values ('" + element.方式 + "', " + (element.Flag ? "1" : "0") + ", '" + element.备注 + "')";
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
            return sqlHelper.Exists("select 1 from TF_FollowupType where 方式='" + name + "'");
        }

        /// <summary>
        /// 是否存在出了自己以外的同名
        /// </summary>
        /// <param name="name"></param>
        /// <param name="myId"></param>
        /// <returns></returns>
        public bool ExistsNameOther(string name, int myId)
        {
            return sqlHelper.Exists("select 1 from TF_FollowupType where ID!=" + myId + " and 方式='" + name + "'");
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
                return sqlHelper.Exists("select 1 from TF_FollowupType " + w);
            }
            return false;
        }

        public DataTable GetFollowupTypes(string where)
        {
            DataTable dt = null;
            string w = "";
            if (!string.IsNullOrEmpty(where))
            {
                w = where.Trim().ToLower();
                if (!w.StartsWith("where "))
                    w = "where " + w;
            }
            string sql = "select * from TF_FollowupType " + w;
            dt = sqlHelper.Query(sql);
            return dt;
        }
    }
}
