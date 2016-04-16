using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace TopFashion
{
    public class InventoryLogic
    {        
        SQLDBHelper sqlHelper;
        static InventoryLogic instance;
        public static InventoryLogic GetInstance()
        {
            if (instance == null)
                instance = new InventoryLogic();

            return instance;
        }

        private InventoryLogic()
        {
            sqlHelper = new SQLDBHelper();
        }

        public Inventory GetInventory(int id)
        {
            string sql = "select * from TF_Inventory where ID=" + id;
            DataTable dt = sqlHelper.Query(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                Inventory element = new Inventory();
                element.ID = id;
                element.PID = Convert.ToInt32(dt.Rows[0]["PID"]);
                element.IsProduct = Convert.ToBoolean(dt.Rows[0]["IsProduct"]);
                element.IsIncome = Convert.ToBoolean(dt.Rows[0]["IsIncome"]);
                element.数量 = Convert.ToDecimal(dt.Rows[0]["数量"]);
                element.备注 = dt.Rows[0]["备注"].ToString();
                return element;
            }
            return null;
        }

        public List<Inventory> GetAllInventorys()
        {
            List<Inventory> elements = new List<Inventory>();
            string sql = "select * from TF_Inventory";
            DataTable dt = sqlHelper.Query(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Inventory element = new Inventory();
                    element.ID = Convert.ToInt32(dt.Rows[i]["ID"]);
                    element.PID = Convert.ToInt32(dt.Rows[i]["PID"]);
                    element.IsProduct = Convert.ToBoolean(dt.Rows[i]["IsProduct"]);
                    element.IsIncome = Convert.ToBoolean(dt.Rows[i]["IsIncome"]);
                    element.数量 = Convert.ToDecimal(dt.Rows[i]["数量"]);
                    element.备注 = dt.Rows[i]["备注"].ToString();
                    elements.Add(element);
                }
            }
            return elements;
        }

        //public int AddInventory(Inventory element)
        //{
        //    string sql = "insert into TF_Inventory (PID, IsProduct, IsIncome, 数量, 备注) values (" + element.PID + ", IsProduct=" + (element.IsProduct ? "1" : "0") + ", IsIncome=" + (element.IsIncome ? "1" : "0") + ", " + element.数量 + ", '" + element.备注 + "'); select SCOPE_IDENTITY()";
        //    object obj = sqlHelper.ExecuteSqlReturn(sql);
        //    int R;
        //    if (obj != null && obj != DBNull.Value && int.TryParse(obj.ToString(), out R))
        //        return R;
        //    else
        //        return 0;
        //}

        //public bool UpdateInventory(Inventory element)
        //{
        //    string sql = "update TF_Inventory set PID=" + element.PID + ", IsProduct=" + (element.IsProduct ? "1" : "0") + ", IsIncome=" + (element.IsIncome ? "1" : "0") + ", 数量=" + element.数量 + ", 备注='" + element.备注 + "', 更新时间=getdate() where ID=" + element.ID;
        //    int r = sqlHelper.ExecuteSql(sql);
        //    return r > 0;
        //}

        public bool SaveInventory(int pid, bool isProduct, bool isIncome, decimal num)
        {
            string Num = "数量+" + num;
            if (!isIncome)
                Num = "数量-" + num;
            string sql = "if exists (select 1 from TF_Inventory where PID=" + pid + " and IsProduct=" + (isProduct ? "1" : "0") + ") update TF_Inventory set IsIncome=" + (isIncome ? "1" : "0") + ", 数量=" + Num + ", 更新时间=getdate() where PID=" + pid + " and IsProduct=" + (isProduct ? "1" : "0") + " else insert into TF_Inventory (PID, IsProduct, IsIncome, 数量) values (" + pid + ", " + (isProduct ? "1" : "0") + ", " + (isIncome ? "1" : "0") + ", " + num + ")";
            int r = sqlHelper.ExecuteSql(sql);
            return r > 0;
        }

        //public bool DeleteInventory(Inventory element)
        //{
        //    string sql = "delete from TF_Inventory where ID=" + element.ID;
        //    int r = sqlHelper.ExecuteSql(sql);
        //    return r > 0;
        //}
        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool UpgradeList(List<Inventory> list)
        {
            int errCount = 0;
            foreach (Inventory element in list)
            {
                string sqlStr = "if exists (select 1 from TF_Inventory where ID=" + element.ID + ") update TF_Inventory set PID=" + element.PID + ", IsProduct=" + (element.IsProduct ? "1" : "0") + ", IsIncome=" + (element.IsIncome ? "1" : "0") + ", 数量=" + element.数量 + ", 备注='" + element.备注 + "', 更新时间=getdate() where ID=" + element.ID + " else insert into TF_Inventory (PID, IsProduct, IsIncome, 数量, 备注) values (" + element.PID + ", IsProduct=" + (element.IsProduct ? "1" : "0") + ", IsIncome=" + (element.IsIncome ? "1" : "0") + ", " + element.数量 + ", '" + element.备注 + "')";
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
        /// 查询产品库存视图
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public DataTable GetInventoryView_Product(string where)
        {
            string w = "";
            if (!string.IsNullOrEmpty(where))
            {
                w = where.Trim().ToLower();
                if (!w.StartsWith("where "))
                    w = "where " + w;
            }
            return sqlHelper.Query("select * from TF_View_ProductInventory " + w + " order by 更新时间 desc");
        }

        /// <summary>
        /// 查询资产库存视图
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public DataTable GetInventoryView_Property(string where)
        {
            string w = "";
            if (!string.IsNullOrEmpty(where))
            {
                w = where.Trim().ToLower();
                if (!w.StartsWith("where "))
                    w = "where " + w;
            }
            return sqlHelper.Query("select * from TF_View_PropertyInventory " + w + " order by 更新时间 desc");
        }
    }
}
