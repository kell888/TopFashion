using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace TopFashion
{
    public class WorkplanLogic
    {        
        SQLDBHelper sqlHelper;
        static WorkplanLogic instance;
        public static WorkplanLogic GetInstance()
        {
            if (instance == null)
                instance = new WorkplanLogic();

            return instance;
        }

        private WorkplanLogic()
        {
            sqlHelper = new SQLDBHelper();
        }

        public Workplan GetWorkplan(int id)
        {
            string sql = "select * from TF_Workplan where ID=" + id;
            DataTable dt = sqlHelper.Query(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                Workplan element = new Workplan();
                element.ID = id;
                element.销售 = StaffLogic.GetInstance().GetStaff(Convert.ToInt32(dt.Rows[0]["销售"]));
                element.日期 = Convert.ToDateTime(dt.Rows[0]["日期"]);
                element.带人数 = Convert.ToInt32(dt.Rows[0]["带人数"]);
                element.号码数 = Convert.ToInt32(dt.Rows[0]["号码数"]);
                element.成单数 = Convert.ToInt32(dt.Rows[0]["成单数"]);
                element.回访数 = Convert.ToInt32(dt.Rows[0]["回访数"]);
                element.备注 = dt.Rows[0]["备注"].ToString();
                return element;
            }
            return null;
        }

        public List<Workplan> GetAllWorkplans()
        {
            List<Workplan> elements = new List<Workplan>();
            string sql = "select * from TF_Workplan";
            DataTable dt = sqlHelper.Query(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Workplan element = new Workplan();
                    element.ID = Convert.ToInt32(dt.Rows[i]["ID"]);
                    element.销售 = StaffLogic.GetInstance().GetStaff(Convert.ToInt32(dt.Rows[i]["销售"]));
                    element.日期 = Convert.ToDateTime(dt.Rows[i]["日期"]);
                    element.带人数 = Convert.ToInt32(dt.Rows[i]["带人数"]);
                    element.号码数 = Convert.ToInt32(dt.Rows[i]["号码数"]);
                    element.成单数 = Convert.ToInt32(dt.Rows[i]["成单数"]);
                    element.回访数 = Convert.ToInt32(dt.Rows[i]["回访数"]);
                    element.备注 = dt.Rows[i]["备注"].ToString();
                    elements.Add(element);
                }
            }
            return elements;
        }

        public int AddWorkplan(Workplan element)
        {
            string sql = "insert into TF_Workplan (销售, 日期, 带人数, 号码数, 成单数, 回访数, 备注) values (" + element.销售.ID + ", '" + element.日期 + "', " + element.带人数 + ", " + element.号码数 + ", " + element.成单数 + ", " + element.回访数 + ", '" + element.备注 + "'); select SCOPE_IDENTITY()";
            object obj = sqlHelper.ExecuteSqlReturn(sql);
            int R;
            if (obj != null && obj != DBNull.Value && int.TryParse(obj.ToString(), out R))
                return R;
            else
                return 0;
        }

        public bool UpdateWorkplan(Workplan element)
        {
            string sql = "update TF_Workplan set 销售=" + element.销售.ID + ", 日期='" + element.日期 + "', 带人数=" + element.带人数 + ", 号码数=" + element.号码数 + ", 成单数=" + element.成单数 + ", 回访数=" + element.回访数 + ", 备注='" + element.备注 + "' where ID=" + element.ID;
            int r = sqlHelper.ExecuteSql(sql);
            return r > 0;
        }

        public bool DeleteWorkplan(Workplan element)
        {
            string sql = "delete from TF_Workplan where ID=" + element.ID;
            int r = sqlHelper.ExecuteSql(sql);
            return r > 0;
        }
        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool UpgradeList(List<Workplan> list)
        {
            int errCount = 0;
            foreach (Workplan element in list)
            {
                string sqlStr = "if exists (select 1 from TF_Workplan where ID=" + element.ID + ") update TF_Workplan set 销售=" + element.销售.ID + ", 日期='" + element.日期 + "', 带人数=" + element.带人数 + ", 号码数=" + element.号码数 + ", 成单数=" + element.成单数 + ", 回访数=" + element.回访数 + ", 备注='" + element.备注 + "' where ID=" + element.ID + " else insert into TF_Workplan (销售, 日期, 带人数, 号码数, 成单数, 回访数, 备注) values (" + element.销售.ID + ", '" + element.日期 + "', " + element.带人数 + ", " + element.号码数 + ", " + element.成单数 + ", " + element.回访数 + ", '" + element.备注 + "')";
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
                return sqlHelper.Exists("select 1 from TF_Workplan " + w);
            }
            return false;
        }

        public DataTable GetWorkplans(string where)
        {
            DataTable dt = null;
            string w = "";
            if (!string.IsNullOrEmpty(where))
            {
                w = where.Trim().ToLower();
                if (!w.StartsWith("where "))
                    w = "where " + w;
            }
            string sql = "select * from TF_View_Workplan " + w + "order by 销售 asc, 日期 desc";
            dt = sqlHelper.Query(sql);
            return dt;
        }
    }
}
