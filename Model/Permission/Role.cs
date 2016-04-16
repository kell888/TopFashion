using System;
using System.Collections.Generic;
using System.Text;

namespace TopFashion
{
    /// <summary>
    /// 角色类
    /// </summary>
    [Serializable]
    public class Role : IArchitecture
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public Role()
        {
            this.permissions = new PermissionCollection();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        public Role(int id, string name)
        {
            this.id = id;
            if (name != null && name.Trim() != "")
                this.name = name;
            this.permissions = new PermissionCollection();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="permissions"></param>
        public Role(int id, string name, PermissionCollection permissions)
        {
            this.id = id;
            if (name != null && name.Trim() != "")
                this.name = name;
            if (permissions == null)
                this.permissions = new PermissionCollection();
            else
                this.permissions = permissions;
        }

        int id;

        /// <summary>
        /// 角色ID
        /// </summary>
        public int ID
        {
            get { return id; }
            set { id = value; }
        }
        string name = "未命名角色";

        /// <summary>
        /// 角色名称
        /// </summary>
        public string Name
        {
            get { return name; }
            set {
                if (value != null && value.Trim() != "")
                    name = value;
            }
        }

        PermissionCollection permissions;

        /// <summary>
        /// 角色拥有的权限集合
        /// </summary>
        public PermissionCollection Permissions
        {
            get { return permissions; }
            set
            {
                if (value != null)
                    permissions = value;
            }
        }
        bool flag = true;

        /// <summary>
        /// 启用状态(默认：true启用)
        /// </summary>
        public bool Flag
        {
            get { return flag; }
            set { flag = value; }
        }

        string remark;
        /// <summary>
        /// 角色描述
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
            return this.Name;
        }
    }
}
