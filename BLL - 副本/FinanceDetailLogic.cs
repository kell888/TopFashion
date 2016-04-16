using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace TopFashion
{
    public class FinanceDetailLogic
    {        
        SQLDBHelper sqlHelper;
        static FinanceDetailLogic instance;
        public static FinanceDetailLogic GetInstance()
        {
            if (instance == null)
                instance = new FinanceDetailLogic();

            return instance;
        }

        private FinanceDetailLogic()
        {
            sqlHelper = new SQLDBHelper();
        }

        public FinanceDetail GetFinanceDetail(int id)
        {
            string sql = "select * from TF_FinanceDetail where ID=" + id;
            DataTable dt = sqlHelper.Query(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                FinanceDetail element = new FinanceDetail();
                element.ID = id;
                element.项目 = dt.Rows[0]["项目"].ToString();
                element.金额 = Convert.ToDecimal(dt.Rows[0]["金额"]);
                element.是否进账 = Convert.ToBoolean(dt.Rows[0]["是否进账"]);
                element.责任人 = StaffLogic.GetInstance().GetStaff(Convert.ToInt32(dt.Rows[0]["责任人"]));
                element.备注 = dt.Rows[0]["备注"].ToString();
                element.提交时间 = Convert.ToDateTime(dt.Rows[0]["提交时间"]);
                element.Flag = Convert.ToInt32(dt.Rows[0]["Flag"]);
                return element;
            }
            return null;
        }

        public List<FinanceDetail> GetAllFinanceDetails()
        {
            List<FinanceDetail> elements = new List<FinanceDetail>();
            string sql = "select * from TF_FinanceDetail";
            DataTable dt = sqlHelper.Query(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    FinanceDetail element = new FinanceDetail();
                    element.ID = Convert.ToInt32(dt.Rows[i]["ID"]);
                    element.项目 = dt.Rows[i]["项目"].ToString();
                    element.金额 = Convert.ToDecimal(dt.Rows[i]["金额"]);
                    element.是否进账 = Convert.ToBoolean(dt.Rows[i]["是否进账"]);
                    element.责任人 = StaffLogic.GetInstance().GetStaff(Convert.ToInt32(dt.Rows[i]["责任人"]));
                    element.备注 = dt.Rows[i]["备注"].ToString();
                    element.提交时间 = Convert.ToDateTime(dt.Rows[i]["提交时间"]);
                    element.Flag = Convert.ToInt32(dt.Rows[i]["Flag"]);
                    elements.Add(element);
                }
            }
            return elements;
        }

        public List<FinanceDetail> GetFinanceDetailList(string where)
        {
            List<FinanceDetail> elements = new List<FinanceDetail>();
            if (!string.IsNullOrEmpty(where))
            {
                string w = where.Trim().ToLower();
                if (!w.StartsWith("where "))
                    w = "where " + w;
                string sql = "select * from TF_FinanceDetail " + w;
                DataTable dt = sqlHelper.Query(sql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        FinanceDetail element = new FinanceDetail();
                        element.ID = Convert.ToInt32(dt.Rows[i]["ID"]);
                        element.项目 = dt.Rows[i]["项目"].ToString();
                        element.金额 = Convert.ToDecimal(dt.Rows[i]["金额"]);
                        element.是否进账 = Convert.ToBoolean(dt.Rows[i]["是否进账"]);
                        element.责任人 = StaffLogic.GetInstance().GetStaff(Convert.ToInt32(dt.Rows[i]["责任人"]));
                        element.备注 = dt.Rows[i]["备注"].ToString();
                        element.提交时间 = Convert.ToDateTime(dt.Rows[i]["提交时间"]);
                        element.Flag = Convert.ToInt32(dt.Rows[i]["Flag"]);
                        elements.Add(element);
                    }
                }
            }
            return elements;
        }

        public List<FinanceDetail> GetFinanceDetailsByIds(string ids)
        {
            List<FinanceDetail> elements = new List<FinanceDetail>();
            string Ids = "";
            if (!string.IsNullOrEmpty(ids))
            {
                Ids = " where ID in (" + ids + ")";
            }
            string sql = "select * from TF_FinanceDetail" + Ids;
            DataTable dt = sqlHelper.Query(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    FinanceDetail element = new FinanceDetail();
                    element.ID = Convert.ToInt32(dt.Rows[i]["ID"]);
                    element.项目 = dt.Rows[i]["项目"].ToString();
                    element.金额 = Convert.ToDecimal(dt.Rows[i]["金额"]);
                    element.是否进账 = Convert.ToBoolean(dt.Rows[i]["是否进账"]);
                    element.责任人 = StaffLogic.GetInstance().GetStaff(Convert.ToInt32(dt.Rows[i]["责任人"]));
                    element.备注 = dt.Rows[i]["备注"].ToString();
                    element.提交时间 = Convert.ToDateTime(dt.Rows[i]["提交时间"]);
                    element.Flag = Convert.ToInt32(dt.Rows[i]["Flag"]);
                    elements.Add(element);
                }
            }
            return elements;
        }

        public int AddFinanceDetail(FinanceDetail element)
        {
            string sql = "insert into TF_FinanceDetail (项目, 金额, 是否进账, 责任人, 备注, Flag) values ('" + element.项目 + "', " + element.金额 + ", " + (element.是否进账 ? "1" : "0") + ", " + element.责任人.ID + ", '" + element.备注 + "', " + element.Flag + "); select SCOPE_IDENTITY()";
            object obj = sqlHelper.ExecuteSqlReturn(sql);
            int R;
            if (obj != null && obj != DBNull.Value && int.TryParse(obj.ToString(), out R))
                return R;
            else
                return 0;
        }

        public bool UpdateFinanceDetail(FinanceDetail element)
        {
            string sql = "update TF_FinanceDetail set 项目='" + element.项目 + "', 金额=" + element.金额 + ", 是否进账=" + (element.是否进账 ? "1" : "0") + ", 责任人=" + element.责任人.ID + ", 备注='" + element.备注 + "', Flag=" + element.Flag + ", 提交时间=getdate() where ID=" + element.ID;
            int r = sqlHelper.ExecuteSql(sql);
            return r > 0;
        }

        public bool DeleteFinanceDetail(FinanceDetail element)
        {
            string sql = "delete from TF_FinanceDetail where ID=" + element.ID;
            int r = sqlHelper.ExecuteSql(sql);
            return r > 0;
        }
        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool UpgradeList(List<FinanceDetail> list)
        {
            int errCount = 0;
            foreach (FinanceDetail element in list)
            {
                string sqlStr = "if exists (select 1 from TF_FinanceDetail where ID=" + element.ID + ") update TF_FinanceDetail set 项目='" + element.项目 + "', 金额=" + element.金额 + ", 是否进账=" + (element.是否进账 ? "1" : "0") + ", 责任人=" + element.责任人.ID + ", 备注='" + element.备注 + "', Flag=" + element.Flag + ", 提交时间=getdate() where ID=" + element.ID + " else insert into TF_FinanceDetail (项目, 金额, 是否进账, 责任人, 备注, Flag) values ('" + element.项目 + "', " + element.金额 + ", " + (element.是否进账 ? "1" : "0") + ", " + element.责任人.ID + ", '" + element.备注 + "', " + element.Flag + ")";
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
        /// 如果返回true，则为报销成功
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Check(int id)
        {
            string sql = "update TF_FinanceDetail set Flag=1 where ID=" + id;
            int r = sqlHelper.ExecuteSql(sql);
            return r > 0;
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
                return sqlHelper.Exists("select 1 from TF_FinanceDetail " + w);
            }
            return false;
        }

        public DataTable GetFinanceDetails(string where)
        {
            DataTable dt = null;
            string w = "";
            if (!string.IsNullOrEmpty(where))
            {
                w = where.Trim().ToLower();
                if (!w.StartsWith("where "))
                    w = "where " + w;
            }
            string sql = "select * from TF_View_FinanceDetail " + w + " order by 提交时间 desc";
            dt = sqlHelper.Query(sql);
            return dt;
        }
    }
}
