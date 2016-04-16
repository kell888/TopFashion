using System;
using System.Collections.Generic;
using System.Text;

namespace TopFashion
{
    /// <summary>
    /// 部门类
    /// </summary>
    [Serializable]
    public class Department : IArchitecture
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public Department()
        {
            roles = new RoleCollection();
        }

        /// <summary>
        /// 以名称创建部门对象
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        public Department(int id, string name)
        {
            this.id = id;
            if (name != null && name.Trim() != "")
                this.name = name;
            roles = new RoleCollection();
        }

        /// <summary>
        /// 以名称和角色集合创建部门对象
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="roles"></param>
        public Department(int id, string name, RoleCollection roles)
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
        /// 以名称和上级部门创建新部门对象
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="parent"></param>
        public Department(int id, string name, int parent)
        {
            this.id = id;
            if (name != null && name.Trim() != "")
                this.name = name;
            this.parentId = parent;
            roles = new RoleCollection();
        }

        /// <summary>
        /// 以名称、角色集合和上级部门创建新部门对象
        /// </summary>
        /// <param name="name"></param>
        /// <param name="roles"></param>
        /// <param name="parent"></param>
        public Department(int id, string name, RoleCollection roles, int parent)
        {
            this.id = id;
            if (name != null && name.Trim() != "")
                this.name = name;
            if (roles == null)
                this.roles = new RoleCollection();
            else
                this.roles = roles;
            this.parentId = parent;
        }

        /// <summary>
        /// 以指定的名称创建子部门
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public Department CreateSubDepartment(int id, string name)
        {
            return new Department(id, name, this.ID);
        }

        /// <summary>
        /// 以指定的名称创建子部门
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="roles"></param>
        /// <returns></returns>
        public Department CreateSubDepartment(int id, string name, RoleCollection roles)
        {
            return new Department(id, name, roles, this.ID);
        }

        /// <summary>
        /// 指定部门是否为我的后代
        /// </summary>
        /// <param name="dep">指定部门</param>
        /// <param name="recursion">是否递归判断后代，默认为true，只找下一代为false</param>
        /// <returns></returns>
        public bool IsMyChildren(Department dep, bool recursion = true)
        {
            if (dep.Parent != null)
            {
                if (dep.Parent.ID == this.ID)//dep=1.2,this=1
                    return true;//dep.parent=1

                if (recursion)//dep=1.2.3
                {
                    return this.IsMyChildren(dep.Parent, true);//dep=1.2.3,this=1.2
                }
            }
            return false;
        }

        int id;

        /// <summary>
        /// 部门ID
        /// </summary>
        public int ID
        {
            get { return id; }
            set { id = value; }
        }
        RoleCollection roles;

        /// <summary>
        /// 部门拥有的角色集合
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
        string name = "未命名部门";

        /// <summary>
        /// 部门名称
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
        string manager;

        public string Manager
        {
            get { return manager; }
            set { manager = value; }
        }

        int parentId;

        /// <summary>
        /// 上级部门
        /// </summary>
        public int ParentID
        {
            get { return parentId; }
            set { parentId = value; }
        }

        public Department Parent
        {
            get
            {
                if (parentId > 0)
                {
                    Department d = new Department();
                    d.ID = parentId;
                    return d;
                }
                return null;
            }
        }
        string remark;

        /// <summary>
        /// 部门描述
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

    public static class DepartmentListExtensions
    {
        public static bool ContainsDepartment(this List<Department> deps, Department dep)
        {
            foreach (Department d in deps)
            {
                if (d.ID == dep.ID)
                    return true;
            }
            return false;
        }

        public static bool ContainsDepartment(this List<int> deps, int dep)
        {
            return deps.Contains(dep);
        }
    }
}
