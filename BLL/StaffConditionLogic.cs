using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace TopFashion
{
    public class StaffConditionLogic
    {        
        SQLDBHelper sqlHelper;
        static StaffConditionLogic instance;
        public static StaffConditionLogic GetInstance()
        {
            if (instance == null)
                instance = new StaffConditionLogic();

            return instance;
        }

        private StaffConditionLogic()
        {
            sqlHelper = new SQLDBHelper();
        }

        public StaffCondition GetStaffCondition(int id)
        {
            string sql = "select * from TF_StaffCondition where ID=" + id;
            DataTable dt = sqlHelper.Query(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                StaffCondition element = new StaffCondition();
                element.ID = id;
                element.状态 = dt.Rows[0]["状态"].ToString();
                element.是否在职 = Convert.ToBoolean(dt.Rows[0]["是否在职"]);
                element.备注 = dt.Rows[0]["备注"].ToString();
                return element;
            }
            return null;
        }

        public StaffCondition GetStaffConditionByName(string name)
        {
            string sql = "select * from TF_StaffCondition where 状态='" + name + "'";
            DataTable dt = sqlHelper.Query(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                StaffCondition element = new StaffCondition();
                element.ID = Convert.ToInt32(dt.Rows[0]["ID"]);
                element.状态 = dt.Rows[0]["状态"].ToString();
                element.是否在职 = Convert.ToBoolean(dt.Rows[0]["是否在职"]);
                element.备注 = dt.Rows[0]["备注"].ToString();
                return element;
            }
            return null;
        }

        public List<StaffCondition> GetAllStaffConditions()
        {
            List<StaffCondition> elements = new List<StaffCondition>();
            string sql = "select * from TF_StaffCondition";
            DataTable dt = sqlHelper.Query(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    StaffCondition element = new StaffCondition();
                    element.ID = Convert.ToInt32(dt.Rows[i]["ID"]);
                    element.状态 = dt.Rows[i]["状态"].ToString();
                    element.是否在职 = Convert.ToBoolean(dt.Rows[i]["是否在职"]);
                    element.备注 = dt.Rows[i]["备注"].ToString();
                    elements.Add(element);
                }
            }
            return elements;
        }

        public int AddStaffCondition(StaffCondition element)
        {
            string sql = "insert into TF_StaffCondition (状态, 是否在职, 备注) values ('" + element.状态 + "', " + (element.是否在职 ? "1" : "0") + ", '" + element.备注 + "'); select SCOPE_IDENTITY()";
            object obj = sqlHelper.ExecuteSqlReturn(sql);
            int R;
            if (obj != null && obj != DBNull.Value && int.TryParse(obj.ToString(), out R))
                return R;
            else
                return 0;
        }

        public bool UpdateStaffCondition(StaffCondition element)
        {
            string sql = "update TF_StaffCondition set 状态='" + element.状态 + "', 是否在职=" + (element.是否在职 ? "1" : "0") + ", 备注='" + element.备注 + "' where ID=" + element.ID;
            int r = sqlHelper.ExecuteSql(sql);
            return r > 0;
        }

        public bool DeleteStaffCondition(StaffCondition element)
        {
            string sql = "delete from TF_StaffCondition where ID=" + element.ID;
            int r = sqlHelper.ExecuteSql(sql);
            return r > 0;
        }
        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool UpgradeList(List<StaffCondition> list)
        {
            int errCount = 0;
            foreach (StaffCondition element in list)
            {
                string sqlStr = "if exists (select 1 from TF_StaffCondition where ID=" + element.ID + ") update TF_StaffCondition set 状态='" + element.状态 + "', 是否在职=" + (element.是否在职 ? "1" : "0") + ", 备注='" + element.备注 + "' where ID=" + element.ID + " else insert into TF_StaffCondition (状态, 是否在职, 备注) values ('" + element.状态 + "', " + (element.是否在职 ? "1" : "0") + ", '" + element.备注 + "')";
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
            return sqlHelper.Exists("select 1 from TF_StaffCondition where 状态='" + name + "'");
        }

        /// <summary>
        /// 是否存在出了自己以外的同名
        /// </summary>
        /// <param name="name"></param>
        /// <param name="myId"></param>
        /// <returns></returns>
        public bool ExistsNameOther(string name, int myId)
        {
            return sqlHelper.Exists("select 1 from TF_StaffCondition where ID!=" + myId + " and 状态='" + name + "'");
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
                return sqlHelper.Exists("select 1 from TF_StaffCondition " + w);
            }
            return false;
        }

        public DataTable GetStaffConditions(string where)
        {
            DataTable dt = null;
            string w = "";
            if (!string.IsNullOrEmpty(where))
            {
                w = where.Trim().ToLower();
                if (!w.StartsWith("where "))
                    w = "where " + w;
            }
            string sql = "select * from TF_StaffCondition " + w;
            dt = sqlHelper.Query(sql);
            return dt;
        }
    }
}
