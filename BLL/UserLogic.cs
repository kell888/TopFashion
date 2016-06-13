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
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    User user = new User();
                    user.ID = Convert.ToInt32(dt.Rows[i]["ID"]);
                    user.Username = dt.Rows[i]["Username"].ToString();
                    user.Departments = Common.GetDepartments(dt.Rows[i]["Depart"].ToString());
                    user.Flag = Convert.ToInt32(dt.Rows[i]["Flag"]);
                    if (dt.Rows[i]["Password"] != null && dt.Rows[i]["Password"] != DBNull.Value)
                        user.Password = dt.Rows[i]["Password"].ToString();
                    else
                        user.Password = "";
                    if (dt.Rows[i]["Roles"] != null && dt.Rows[i]["Roles"] != DBNull.Value)
                        user.Roles = Common.GetRoles(dt.Rows[i]["Roles"].ToString());
                    if (dt.Rows[i]["Usergroup"] != null && dt.Rows[i]["Usergroup"] != DBNull.Value)
                        user.Usergroups = Common.GetUserGroups(dt.Rows[i]["Usergroup"].ToString());
                    if (dt.Rows[i]["Remark"] != null && dt.Rows[i]["Remark"] != DBNull.Value)
                        user.Remark = dt.Rows[i]["Remark"].ToString();
                    users.Add(user);
                }
            }
            return users;
        }

        public int AddUser(User user)
        {
            string sql = "insert into TF_User (Username, Password, Depart, Flag, Roles, Usergroup, Remark) values ('" + user.Username + "','" + user.Password + "','" + Common.GetDepartmentsStr(user.Departments) + "'," + user.Flag + ",'" + Common.GetRolesStr(user.Roles) + "','" + Common.GetUserGroupsStr(user.Usergroups) + "','" + user.Remark + "'); select SCOPE_IDENTITY()";
            object obj = sqlHelper.ExecuteSqlReturn(sql);
            int R;
            if (obj != null && obj != DBNull.Value && int.TryParse(obj.ToString(), out R))
                return R;
            else
                return 0;
        }

        public bool UpdateUser(User user)
        {
            string sql = "update TF_User set Username='" + user.Username + "',Password='" + user.Password + "', Depart='" + Common.GetDepartmentsStr(user.Departments) + "', Flag=" + user.Flag + ", Roles='" + Common.GetRolesStr(user.Roles) + "', Usergroup='" + Common.GetUserGroupsStr(user.Usergroups) + "', Remark='" + user.Remark + "' where ID=" + user.ID;
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
    }
}
