using System;
using System.Collections.Generic;
using System.Text;

namespace TopFashion
{
    /// <summary>
    /// 用户类
    /// </summary>
    [Serializable]
    public class User : IArchitecture
    {
        int id;

        /// <summary>
        /// 用户ID
        /// </summary>
        public int ID
        {
            get { return id; }
            set { id = value; }
        }
        List<Department> departments;

        /// <summary>
        /// 所在部门ID
        /// </summary>
        public List<Department> Departments
        {
            get { return departments; }
            set
            {
                if (value != null)
                    departments = value;
            }
        }
        List<UserGroup> userGroups;

        /// <summary>
        /// 所在用户组
        /// </summary>
        public List<UserGroup> Usergroups
        {
            get { return userGroups; }
            set
            {
                if (value != null)
                    userGroups = value;
            }
        }

        RoleCollection roles;

        /// <summary>
        /// 用户拥有的角色集合
        /// </summary>
        public RoleCollection Roles
        {
            get { return roles; }
            set
            {
                if (value != null)
                    roles = value;
            }
        }
        string userName = "未命名用户";

        /// <summary>
        /// 用户名
        /// </summary>
        public string Username
        {
            get { return userName; }
            set
            {
                if (value != null && value.Trim() != "")
                    userName = value;
            }
        }
        string password;

        /// <summary>
        /// 密码
        /// </summary>
        public string Password
        {
            get { return password; }
            set { password = value; }
        }
        int flag;

        /// <summary>
        /// 状态
        /// </summary>
        public int Flag
        {
            get { return flag; }
            set { flag = value; }
        }
        string remark;

        /// <summary>
        /// 用户描述
        /// </summary>
        public string Remark
        {
            get { return remark; }
            set { remark = value; }
        }

        /// <summary>
        /// 重载基类的ToString()
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.Username;
        }

        /// <summary>
        /// 用户的默认构造函数
        /// </summary>
        public User()
        {
            departments = new List<Department>();
            roles = new RoleCollection();
            userGroups = new List<UserGroup>();
        }

        /// <summary>
        /// 由用户名和密码构造用户对象
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        public User(int id, string userName, string password)
        {
            this.id = id;
            departments = new List<Department>();
            roles = new RoleCollection();
            userGroups = new List<UserGroup>();
            if (userName != null && userName.Trim() != "")
                this.userName = userName;
            this.password = password;
        }

        /// <summary>
        /// 由用户名、密码和所在部门ID构造用户对象
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="departId"></param>
        public User(int id, string userName, string password, Department depart)
        {
            this.id = id;
            this.departments = new List<Department>();
            roles = new RoleCollection();
            userGroups = new List<UserGroup>();
            if (userName != null && userName.Trim() != "")
                this.userName = userName;
            this.password = password;
            this.departments.Add(depart);
        }
    }
}
