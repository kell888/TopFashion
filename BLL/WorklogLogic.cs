using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace TopFashion
{
    public class WorklogLogic
    {        
        SQLDBHelper sqlHelper;
        static WorklogLogic instance;
        public static WorklogLogic GetInstance()
        {
            if (instance == null)
                instance = new WorklogLogic();

            return instance;
        }

        private WorklogLogic()
        {
            sqlHelper = new SQLDBHelper();
        }

        public Worklog GetWorklog(int id)
        {
            string sql = "select * from TF_Worklog where ID=" + id;
            DataTable dt = sqlHelper.Query(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                Worklog element = new Worklog();
                element.ID = id;
                element.销售 = StaffLogic.GetInstance().GetStaff(Convert.ToInt32(dt.Rows[0]["销售"]));
                element.日期 = Convert.ToDateTime(dt.Rows[0]["日期"]);
                element.客户 = dt.Rows[0]["客户"].ToString();
                element.电话 = dt.Rows[0]["电话"].ToString();
                element.是否自访 = Convert.ToBoolean(dt.Rows[0]["是否自访"]);
                element.是否老会员 = Convert.ToBoolean(dt.Rows[0]["是否老会员"]);
                element.是否电话拜访 = Convert.ToBoolean(dt.Rows[0]["是否电话拜访"]);
                element.性别 = (性别)Enum.ToObject(typeof(性别), Convert.ToInt32(dt.Rows[0]["性别"]));
                element.意向 = dt.Rows[0]["意向"].ToString();
                element.住址 = dt.Rows[0]["住址"].ToString();
                element.备注 = dt.Rows[0]["备注"].ToString();
                return element;
            }
            return null;
        }

        public List<Worklog> GetAllWorklogs()
        {
            List<Worklog> elements = new List<Worklog>();
            string sql = "select * from TF_Worklog";
            DataTable dt = sqlHelper.Query(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Worklog element = new Worklog();
                    element.ID = Convert.ToInt32(dt.Rows[i]["ID"]);
                    element.销售 = StaffLogic.GetInstance().GetStaff(Convert.ToInt32(dt.Rows[i]["销售"]));
                    element.日期 = Convert.ToDateTime(dt.Rows[i]["日期"]);
                    element.客户 = dt.Rows[i]["客户"].ToString();
                    element.电话 = dt.Rows[i]["电话"].ToString();
                    element.是否自访 = Convert.ToBoolean(dt.Rows[i]["是否自访"]);
                    element.是否老会员 = Convert.ToBoolean(dt.Rows[i]["是否老会员"]);
                    element.是否电话拜访 = Convert.ToBoolean(dt.Rows[i]["是否电话拜访"]);
                    element.性别 = (性别)Enum.ToObject(typeof(性别), Convert.ToInt32(dt.Rows[i]["性别"]));
                    element.意向 = dt.Rows[i]["意向"].ToString();
                    element.住址 = dt.Rows[i]["住址"].ToString();
                    element.备注 = dt.Rows[i]["备注"].ToString();
                    elements.Add(element);
                }
            }
            return elements;
        }

        public int AddWorklog(Worklog element)
        {
            string sql = "insert into TF_Worklog (销售, 日期, 客户, 电话, 是否自访, 是否老会员, 是否电话拜访, 性别, 意向, 住址, 备注) values (" + element.销售.ID + ", '" + element.日期 + "', '" + element.客户 + "', '" + element.电话 + "', " + (element.是否自访 ? "1" : "0") + ", " + (element.是否老会员 ? "1" : "0") + ", " + (element.是否电话拜访 ? "1" : "0") + ", " + (int)element.性别 + ", '" + element.意向 + "', '" + element.住址 + "', '" + element.备注 + "'); select SCOPE_IDENTITY()";
            object obj = sqlHelper.ExecuteSqlReturn(sql);
            int R;
            if (obj != null && obj != DBNull.Value && int.TryParse(obj.ToString(), out R))
                return R;
            else
                return 0;
        }

        public bool UpdateWorklog(Worklog element)
        {
            string sql = "update TF_Worklog set 销售=" + element.销售.ID + ", 日期='" + element.日期 + "', 客户='" + element.客户 + "', 电话='" + element.电话 + "', 是否自访=" + (element.是否自访 ? "1" : "0") + ", 是否老会员=" + (element.是否老会员 ? "1" : "0") + ", 是否电话拜访=" + (element.是否电话拜访 ? "1" : "0") + ", 性别=" + (int)element.性别 + ", 意向='" + element.意向 + "', 住址='" + element.住址 + "', 备注='" + element.备注 + "' where ID=" + element.ID;
            int r = sqlHelper.ExecuteSql(sql);
            return r > 0;
        }

        public bool DeleteWorklog(Worklog element)
        {
            string sql = "delete from TF_Worklog where ID=" + element.ID;
            int r = sqlHelper.ExecuteSql(sql);
            return r > 0;
        }
        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool UpgradeList(List<Worklog> list)
        {
            int errCount = 0;
            foreach (Worklog element in list)
            {
                string sqlStr = "if exists (select 1 from TF_Worklog where ID=" + element.ID + ") update TF_Worklog set 销售=" + element.销售.ID + ", 日期='" + element.日期 + "', 客户='" + element.客户 + "', 电话='" + element.电话 + "', 是否自访=" + (element.是否自访 ? "1" : "0") + ", 是否老会员=" + (element.是否老会员 ? "1" : "0") + ", 是否电话拜访=" + (element.是否电话拜访 ? "1" : "0") + ", 性别=" + (int)element.性别 + ", 意向='" + element.意向 + "', 住址='" + element.住址 + "', 备注='" + element.备注 + "' where ID=" + element.ID + " else insert into TF_Worklog (销售, 日期, 客户, 电话, 是否自访, 是否老会员, 是否电话拜访, 性别, 意向, 住址, 备注) values (" + element.销售.ID + ", '" + element.日期 + "', '" + element.客户 + "', '" + element.电话 + "', " + (element.是否自访 ? "1" : "0") + ", " + (element.是否老会员 ? "1" : "0") + ", " + (element.是否电话拜访 ? "1" : "0") + ", " + (int)element.性别 + ", '" + element.意向 + "', '" + element.住址 + "', '" + element.备注 + "')";
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
                return sqlHelper.Exists("select 1 from TF_Worklog " + w);
            }
            return false;
        }

        public DataTable GetWorklogs(string where)
        {
            DataTable dt = null;
            string w = "";
            if (!string.IsNullOrEmpty(where))
            {
                w = where.Trim().ToLower();
                if (!w.StartsWith("where "))
                    w = "where " + w;
            }
            string sql = "select * from TF_View_Worklog " + w + "order by 销售 asc, 日期 desc";
            dt = sqlHelper.Query(sql);
            return dt;
        }
    }
}
