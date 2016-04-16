using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace TopFashion
{
    public class FinanceLogic
    {        
        SQLDBHelper sqlHelper;
        static FinanceLogic instance;
        public static FinanceLogic GetInstance()
        {
            if (instance == null)
                instance = new FinanceLogic();

            return instance;
        }

        private FinanceLogic()
        {
            sqlHelper = new SQLDBHelper();
        }

        public Finance GetFinance(int id)
        {
            string sql = "select * from TF_Finance where ID=" + id;
            DataTable dt = sqlHelper.Query(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                Finance element = new Finance();
                element.ID = id;
                element.项目 = dt.Rows[0]["项目"].ToString();
                element.金额 = Convert.ToDecimal(dt.Rows[0]["金额"]);
                element.是否进账 = Convert.ToBoolean(dt.Rows[0]["是否进账"]);
                element.余款 = Convert.ToDecimal(dt.Rows[0]["余款"]);
                element.日期 = Convert.ToDateTime(dt.Rows[0]["日期"]);
                element.经手人 = dt.Rows[0]["经手人"].ToString();
                element.接收人 = dt.Rows[0]["接收人"].ToString();
                element.Detail = dt.Rows[0]["Detail"].ToString();
                return element;
            }
            return null;
        }

        public List<Finance> GetAllFinances()
        {
            List<Finance> elements = new List<Finance>();
            string sql = "select * from TF_Finance";
            DataTable dt = sqlHelper.Query(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Finance element = new Finance();
                    element.ID = Convert.ToInt32(dt.Rows[i]["ID"]);
                    element.项目 = dt.Rows[i]["项目"].ToString();
                    element.金额 = Convert.ToDecimal(dt.Rows[i]["金额"]);
                    element.是否进账 = Convert.ToBoolean(dt.Rows[i]["是否进账"]);
                    element.余款 = Convert.ToDecimal(dt.Rows[i]["余款"]);
                    element.日期 = Convert.ToDateTime(dt.Rows[i]["日期"]);
                    element.经手人 = dt.Rows[i]["经手人"].ToString();
                    element.接收人 = dt.Rows[i]["接收人"].ToString();
                    element.Detail = dt.Rows[i]["Detail"].ToString();
                    elements.Add(element);
                }
            }
            return elements;
        }

        public int AddFinance(Finance element)
        {
            string sql = "insert into TF_Finance (项目, 金额, 是否进账, 余款, 日期, 经手人, 接收人, Detail) values ('" + element.项目 + "', " + element.金额 + ", " + (element.是否进账 ? "1" : "0") + ", " + element.余款 + ", '" + element.日期 + "', '" + element.经手人 + "', '" + element.接收人 + "', '" + element.Detail + "'); select SCOPE_IDENTITY()";
            object obj = sqlHelper.ExecuteSqlReturn(sql);
            int R;
            if (obj != null && obj != DBNull.Value && int.TryParse(obj.ToString(), out R))
                return R;
            else
                return 0;
        }

        public bool UpdateFinance(Finance element)
        {
            string sql = "update TF_Finance set 项目='" + element.项目 + "', 金额=" + element.金额 + ", 是否进账=" + (element.是否进账 ? "1" : "0") + ", 余款=" + element.余款 + ", 日期='" + element.日期 + "', 经手人='" + element.经手人 + "', 接收人='" + element.接收人 + "', Detail='" + element.Detail + "' where ID=" + element.ID;
            int r = sqlHelper.ExecuteSql(sql);
            return r > 0;
        }
        /// <summary>
        /// 在原有的明细基础上添加新的明细
        /// </summary>
        /// <param name="element"></param>
        /// <param name="details"></param>
        /// <returns></returns>
        public bool AddDetail(Finance element, List<FinanceDetail> details)
        {
            if (details != null && details.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (FinanceDetail fd in details)
                {
                    if (sb.Length == 0)
                        sb.Append(fd.ID);
                    else
                        sb.Append("," + fd.ID);
                }
                string sql = "update TF_Finance set Detail='" + sb.ToString() + "' where ID=" + element.ID;
                int r = sqlHelper.ExecuteSql(sql);
                return r > 0;
            }
            return false;
        }

        public bool DeleteFinance(Finance element)
        {
            string sql = "delete from TF_Finance where ID=" + element.ID;
            int r = sqlHelper.ExecuteSql(sql);
            return r > 0;
        }
        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool UpgradeList(List<Finance> list)
        {
            int errCount = 0;
            foreach (Finance element in list)
            {
                string sqlStr = "if exists (select 1 from TF_Finance where ID=" + element.ID + ") update TF_Finance set 项目='" + element.项目 + "', 金额=" + element.金额 + ", 是否进账=" + (element.是否进账 ? "1" : "0") + ", 余款=" + element.余款 + ", 日期='" + element.日期 + "', 经手人='" + element.经手人 + "', 接收人='" + element.接收人 + "' where ID=" + element.ID + " else insert into TF_Finance (项目, 金额, 是否进账, 余款, 日期, 经手人, 接收人) values ('" + element.项目 + "', " + element.金额 + ", " + (element.是否进账 ? "1" : "0") + ", " + element.余款 + ", '" + element.日期 + "', '" + element.经手人 + "', '" + element.接收人 + "')";
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
                return sqlHelper.Exists("select 1 from TF_Finance " + w);
            }
            return false;
        }

        public DataTable GetFinances(string where)
        {
            DataTable dt = null;
            string w = "";
            if (!string.IsNullOrEmpty(where))
            {
                w = where.Trim().ToLower();
                if (!w.StartsWith("where "))
                    w = "where " + w;
            }
            string sql = "select * from TF_View_Finance " + w + " order by 日期 desc";
            dt = sqlHelper.Query(sql);
            return dt;
        }
    }
}
