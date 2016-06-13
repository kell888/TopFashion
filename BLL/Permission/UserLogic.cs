using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace TopFashion
{
    public class UserLogic
    {
        SQLDBHelper sqlHelper;
        static UserLogic instance;
        public static UserLogic GetInstance()
        {
            if (instance == null)
                instance = new UserLogic();

            return instance;
        }

        private UserLogic()
        {
            sqlHelper = new SQLDBHelper();
        }

        public User GetUser(int id)
        {
            string sql = "select * from TF_User where ID=" + id;
            DataTable dt = sqlHelper.Query(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                User user = new User();
                user.ID = id;
                user.Username = dt.Rows[0]["Username"].ToString();
                user.Departments = Common.GetDepartments(dt.Rows[0]["Depart"].ToString());
                user.Flag = Convert.ToInt32(dt.Rows[0]["Flag"]);
                if (dt.Rows[0]["Password"] != null && dt.Rows[0]["Password"] != DBNull.Value)
                    user.Password = dt.Rows[0]["Password"].ToString();
                else
                    user.Password = "";
                if (dt.Rows[0]["Roles"] != null && dt.Rows[0]["Roles"] != DBNull.Value)
                    user.Roles = Common.GetRoles(dt.Rows[0]["Roles"].ToString());
                if (dt.Rows[0]["Usergroup"] != null && dt.Rows[0]["Usergroup"] != DBNull.Value)
                    user.Usergroups = Common.GetUserGroups(dt.Rows[0]["Usergroup"].ToString());
                if (dt.Rows[0]["Remark"] != null && dt.Rows[0]["Remark"] != DBNull.Value)
                    user.Remark = dt.Rows[0]["Remark"].ToString();
                return user;
            }
            return null;
        }

        public User GetUser(string username)
        {
            string sql = "select * from TF_User where Username='"+username+"'";
            DataTable dt = sqlHelper.Query(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                User user = new User();
                user.ID = Convert.ToInt32(dt.Rows[0]["ID"]);
                user.Username = username;
                user.Departments = Common.GetDepartments(dt.Rows[0]["Depart"].ToString());
                user.Flag = Convert.ToInt32(dt.Rows[0]["Flag"]);
                if (dt.Rows[0]["Password"] != null && dt.Rows[0]["Password"] != DBNull.Value)
                    user.Password = dt.Rows[0]["Password"].ToString();
                else
                    user.Password = "";
                if (dt.Rows[0]["Roles"] != null && dt.Rows[0]["Roles"] != DBNull.Value)
                    user.Roles = Common.GetRoles(dt.Rows[0]["Roles"].ToString());
                if (dt.Rows[0]["Usergroup"] != null && dt.Rows[0]["Usergroup"] != DBNull.Value)
                    user.Usergroups = Common.GetUserGroups(dt.Rows[0]["Usergroup"].ToString());
                if (dt.Rows[0]["Remark"] != null && dt.Rows[0]["Remark"] != DBNull.Value)
                    user.Remark = dt.Rows[0]["Remark"].ToString();
                return user;
            }
            return null;
        }

        public List<User> GetAllUsers()
        {
            List<User> users = new List<User>();
            string sql = "select * from TF_User";
            DataTable dt = sqlHelper.Query(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                DepartmentLogic dl = DepartmentLogic.GetInstance();
                RoleLogic rl = RoleLogic.GetInstance();
                UserGroupLogic ul = UserGroupLogic.GetInstance();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    User user = new User();
                    user.ID = Convert.ToInt32(dt.Rows[i]["ID"]);
                    user.Username = dt.Rows[i]["Username"].ToString();
                    user.Departments = Common.GetDepartments(dt.Rows[i]["Depart"].ToString(), dl);
                    user.Flag = Convert.ToInt32(dt.Rows[i]["Flag"]);
                    if (dt.Rows[i]["Password"] != null && dt.Rows[i]["Password"] != DBNull.Value)
                        user.Password = dt.Rows[i]["Password"].ToString();
                    else
                        user.Password = "";
                    if (dt.Rows[i]["Roles"] != null && dt.Rows[i]["Roles"] != DBNull.Value)
                        user.Roles = Common.GetRoles(dt.Rows[i]["Roles"].ToString(), rl);
                    if (dt.Rows[i]["Usergroup"] != null && dt.Rows[i]["Usergroup"] != DBNull.Value)
                        user.Usergroups = Common.GetUserGroups(dt.Rows[i]["Usergroup"].ToString(), ul);
                    if (dt.Rows[i]["Remark"] != null && dt.Rows[i]["Remark"] != DBNull.Value)
                        user.Remark = dt.Rows[i]["Remark"].ToString();
                    users.Add(user);
                }
            }
            return users;
        }

        public int AddUser(User user)
        {
            string sql = "insert into TF_User (Username, Password, Depart, Flag, Remark) values ('" + user.Username + "', '" + user.Password + "', '" + Common.GetDepartmentsStr(user.Departments) + "'," + user.Flag + ", '" + user.Remark + "'); select SCOPE_IDENTITY()";
            object obj = sqlHelper.ExecuteSqlReturn(sql);
            int R;
            if (obj != null && obj != DBNull.Value && int.TryParse(obj.ToString(), out R))
                return R;
            else
                return 0;
        }

        public bool UpdateUser(User user)
        {
            string sql = "update TF_User set Username='" + user.Username + "',Password='" + user.Password + "', Depart='" + Common.GetDepartmentsStr(user.Departments) + "', Flag=" + user.Flag + ", Remark='" + user.Remark + "' where ID=" + user.ID;
            int r = sqlHelper.ExecuteSql(sql);
            return r > 0;
        }

        public bool ChangePwd(int id, string pwd)
        {
            string sql = "update TF_User set Password='" + pwd + "' where ID=" + id;
            int r = sqlHelper.ExecuteSql(sql);
            return r > 0;
        }

        public bool DeleteUser(User user)
        {
            string sql = "delete from TF_User where ID=" + user.ID;
            int r = sqlHelper.ExecuteSql(sql);
            return r > 0;
        }
        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool UpgradeList(List<User> list)
        {
            int errCount = 0;
            foreach (User user in list)
            {
                string sqlStr = "if exists (select 1 from TF_User where ID=" + user.ID + ") update TF_User set Username='" + user.Username + "',Password='" + user.Password + "', Depart='" + Common.GetDepartmentsStr(user.Departments) + "', Flag=" + user.Flag + ", Roles='" + Common.GetRolesStr(user.Roles) + "', Usergroup='" + Common.GetUserGroupsStr(user.Usergroups) + "', Remark='" + user.Remark + "' where ID=" + user.ID + " else insert into TF_User (Username, Password, Depart, Flag, Roles, Usergroup, Remark) values ('" + user.Username + "','" + user.Password + "','" + Common.GetDepartmentsStr(user.Departments) + "'," + user.Flag + ",'" + Common.GetRolesStr(user.Roles) + "','" + Common.GetUserGroupsStr(user.Usergroups) + "','" + user.Remark + "')";
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
            return sqlHelper.Exists("select 1 from TF_User where Username='" + name + "'");
        }

        /// <summary>
        /// 是否存在出了自己以外的同名
        /// </summary>
        /// <param name="name"></param>
        /// <param name="myId"></param>
        /// <returns></returns>
        public bool ExistsNameOther(string name, int myId)
        {
            return sqlHelper.Exists("select 1 from TF_User where ID!=" + myId + " and Username='" + name + "'");
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
                return sqlHelper.Exists("select 1 from TF_User " + w);
            }
            return false;
        }
    }

    public static class UserExtensions
    {
        /// <summary>
        /// 当前用户是否属于指定的部门
        /// </summary>
        /// <param name="depart"></param>
        /// <returns></returns>
        public static bool BelongToDepart(this User user, Department depart, bool recursion)
        {
            foreach (Department dep in user.Departments)
            {
                if (dep.ID == depart.ID)
                {
                    return true;
                }
                else
                {
                    return depart.IsMyChildren(dep, recursion);
                }
            }
            return false;
        }
        /// <summary>
        /// 当前用户是否属于指定的部门
        /// </summary>
        /// <param name="departId"></param>
        /// <returns></returns>
        public static bool BelongToDepart(this User user, int departId, bool recursion)
        {
            List<int> depIds = new List<int>();
            user.Departments.ForEach(a => depIds.Add(a.ID));
            foreach (int dep in depIds)
            {
                if (dep == departId)
                {
                    return true;
                }
                else
                {
                    return departId.IsMyChildren(dep, recursion);
                }
            }
            return false;
        }

        /// <summary>
        /// 获取当前用户拥有的所有权限
        /// </summary>
        public static PermissionCollection GetAllPermissionsByUser(this User user)
        {
            PermissionCollection ps = new PermissionCollection();
            foreach (Role role in user.Roles)
            {
                //Role role = RoleLogic.GetInstance().GetRole(r);
                if (role.Flag)
                {
                    //PermissionLogic pl = PermissionLogic.GetInstance();
                    foreach (Permission per in role.Permissions)
                    {
                        //Permission per = pl.GetPermission(p);
                        if (!ps.ContainsPermission(per))
                            ps.Add(per);
                    }
                }
            }
            foreach (Department dep in user.Departments)
            {
                //Department dep = DepartmentLogic.GetInstance().GetDepartment(d);
                foreach (Role role in dep.Roles)
                {
                    //Role role = RoleLogic.GetInstance().GetRole(r);
                    if (role.Flag)
                    {
                        //PermissionLogic pl = PermissionLogic.GetInstance();
                        foreach (Permission per in role.Permissions)
                        {
                            //Permission per = pl.GetPermission(p);
                            if (!ps.ContainsPermission(per))
                                ps.Add(per);
                        }
                    }
                }
            }
            foreach (UserGroup ugg in user.Usergroups)
            {
                //UserGroup ugg = UserGroupLogic.GetInstance().GetUserGroup(ug);
                foreach (Role role in ugg.Roles)
                {
                    //Role role = RoleLogic.GetInstance().GetRole(r);
                    if (role.Flag)
                    {
                        //PermissionLogic pl = PermissionLogic.GetInstance();
                        foreach (Permission per in role.Permissions)
                        {
                            //Permission per = pl.GetPermission(p);
                            if (!ps.ContainsPermission(per))
                                ps.Add(per);
                        }
                    }
                }
            }
            return ps;
        }

        /// <summary>
        /// 获取当前用户拥有的所有权限
        /// </summary>
        public static PermissionCollection GetAllPermissions(this User user, bool Obsolete)
        {
            PermissionCollection ps = new PermissionCollection();
            foreach (Role r in user.Roles())
            {
                if (r.Flag)
                {
                    foreach (Permission p in r.Permissions)
                    {
                        if (!ps.ContainsPermission(p))
                            ps.Add(p);
                    }
                }
            }
            foreach (Department dep in user.Departments)
            {
                //Department dep = DepartmentLogic.GetInstance().GetDepartment(d);
                foreach (Role r in dep.Roles())
                {
                    if (r.Flag)
                    {
                        foreach (Permission p in r.Permissions)
                        {
                            if (!ps.ContainsPermission(p))
                                ps.Add(p);
                        }
                    }
                }
            }
            foreach (UserGroup ugg in user.Usergroups)
            {
                //UserGroup ugg = UserGroupLogic.GetInstance().GetUserGroup(ug);
                foreach (Role r in ugg.Roles())
                {
                    if (r.Flag)
                    {
                        foreach (Permission p in r.Permissions())
                        {
                            if (!ps.ContainsPermission(p))
                                ps.Add(p);
                        }
                    }
                }
            }
            return ps;
        }

        /// <summary>
        /// 获取当前用户拥有的所有权限
        /// </summary>
        public static List<int> GetAllPermissions(this User user)
        {
            List<int> ps = new List<int>();
            List<int> roleIds = new List<int>();
            foreach (Role role in user.Roles)
            {
                roleIds.Add(role.ID);
            }
            foreach (int r in roleIds)
            {
                Tuple<bool, List<int>> tuple = RoleLogic.GetInstance().GetRoleFlagPermissions(r);
                if (tuple.Item1)
                {
                    foreach (int p in tuple.Item2)
                    {
                        if (!ps.ContainsPermission(p))
                            ps.Add(p);
                    }
                }
            }
            foreach (Department d in user.Departments)
            {
                roleIds.Clear();
                foreach (Role role in d.Roles)
                {
                    roleIds.Add(role.ID);
                }
                foreach (int r in roleIds)
                {
                    Tuple<bool, List<int>> tuple = RoleLogic.GetInstance().GetRoleFlagPermissions(r);
                    if (tuple.Item1)
                    {
                        foreach (int p in tuple.Item2)
                        {
                            if (!ps.ContainsPermission(p))
                                ps.Add(p);
                        }
                    }
                }
            }
            foreach (UserGroup ug in user.Usergroups)
            {
                roleIds.Clear();
                foreach (Role role in ug.Roles)
                {
                    roleIds.Add(role.ID);
                }
                foreach (int r in roleIds)
                {
                    Tuple<bool, List<int>> tuple = RoleLogic.GetInstance().GetRoleFlagPermissions(r);
                    if (tuple.Item1)
                    {
                        foreach (int p in tuple.Item2)
                        {
                            if (!ps.ContainsPermission(p))
                                ps.Add(p);
                        }
                    }
                }
            }
            return ps;
        }

        public static RoleCollection Roles(this User thisUser)
        {
            RoleCollection rs = Common.GetRoles(Common.GetRolesStr(thisUser.Roles));
            return rs;
        }
    }
}
