using System;
using System.Collections.Generic;
using System.Text;

namespace TopFashion
{
    /// <summary>
    /// 权限类
    /// </summary>
    [Serializable]
    public class Permission : IArchitecture
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public Permission()
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public Permission(int id, string name, bool isExcept)
        {
            this.id = id;
            if (name != null && name.Trim() != "")
                this.name = name;
            this.isExcept = isExcept;
        }

        /// <summary>
        /// 由模块和操作构造权限对象
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isExcept"></param>
        /// <param name="module"></param>
        /// <param name="action"></param>
        public Permission(int id, bool isExcept, Module module, Action action)
        {
            this.id = id;
            this.isExcept = isExcept;
            this.name = module.Name + "-" + action.Name;
            this.module = module;
            this.action = action;
        }

        /// <summary>
        /// 由模块和操作构造权限对象
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="isExcept"></param>
        /// <param name="module"></param>
        /// <param name="action"></param>
        public Permission(int id, string name, bool isExcept, Module module, Action action)
        {
            this.id = id;
            if (name != null && name.Trim() != "")
                this.name = name;
            this.isExcept = isExcept;
            this.module = module;
            this.action = action;
        }

        int id;

        /// <summary>
        /// 权限ID
        /// </summary>
        public int ID
        {
            get { return id; }
            set { id = value; }
        }
        string name = "未命名权限";

        /// <summary>
        /// 权限的名称
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
        /// 权限描述
        /// </summary>
        public string Remark
        {
            get { return remark; }
            set { remark = value; }
        }

        bool isExcept;
        /// <summary>
        /// 是否为排除类的权限
        /// </summary>
        public bool IsExcept
        {
            get { return isExcept; }
            set { isExcept = value; }
        }
        
        Module module;

        /// <summary>
        /// 权限的模块
        /// </summary>
        public Module TheModule
        {
            get { return module; }
            set { module = value; }
        }
        Action action;

        /// <summary>
        /// 权限的操作
        /// </summary>
        public Action TheAction
        {
            get { return action; }
            set { action = value; }
        }

        /// <summary>
        /// 权限的值，用于验证具体的模块操作是否合法
        /// </summary>
        public string PermissionValue
        {
            get
            {
                if (module != null)
                {
                    if (action != null)
                    {
                        return isExcept + "-" + module.FormName + "-" + module.ControlName + "-" + action.ControlName;
                    }
                    else
                    {
                        return isExcept + "-" + module.FormName + "-" + module.ControlName;
                    }
                }
                else
                {
                    throw new Exception("Module Can Not Be Null!");
                }
            }
        }

        /// <summary>
        /// 权限名称与权限值的组合字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.Name;
        }
    }
}
