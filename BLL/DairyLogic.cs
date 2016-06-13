using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace TopFashion
{
    public class DairyLogic
    {        
        SQLDBHelper sqlHelper;
        static DairyLogic instance;
        public static DairyLogic GetInstance()
        {
            if (instance == null)
                instance = new DairyLogic();

            return instance;
        }

        private DairyLogic()
        {
            sqlHelper = new SQLDBHelper();
        }

        public Dairy GetDairy(int id)
        {
            string sql = "select * from TF_Dairy where ID=" + id;
            DataTable dt = sqlHelper.Query(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                Dairy element = new Dairy();
                element.ID = id;
                element.Pos机会籍 = Convert.ToDecimal(dt.Rows[0]["Pos机会籍"]);
                element.Pos机私教 = Convert.ToDecimal(dt.Rows[0]["Pos机私教"]);
                element.现金会籍 = Convert.ToDecimal(dt.Rows[0]["现金会籍"]);
                element.现金私教 = Convert.ToDecimal(dt.Rows[0]["现金私教"]);
                element.微信会籍 = Convert.ToDecimal(dt.Rows[0]["微信会籍"]);
                element.微信私教 = Convert.ToDecimal(dt.Rows[0]["微信私教"]);
                element.现金存水 = Convert.ToDecimal(dt.Rows[0]["现金存水"]);
                element.微信存水 = Convert.ToDecimal(dt.Rows[0]["微信存水"]);
                element.水吧余 = Convert.ToDecimal(dt.Rows[0]["水吧余"]);
                element.总金额 = Convert.ToDecimal(dt.Rows[0]["总金额"]);
                element.备注 = dt.Rows[0]["备注"].ToString();
                element.经手人 = StaffLogic.GetInstance().GetStaff(Convert.ToInt32(dt.Rows[0]["经手人"]));
                element.日期 = Convert.ToDateTime(dt.Rows[0]["日期"]);
                return element;
            }
            return null;
        }

        public List<Dairy> GetAllDairys()
        {
            List<Dairy> elements = new List<Dairy>();
            string sql = "select * from TF_Dairy";
            DataTable dt = sqlHelper.Query(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Dairy element = new Dairy();
                    element.ID = Convert.ToInt32(dt.Rows[i]["ID"]);
                    element.Pos机会籍 = Convert.ToDecimal(dt.Rows[i]["Pos机会籍"]);
                    element.Pos机私教 = Convert.ToDecimal(dt.Rows[i]["Pos机私教"]);
                    element.现金会籍 = Convert.ToDecimal(dt.Rows[i]["现金会籍"]);
                    element.现金私教 = Convert.ToDecimal(dt.Rows[i]["现金私教"]);
                    element.微信会籍 = Convert.ToDecimal(dt.Rows[i]["微信会籍"]);
                    element.微信私教 = Convert.ToDecimal(dt.Rows[i]["微信私教"]);
                    element.现金存水 = Convert.ToDecimal(dt.Rows[i]["现金存水"]);
                    element.微信存水 = Convert.ToDecimal(dt.Rows[i]["微信存水"]);
                    element.水吧余 = Convert.ToDecimal(dt.Rows[i]["水吧余"]);
                    element.总金额 = Convert.ToDecimal(dt.Rows[i]["总金额"]);
                    element.备注 = dt.Rows[i]["备注"].ToString();
                    element.经手人 = StaffLogic.GetInstance().GetStaff(Convert.ToInt32(dt.Rows[i]["经手人"]));
                    element.日期 = Convert.ToDateTime(dt.Rows[i]["日期"]);
                    elements.Add(element);
                }
            }
            return elements;
        }

        public List<Dairy> GetDairysByStaff(Staff staff)
        {
            List<Dairy> elements = new List<Dairy>();
            string sql = "select * from TF_Dairy where 经手人=" + staff.ID;
            DataTable dt = sqlHelper.Query(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Dairy element = new Dairy();
                    element.ID = Convert.ToInt32(dt.Rows[i]["ID"]);
                    element.Pos机会籍 = Convert.ToDecimal(dt.Rows[i]["Pos机会籍"]);
                    element.Pos机私教 = Convert.ToDecimal(dt.Rows[i]["Pos机私教"]);
                    element.现金会籍 = Convert.ToDecimal(dt.Rows[i]["现金会籍"]);
                    element.现金私教 = Convert.ToDecimal(dt.Rows[i]["现金私教"]);
                    element.微信会籍 = Convert.ToDecimal(dt.Rows[i]["微信会籍"]);
                    element.微信私教 = Convert.ToDecimal(dt.Rows[i]["微信私教"]);
                    element.现金存水 = Convert.ToDecimal(dt.Rows[i]["现金存水"]);
                    element.微信存水 = Convert.ToDecimal(dt.Rows[i]["微信存水"]);
                    element.水吧余 = Convert.ToDecimal(dt.Rows[i]["水吧余"]);
                    element.总金额 = Convert.ToDecimal(dt.Rows[i]["总金额"]);
                    element.备注 = dt.Rows[i]["备注"].ToString();
                    element.经手人 = staff;
                    element.日期 = Convert.ToDateTime(dt.Rows[i]["日期"]);
                    elements.Add(element);
                }
            }
            return elements;
        }

        public int AddDairy(Dairy element)
        {
            string sql = "insert into TF_Dairy (Pos机会籍, Pos机私教, 现金会籍, 现金私教, 微信会籍, 微信私教, 现金存水, 微信存水, 水吧余, 总金额, 备注, 经手人, 日期) values (" + element.Pos机会籍 + ", " + element.Pos机私教 + ", " + element.现金会籍 + ", " + element.现金私教 + ", " + element.微信会籍 + ", " + element.微信私教 + ", " + element.现金存水 + ", " + element.微信存水 + ", " + element.水吧余 + ", " + element.总金额 + ", '" + element.备注 + "', " + element.经手人.ID + ", '" + element.日期 + "'); select SCOPE_IDENTITY()";
            object obj = sqlHelper.ExecuteSqlReturn(sql);
            int R;
            if (obj != null && obj != DBNull.Value && int.TryParse(obj.ToString(), out R))
                return R;
            else
                return 0;
        }

        public bool UpdateDairy(Dairy element)
        {
            string sql = "update TF_Dairy set Pos机会籍=" + element.Pos机会籍 + ", Pos机私教=" + element.Pos机私教 + ", 现金会籍=" + element.现金会籍 + ", 现金私教=" + element.现金私教 + ", 微信会籍=" + element.微信会籍 + ", 微信私教=" + element.微信私教 + ", 现金存水=" + element.现金存水 + ", 微信存水=" + element.微信存水 + ", 水吧余=" + element.水吧余 + ", 总金额=" + element.总金额 + ", 备注='" + element.备注 + "', 经手人=" + element.经手人.ID + ",日期='" + element.日期 + "' where ID=" + element.ID;
            int r = sqlHelper.ExecuteSql(sql);
            return r > 0;
        }

        public bool DeleteDairy(Dairy element)
        {
            string sql = "delete from TF_Dairy where ID=" + element.ID;
            int r = sqlHelper.ExecuteSql(sql);
            return r > 0;
        }
        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool UpgradeList(List<Dairy> list)
        {
            int errCount = 0;
            foreach (Dairy element in list)
            {
                string sqlStr = "if exists (select 1 from TF_Dairy where ID=" + element.ID + ") update TF_Dairy set Pos机会籍=" + element.Pos机会籍 + ", Pos机私教=" + element.Pos机私教 + ", 现金会籍=" + element.现金会籍 + ", 现金私教=" + element.现金私教 + ", 微信会籍=" + element.微信会籍 + ", 微信私教=" + element.微信私教 + ", 现金存水=" + element.现金存水 + ", 微信存水=" + element.微信存水 + ", 水吧余=" + element.水吧余 + ", 总金额=" + element.总金额 + ", 备注='" + element.备注 + "', 经手人=" + element.经手人.ID + ",日期='" + element.日期 + "' where ID=" + element.ID + " else insert into TF_Dairy (Pos机会籍, Pos机私教, 现金会籍, 现金私教, 微信会籍, 微信私教, 现金存水, 微信存水, 水吧余, 总金额, 备注, 经手人, 日期) values (" + element.Pos机会籍 + ", " + element.Pos机私教 + ", " + element.现金会籍 + ", " + element.现金私教 + ", " + element.微信会籍 + ", " + element.微信私教 + ", " + element.现金存水 + ", " + element.微信存水 + ", " + element.水吧余 + ", " + element.总金额 + ", '" + element.备注 + "', " + element.经手人.ID + ", '" + element.日期 + "')";
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
                return sqlHelper.Exists("select 1 from TF_Dairy " + w);
            }
            return false;
        }

        public DataTable GetDairys(string where)
        {
            DataTable dt = null;
            string w = "";
            if (!string.IsNullOrEmpty(where))
            {
                w = where.Trim().ToLower();
                if (!w.StartsWith("where "))
                    w = "where " + w;
            }
            string sql = "select * from TF_View_Dairy " + w + " order by 日期 desc";
            dt = sqlHelper.Query(sql);
            return dt;
        }
    }
}
