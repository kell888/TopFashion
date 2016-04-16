using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace TopFashion
{
    public class DepartmentLogic
    {
        SQLDBHelper sqlHelper;
        static DepartmentLogic instance;
        public static DepartmentLogic GetInstance()
        {
            if (instance == null)
                instance = new DepartmentLogic();

            return instance;
        }

        private DepartmentLogic()
        {
            sqlHelper = new SQLDBHelper();
        }

        public Department GetDepartment(int id)
        {
            string sql = "select * from TF_Depart where ID=" + id;
            DataTable dt = sqlHelper.Query(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                Department dep = new Department();
                dep.ID = id;
                dep.Name = dt.Rows[0]["Name"].ToString();
                dep.Manager = dt.Rows[0]["Manager"].ToString();
                if (dt.Rows[0]["Parent"] != null && dt.Rows[0]["Parent"] != DBNull.Value)
                    dep.Parent = GetDepartment(Convert.ToInt32(dt.Rows[0]["Parent"]));
                if (dt.Rows[0]["Remark"] != null && dt.Rows[0]["Remark"] != DBNull.Value)
                    dep.Remark = dt.Rows[0]["Remark"].ToString();
                else
                    dep.Remark = "";
                return dep;
            }
            return null;
        }

        public List<Department> GetAllDepartments()
        {
            List<Department> deps = new List<Department>();
            string sql = "select * from TF_Depart";
            DataTable dt = sqlHelper.Query(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Department dep = new Department();
                    dep.ID = Convert.ToInt32(dt.Rows[i]["ID"]);
                    dep.Name = dt.Rows[i]["Name"].ToString();
                    dep.Manager = dt.Rows[i]["Manager"].ToString();
                    if (dt.Rows[i]["Parent"] != null && dt.Rows[i]["Parent"] != DBNull.Value)
                        dep.Parent = GetDepartment(Convert.ToInt32(dt.Rows[i]["Parent"]));
                    if (dt.Rows[i]["Remark"] != null && dt.Rows[i]["Remark"] != DBNull.Value)
                        dep.Remark = dt.Rows[i]["Remark"].ToString();
                    else
                        dep.Remark = "";
                    deps.Add(dep);
                }
            }
            return deps;
        }

        public int AddDepartment(Department dep)
        {
            string parent = "0";
            if (dep.Parent != null)
                parent = dep.Parent.ID.ToString();
            string sql = "insert into TF_Depart (Name, Manager,Parent, Remark) values ('" + dep.Name + "','" + dep.Manager + "', " + parent + ", '" + dep.Remark + "'); select SCOPE_IDENTITY()";
            object obj = sqlHelper.ExecuteSqlReturn(sql);
            int R;
            if (obj != null && obj != DBNull.Value && int.TryParse(obj.ToString(), out R))
                return R;
            else
                return 0;
        }

        public bool UpdateDepartment(Department dep)
        {
            string parent = "0";
            if (dep.Parent != null)
                parent = dep.Parent.ID.ToString();
            string sql = "update TF_Depart set Name='" + dep.Name + "', Manager='" + dep.Manager + "', Parent=" + parent + ", Remark='" + dep.Remark + "' where ID=" + dep.ID;
            int r = sqlHelper.ExecuteSql(sql);
            return r > 0;
        }

        public bool DeleteDepartment(Department dep)
        {
            string sql = "delete from TF_Depart where ID=" + dep.ID;
            int r = sqlHelper.ExecuteSql(sql);
            return r > 0;
        }
        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool UpgradeList(List<Department> list)
        {
            int errCount = 0;
            foreach (Department dep in list)
            {
                string parent = "0";
                if (dep.Parent != null)
                    parent = dep.Parent.ID.ToString();
                string sqlStr = "if exists (select 1 from TF_Depart where ID=" + dep.ID + ") update TF_Depart set Name='" + dep.Name + "', Manager='" + dep.Manager + "', Parent=" + parent + ", Remark='" + dep.Remark + "' where ID=" + dep.ID + " else insert into TF_Depart (Name, Manager,Parent, Remark) values ('" + dep.Name + "','" + dep.Manager + "', " + parent + ", '" + dep.Remark + "')";
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
    }
}
