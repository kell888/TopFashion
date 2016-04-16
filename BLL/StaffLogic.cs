using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace TopFashion
{
    public class StaffLogic
    {        
        SQLDBHelper sqlHelper;
        static StaffLogic instance;
        public static StaffLogic GetInstance()
        {
            if (instance == null)
                instance = new StaffLogic();

            return instance;
        }

        private StaffLogic()
        {
            sqlHelper = new SQLDBHelper();
        }

        public Staff GetStaff(int id)
        {
            string sql = "select * from TF_Staff where ID=" + id;
            DataTable dt = sqlHelper.Query(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                Staff element = new Staff();
                element.ID = id;
                element.Depart = DepartmentLogic.GetInstance().GetDepartment(Convert.ToInt32(dt.Rows[0]["Depart"]));
                element.Condition = StaffConditionLogic.GetInstance().GetStaffCondition(Convert.ToInt32(dt.Rows[0]["Condition"]));
                element.性别 = (性别)Enum.ToObject(typeof(性别), Convert.ToInt32(dt.Rows[0]["性别"]));
                element.姓名 = dt.Rows[0]["姓名"].ToString();
                element.电话 = dt.Rows[0]["电话"].ToString();
                element.生日 = Convert.ToDateTime(dt.Rows[0]["生日"]);
                element.宿舍 = dt.Rows[0]["宿舍"].ToString();
                element.备注 = dt.Rows[0]["备注"].ToString();
                element.钥匙数 = Convert.ToInt32(dt.Rows[0]["钥匙数"]);
                element.工衣数 = Convert.ToInt32(dt.Rows[0]["工衣数"]);
                element.工牌数 = Convert.ToInt32(dt.Rows[0]["工牌数"]);
                element.是否全部回收 = Convert.ToBoolean(dt.Rows[0]["是否全部回收"]);
                return element;
            }
            return null;
        }

        public Staff GetStaffByName(string name)
        {
            string sql = "select * from TF_Staff where 姓名 like '%" + name + "%'";
            DataTable dt = sqlHelper.Query(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                Staff element = new Staff();
                element.ID = Convert.ToInt32(dt.Rows[0]["ID"]);
                element.Depart = DepartmentLogic.GetInstance().GetDepartment(Convert.ToInt32(dt.Rows[0]["Depart"]));
                element.Condition = StaffConditionLogic.GetInstance().GetStaffCondition(Convert.ToInt32(dt.Rows[0]["Condition"]));
                element.性别 = (性别)Enum.ToObject(typeof(性别), Convert.ToInt32(dt.Rows[0]["性别"]));
                element.姓名 = dt.Rows[0]["姓名"].ToString();
                element.电话 = dt.Rows[0]["电话"].ToString();
                element.生日 = Convert.ToDateTime(dt.Rows[0]["生日"]);
                element.宿舍 = dt.Rows[0]["宿舍"].ToString();
                element.备注 = dt.Rows[0]["备注"].ToString();
                element.钥匙数 = Convert.ToInt32(dt.Rows[0]["钥匙数"]);
                element.工衣数 = Convert.ToInt32(dt.Rows[0]["工衣数"]);
                element.工牌数 = Convert.ToInt32(dt.Rows[0]["工牌数"]);
                element.是否全部回收 = Convert.ToBoolean(dt.Rows[0]["是否全部回收"]);
                return element;
            }
            return null;
        }

        public List<Staff> GetAllStaffs()
        {
            List<Staff> elements = new List<Staff>();
            string sql = "select * from TF_Staff";
            DataTable dt = sqlHelper.Query(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Staff element = new Staff();
                    element.ID = Convert.ToInt32(dt.Rows[i]["ID"]);
                    element.Depart = DepartmentLogic.GetInstance().GetDepartment(Convert.ToInt32(dt.Rows[i]["Depart"]));
                    element.Condition = StaffConditionLogic.GetInstance().GetStaffCondition(Convert.ToInt32(dt.Rows[i]["Condition"]));
                    element.性别 = (性别)Enum.ToObject(typeof(性别), Convert.ToInt32(dt.Rows[i]["性别"]));
                    element.姓名 = dt.Rows[i]["姓名"].ToString();
                    element.电话 = dt.Rows[i]["电话"].ToString();
                    element.生日 = Convert.ToDateTime(dt.Rows[i]["生日"]);
                    element.宿舍 = dt.Rows[i]["宿舍"].ToString();
                    element.备注 = dt.Rows[i]["备注"].ToString();
                    element.钥匙数 = Convert.ToInt32(dt.Rows[i]["钥匙数"]);
                    element.工衣数 = Convert.ToInt32(dt.Rows[i]["工衣数"]);
                    element.工牌数 = Convert.ToInt32(dt.Rows[i]["工牌数"]);
                    element.是否全部回收 = Convert.ToBoolean(dt.Rows[i]["是否全部回收"]);
                    elements.Add(element);
                }
            }
            return elements;
        }

        public int AddStaff(Staff element)
        {
            string sql = "insert into TF_Staff (Depart, Condition, 姓名, 性别, 生日, 电话, 宿舍, 钥匙数, 工衣数, 工牌数, 是否全部回收, 备注) values (" + element.Depart.ID + ", " + element.Condition.ID + ", '" + element.姓名 + "', " + (int)element.性别 + ", '" + element.生日.ToString("yyyy-MM-dd") + "', '" + element.电话 + "', '" + element.宿舍 + "', " + element.钥匙数 + ", " + element.工衣数 + ", " + element.工牌数 + ", " + (element.是否全部回收 ? "1" : "0") + ", '" + element.备注 + "'); select SCOPE_IDENTITY()";
            object obj = sqlHelper.ExecuteSqlReturn(sql);
            int R;
            if (obj != null && obj != DBNull.Value && int.TryParse(obj.ToString(), out R))
                return R;
            else
                return 0;
        }

        public bool UpdateStaff(Staff element)
        {
            string sql = "update TF_Staff set Depart=" + element.Depart.ID + ", Condition=" + element.Condition.ID + ", 姓名='" + element.姓名 + "', 性别=" + (int)element.性别 + ", 生日='" + element.生日 + "', 电话='" + element.电话 + "', 宿舍='" + element.宿舍 + "', 钥匙数=" + element.钥匙数 + ", 工衣数=" + element.工衣数 + ", 工牌数=" + element.工牌数 + ", 是否全部回收=" + (element.是否全部回收 ? "1" : "0") + ", 备注='" + element.备注 + "' where ID=" + element.ID;
            int r = sqlHelper.ExecuteSql(sql);
            return r > 0;
        }

        public bool DeleteStaff(Staff element)
        {
            string sql = "delete from TF_Staff where ID=" + element.ID;
            int r = sqlHelper.ExecuteSql(sql);
            return r > 0;
        }
        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool UpgradeList(List<Staff> list)
        {
            int errCount = 0;
            foreach (Staff element in list)
            {
                string sqlStr = "if exists (select 1 from TF_Staff where ID=" + element.ID + ") update TF_Staff set Depart=" + element.Depart.ID + ", Condition=" + element.Condition.ID + ", 姓名='" + element.姓名 + "', 性别=" + (int)element.性别 + ", 生日='" + element.生日 + "', 电话='" + element.电话 + "', 宿舍='" + element.宿舍 + "', 钥匙数=" + element.钥匙数 + ", 工衣数=" + element.工衣数 + ", 工牌数=" + element.工牌数 + ", 是否全部回收=" + (element.是否全部回收 ? "1" : "0") + ", 备注='" + element.备注 + "' where ID=" + element.ID + " else insert into TF_Staff (Depart, Condition, 姓名, 性别, 生日, 电话, 宿舍, 钥匙数, 工衣数, 工牌数, 是否全部回收, 备注) values (" + element.Depart.ID + ", " + element.Condition.ID + ", '" + element.姓名 + "', " + (int)element.性别 + ", '" + element.生日.ToString("yyyy-MM-dd") + "', '" + element.电话 + "', '" + element.宿舍 + "', " + element.钥匙数 + ", " + element.工衣数 + ", " + element.工牌数 + ", " + (element.是否全部回收 ? "1" : "0") + ", '" + element.备注 + "')";
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
            return sqlHelper.Exists("select 1 from TF_Staff where 姓名='" + name + "'");
        }

        /// <summary>
        /// 是否存在出了自己以外的同名
        /// </summary>
        /// <param name="name"></param>
        /// <param name="myId"></param>
        /// <returns></returns>
        public bool ExistsNameOther(string name, int myId)
        {
            return sqlHelper.Exists("select 1 from TF_Staff where ID!=" + myId + " and 姓名='" + name + "'");
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
                return sqlHelper.Exists("select 1 from TF_Staff " + w);
            }
            return false;
        }

        public DataTable GetStaffData(string where = null)
        {
            DataTable dt = null;
            string w = "";
            if (!string.IsNullOrEmpty(where))
            {
                w = where.Trim().ToLower();
                if (!w.StartsWith("where "))
                    w = "where " + w;
            }
            string sql = "select * from TF_Staff " + w + " order by ID asc";
            dt = sqlHelper.Query(sql);
            return dt;
        }

        public DataTable GetStaffs(string where)
        {
            DataTable dt = null;
            string w = "";
            if (!string.IsNullOrEmpty(where))
            {
                w = where.Trim().ToLower();
                if (!w.StartsWith("where "))
                    w = "where " + w;
            }
            string sql = "select * from TF_View_Staff " + w + " order by 姓名 asc";
            dt = sqlHelper.Query(sql);
            return dt;
        }

        public bool ClearStaffs()
        {
            string sql = "delete from TF_Staff";
            int r = sqlHelper.ExecuteSql(sql);
            return r > 0;
        }
    }
}
