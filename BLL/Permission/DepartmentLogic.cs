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
                    dep.ParentID = Convert.ToInt32(dt.Rows[0]["Parent"]);
                if (dt.Rows[0]["Remark"] != null && dt.Rows[0]["Remark"] != DBNull.Value)
                    dep.Remark = dt.Rows[0]["Remark"].ToString();
                else
                    dep.Remark = "";
                return dep;
            }
            return null;
        }

        public int GetDepartmentParentId(int id)
        {
            string sql = "select Parent from TF_Depart where ID=" + id;
            DataTable dt = sqlHelper.Query(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                return Convert.ToInt32(dt.Rows[0]["Parent"]);
            }
            return 0;
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
                        dep.ParentID = Convert.ToInt32(dt.Rows[i]["Parent"]);
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
            //string parent = "0";
            //if (dep.Parent != null)
            //    parent = dep.Parent.ID.ToString();
            string sql = "insert into TF_Depart (Name, Manager, Parent, Remark) values ('" + dep.Name + "','" + dep.Manager + "', " + dep.ParentID + ", '" + dep.Remark + "'); select SCOPE_IDENTITY()";
            object obj = sqlHelper.ExecuteSqlReturn(sql);
            int R;
            if (obj != null && obj != DBNull.Value && int.TryParse(obj.ToString(), out R))
                return R;
            else
                return 0;
        }

        public bool UpdateDepartment(Department dep)
        {
            //string parent = "0";
            //if (dep.Parent != null)
            //    parent = dep.Parent.ID.ToString();
            string sql = "update TF_Depart set Name='" + dep.Name + "', Manager='" + dep.Manager + "', Parent=" + dep.ParentID + ", Remark='" + dep.Remark + "' where ID=" + dep.ID;
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
                //string parent = "0";
                //if (dep.Parent != null)
                //    parent = dep.Parent.ID.ToString();
                string sqlStr = "if exists (select 1 from TF_Depart where ID=" + dep.ID + ") update TF_Depart set Name='" + dep.Name + "', Manager='" + dep.Manager + "', Parent=" + dep.ParentID + ", Roles='" + Common.GetRolesStr(dep.Roles) + "', Remark='" + dep.Remark + "' where ID=" + dep.ID + " else insert into TF_Depart (Name, Manager, Parent, Roles, Remark) values ('" + dep.Name + "','" + dep.Manager + "', " + dep.ParentID + ", '" + Common.GetRolesStr(dep.Roles) + "', '" + dep.Remark + "')";
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
            return sqlHelper.Exists("select 1 from TF_Depart where Name='" + name + "'");
        }

        /// <summary>
        /// 是否存在出了自己以外的同名
        /// </summary>
        /// <param name="name"></param>
        /// <param name="myId"></param>
        /// <returns></returns>
        public bool ExistsNameOther(string name, int myId)
        {
            return sqlHelper.Exists("select 1 from TF_Depart where ID!=" + myId + " and Name='" + name + "'");
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
                return sqlHelper.Exists("select 1 from TF_Depart " + w);
            }
            return false;
        }
    }

    public static class DepartmentExtensions
    {
        /// <summary>
        /// 指定部门是否为我的后代
        /// </summary>
        /// <param name="dep">指定部门</param>
        /// <param name="recursion">是否递归判断后代，默认为true，只找下一代为false</param>
        /// <returns></returns>
        public static bool IsMyChildren(this Department thisDep, int depId, bool recursion = true)
        {
            int parentId = DepartmentLogic.GetInstance().GetDepartmentParentId(depId);
            if (parentId > 0)
            {
                if (parentId == thisDep.ID)//dep=1.2,this=1
                    return true;//dep.parent=1

                if (recursion)//dep=1.2.3
                {
                    return thisDep.IsMyChildren(parentId, true);//dep=1.2.3,this=1.2
                }
            }
            return false;
        }
        /// <summary>
        /// 指定部门是否为我的后代
        /// </summary>
        /// <param name="dep">指定部门</param>
        /// <param name="recursion">是否递归判断后代，默认为true，只找下一代为false</param>
        /// <returns></returns>
        public static bool IsMyChildren(this int thisDep, int depId, bool recursion = true)
        {
            int parentId = DepartmentLogic.GetInstance().GetDepartmentParentId(depId);
            if (parentId > 0)
            {
                if (parentId == thisDep)//dep=1.2,this=1
                    return true;//dep.parent=1

                if (recursion)//dep=1.2.3
                {
                    return thisDep.IsMyChildren(parentId, true);//dep=1.2.3,this=1.2
                }
            }
            return false;
        }

        public static RoleCollection Roles(this Department thisDep)
        {
            RoleCollection rs = Common.GetRoles(Common.GetRolesStr(thisDep.Roles));
            return rs;
        }
    }
}
