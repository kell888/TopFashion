using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace TopFashion
{
    public class RoleLogic
    {
        SQLDBHelper sqlHelper;
        static RoleLogic instance;
        public static RoleLogic GetInstance()
        {
            if (instance == null)
                instance = new RoleLogic();

            return instance;
        }

        private RoleLogic()
        {
            sqlHelper = new SQLDBHelper();
        }

        public Role GetRole(int id)
        {
            string sql = "select * from TF_Role where ID=" + id;
            DataTable dt = sqlHelper.Query(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                Role role = new Role();
                role.ID = id;
                role.Name = dt.Rows[0]["Name"].ToString();
                role.Flag = Convert.ToBoolean(dt.Rows[0]["Flag"]);
                role.Permissions = Common.GetPermissions(dt.Rows[0]["Permissions"].ToString());
                if (dt.Rows[0]["Remark"] != null && dt.Rows[0]["Remark"] != DBNull.Value)
                    role.Remark = dt.Rows[0]["Remark"].ToString();
                else
                    role.Remark = "";
                return role;
            }
            return null;
        }

        public Tuple<bool, List<int>> GetRoleFlagPermissions(int id)
        {
            string sql = "select Flag, Permissions from TF_Role where ID=" + id;
            DataTable dt = sqlHelper.Query(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                return new Tuple<bool,List<int>>(Convert.ToBoolean(dt.Rows[0]["Flag"]),Common.GetPermissionIds(dt.Rows[0]["Permissions"].ToString()));
            }
            return new Tuple<bool,List<int>>(false, null);
        }

        public List<Role> GetAllRoles()
        {
            List<Role> roles = new List<Role>();
            string sql = "select * from TF_Role";
            DataTable dt = sqlHelper.Query(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                PermissionLogic pl = PermissionLogic.GetInstance();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Role role = new Role();
                    role.ID = Convert.ToInt32(dt.Rows[i]["ID"]);
                    role.Name = dt.Rows[i]["Name"].ToString();
                    role.Flag = Convert.ToBoolean(dt.Rows[i]["Flag"]);
                    role.Permissions = Common.GetPermissions(dt.Rows[i]["Permissions"].ToString(), pl);
                    if (dt.Rows[i]["Remark"] != null && dt.Rows[i]["Remark"] != DBNull.Value)
                        role.Remark = dt.Rows[i]["Remark"].ToString();
                    else
                        role.Remark = "";
                    roles.Add(role);
                }
            }
            return roles;
        }

        public int AddRole(Role role)
        {
            string sql = "insert into TF_Role (Name, Permissions, Flag, Remark) values ('" + role.Name + "', '"+Common.GetPermissionsStr(role.Permissions)+"', "+(role.Flag ? "1" : "0")+", '" + role.Remark + "'); select SCOPE_IDENTITY()";
            object obj = sqlHelper.ExecuteSqlReturn(sql);
            int R;
            if (obj != null && obj != DBNull.Value && int.TryParse(obj.ToString(), out R))
                return R;
            else
                return 0;
        }

        public bool UpdateRole(Role role)
        {
            string sql = "update TF_Role set Name='" + role.Name + "', Permissions='" + Common.GetPermissionsStr(role.Permissions) + "', Flag=" + (role.Flag ? "1" : "0") + ", Remark='" + role.Remark + "' where ID=" + role.ID;
            int r = sqlHelper.ExecuteSql(sql);
            return r > 0;
        }

        public bool DeleteRole(Role role)
        {
            string sql = "delete from TF_Role where ID=" + role.ID;
            int r = sqlHelper.ExecuteSql(sql);
            return r > 0;
        }
        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool UpgradeList(List<Role> list)
        {
            int errCount = 0;
            foreach (Role role in list)
            {
                string sqlStr = "if exists (select 1 from TF_Role where ID=" + role.ID + ") update TF_Role set Name='" + role.Name + "', Permissions='" + Common.GetPermissionsStr(role.Permissions) + "', Flag=" + (role.Flag ? "1" : "0") + ", Remark='" + role.Remark + "' where ID=" + role.ID + " else insert into TF_Role (Name, Permissions, Flag, Remark) values ('" + role.Name + "', '" + Common.GetPermissionsStr(role.Permissions) + "', " + (role.Flag ? "1" : "0") + ", '" + role.Remark + "')";
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
            return sqlHelper.Exists("select 1 from TF_Role where Name='" + name + "'");
        }

        /// <summary>
        /// 是否存在出了自己以外的同名
        /// </summary>
        /// <param name="name"></param>
        /// <param name="myId"></param>
        /// <returns></returns>
        public bool ExistsNameOther(string name, int myId)
        {
            return sqlHelper.Exists("select 1 from TF_Role where ID!=" + myId + " and Name='" + name + "'");
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
                return sqlHelper.Exists("select 1 from TF_Role " + w);
            }
            return false;
        }
    }

    public static class RoleExtensions
    {
        public static PermissionCollection Permissions(this Role thisRole)
        {
            PermissionCollection ps = Common.GetPermissions(Common.GetPermissionsStr(thisRole.Permissions));
            return ps;
        }
    }
}
