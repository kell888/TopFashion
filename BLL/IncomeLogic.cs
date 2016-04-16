using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace TopFashion
{
    public class IncomeLogic
    {        
        SQLDBHelper sqlHelper;
        static IncomeLogic instance;
        public static IncomeLogic GetInstance()
        {
            if (instance == null)
                instance = new IncomeLogic();

            return instance;
        }

        private IncomeLogic()
        {
            sqlHelper = new SQLDBHelper();
        }

        public Income GetIncome(int id)
        {
            string sql = "select * from TF_Income where ID=" + id;
            DataTable dt = sqlHelper.Query(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                Income element = new Income();
                element.ID = id;
                element.PID = Convert.ToInt32(dt.Rows[0]["PID"]);
                element.IsProduct = Convert.ToBoolean(dt.Rows[0]["IsProduct"]);
                element.IsIncome = Convert.ToBoolean(dt.Rows[0]["IsIncome"]);
                element.数量 = Convert.ToDecimal(dt.Rows[0]["数量"]);
                element.时间 = Convert.ToDateTime(dt.Rows[0]["时间"]);
                element.经手人 = dt.Rows[0]["经手人"].ToString();
                element.备注 = dt.Rows[0]["备注"].ToString();
                return element;
            }
            return null;
        }

        public List<Income> GetAllIncomes()
        {
            List<Income> elements = new List<Income>();
            string sql = "select * from TF_Income";
            DataTable dt = sqlHelper.Query(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Income element = new Income();
                    element.ID = Convert.ToInt32(dt.Rows[i]["ID"]);
                    element.PID = Convert.ToInt32(dt.Rows[i]["PID"]);
                    element.IsProduct = Convert.ToBoolean(dt.Rows[i]["IsProduct"]);
                    element.IsIncome = Convert.ToBoolean(dt.Rows[i]["IsIncome"]);
                    element.数量 = Convert.ToDecimal(dt.Rows[i]["数量"]);
                    element.时间 = Convert.ToDateTime(dt.Rows[i]["时间"]);
                    element.经手人 = dt.Rows[i]["经手人"].ToString();
                    element.备注 = dt.Rows[i]["备注"].ToString();
                    elements.Add(element);
                }
            }
            return elements;
        }

        public int AddIncome(Income element)
        {
            string sql = "insert into TF_Income (PID, IsProduct, IsIncome, 数量, 实价, 备注, 经手人) values (" + element.PID + ", " + (element.IsProduct ? "1" : "0") + ", " + (element.IsIncome ? "1" : "0") + "," + element.数量 + ", " + element.实价 + ", '" + element.备注 + "', '" + element.经手人 + "'); select SCOPE_IDENTITY()";
            object obj = sqlHelper.ExecuteSqlReturn(sql);
            int R;
            if (obj != null && obj != DBNull.Value && int.TryParse(obj.ToString(), out R))
            {
                InventoryLogic.GetInstance().SaveInventory(element.PID, element.IsProduct, element.IsIncome, element.数量);
                return R;
            }
            else
            {
                return 0;
            }
        }

        public bool UpdateIncome(Income element)
        {
            string sql = "update TF_Income set PID=" + element.PID + ", IsProduct=" + (element.IsProduct ? "1" : "0") + ", IsIncome=" + (element.IsIncome ? "1" : "0") + ", 数量=" + element.数量 + ", 实价=" + element.实价 + ", 备注='" + element.备注 + "', 经手人='" + element.经手人 + "', 时间=getdate() where ID=" + element.ID;
            int r = sqlHelper.ExecuteSql(sql);
            if (r > 0)
            {
                InventoryLogic.GetInstance().SaveInventory(element.PID, element.IsProduct, element.IsIncome, element.数量);
                return true;
            }
            return false;
        }

        //public bool DeleteIncome(Income element)
        //{
        //    string sql = "delete from TF_Income where ID=" + element.ID;
        //    int r = sqlHelper.ExecuteSql(sql);
        //    return r > 0;
        //}
        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool UpgradeList(List<Income> list)
        {
            int errCount = 0;
            foreach (Income element in list)
            {
                string sqlStr = "if exists (select 1 from TF_Income where ID=" + element.ID + ") update TF_Income set PID=" + element.PID + ", IsProduct=" + (element.IsProduct ? "1" : "0") + ", IsIncome=" + (element.IsIncome ? "1" : "0") + ", 数量=" + element.数量 + ", 实价=" + element.实价 + ", 备注='" + element.备注 + "', 经手人='" + element.经手人 + "', 时间=getdate() where ID=" + element.ID + " else insert into TF_Income (PID, IsProduct, IsIncome, 数量, 实价, 备注, 经手人) values (" + element.PID + ", " + (element.IsProduct ? "1" : "0") + ", " + (element.IsIncome ? "1" : "0") + "," + element.数量 + ", " + element.实价 + ", '" + element.备注 + "', '" + element.经手人 + "')";
                try
                {
                    sqlHelper.ExecuteSql(sqlStr);
                    InventoryLogic.GetInstance().SaveInventory(element.PID, element.IsProduct, element.IsIncome, element.数量);
                }
                catch (Exception)
                {
                    errCount++;
                }
            }
            return errCount == 0;
        }

        /// <summary>
        /// 查询产品进出视图
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public DataTable GetIncomeView_Product(string where)
        {
            string w = "";
            if (!string.IsNullOrEmpty(where))
            {
                w = where.Trim().ToLower();
                if (!w.StartsWith("where "))
                    w = "where " + w;
            }
            return sqlHelper.Query("select * from TF_View_ProductIncome " + w + " order by 时间 desc");
        }

        /// <summary>
        /// 查询资产进出视图
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public DataTable GetIncomeView_Property(string where)
        {
            string w = "";
            if (!string.IsNullOrEmpty(where))
            {
                w = where.Trim().ToLower();
                if (!w.StartsWith("where "))
                    w = "where " + w;
            }
            return sqlHelper.Query("select * from TF_View_PropertyIncome " + w + " order by 时间 desc");
        }
    }
}
