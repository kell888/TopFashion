using System;
using System.Collections;
using System.Text;
using System.Collections.Generic;

namespace TopFashion
{
    /// <summary>
    /// 表示由User对象组成的集合
    /// </summary>
    [Serializable]
    public class UserGroup : IArchitecture//, ICollection
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public UserGroup()
        {
            roles = new RoleCollection();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        public UserGroup(int id, string name)
        {
            this.id = id;
            if (name != null && name.Trim() != "")
                this.name = name;
            roles = new RoleCollection();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="roles"></param>
        public UserGroup(int id, string name, RoleCollection roles)
        {
            this.id = id;
            if (name != null && name.Trim() != "")
                this.name = name;
            if (roles == null)
                this.roles = new RoleCollection();
            else
                this.roles = roles;
        }

        /// <summary>
        /// 重载基类的ToString()
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.Name;
        }

        int id;

        /// <summary>
        /// 用户组ID
        /// </summary>
        public int ID
        {
            get { return id; }
            set { id = value; }
        }
        RoleCollection roles;

        /// <summary>
        /// 用户组拥有的角色集合
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

        string name = "未命名用户组";

        /// <summary>
        /// 用户组的名称
        /// </summary>
        public string Name
        {
            get { return name; }
            set
            {
                if (value != null && value.Trim() != "")
                    name = value;
            }
        }
        string remark;

        /// <summary>
        /// 用户组描述
        /// </summary>
        public string Remark
        {
            get { return remark; }
            set { remark = value; }
        }
    }

    public static class UserGroupListExtensions
    {
        public static bool ContainsUserGroup(this List<UserGroup> ugs, UserGroup ug)
        {
            foreach (UserGroup u in ugs)
            {
                if (u.ID == ug.ID)
                    return true;
            }
            return false;
        }

        public static bool ContainsUserGroup(this List<int> ugs, int ug)
        {
            return ugs.Contains(ug);
        }
    }
}
