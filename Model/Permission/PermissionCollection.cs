using System;
using System.Collections;
using System.Text;
using System.Collections.Generic;

namespace TopFashion
{
    /// <summary>
    /// 表示由Permission对象组成的集合
    /// </summary>
    [Serializable]
    public class PermissionCollection : IList<Permission>, IArchitecture
    {
        private IList<Permission> list;

        /// <summary>
         /// 构造函数
        /// </summary>
        public PermissionCollection()
        {
            list = new List<Permission>();
        }

        /// <summary>
        /// 索引器
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Permission this[int index]
        {
            get
            {
                return this.list[index];
            }
            set
            {
                this.list[index] = value;
            }
        }

        /// <summary>
        /// 向集合中添加元素
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public void Add(Permission item)
        {
            this.list.Add(item);
        }

        public void Insert(int index, Permission item)
        {
            this.list.Insert(index, item);
        }

        /// <summary>
        /// 从集合中移出指定元素
        /// </summary>
        /// <param name="item"></param>
        public void Remove(Permission item)
        {
            this.list.Remove(item);
        }

        /// <summary>
        /// 从集合中移出指定索引的元素
        /// </summary>
        /// <param name="index"></param>
        public void RemoveAt(int index)
        {
            this.list.RemoveAt(index);
        }

        public void Clear()
        {
            this.list.Clear();
        }

        public bool Contains(Permission item)
        {
            return this.list.Contains(item);
        }

        public int IndexOf(Permission item)
        {
            return this.list.IndexOf(item);
        }

        public bool IsReadOnly
        {
            get { return this.list.IsReadOnly; }
        }

        /// <summary>
        /// 取得集合元素个数
        /// </summary>
        public int Count
        {
            get
            {
                return this.list.Count;
            }
        }

        void ICollection<Permission>.CopyTo(Permission[] array, int index)
        {
            this.list.CopyTo(array, index);
        }

        int ICollection<Permission>.Count
        {
            get { return this.list.Count; }
        }

        bool ICollection<Permission>.Remove(Permission item)
        {
            return this.list.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.list.GetEnumerator();
        }

        IEnumerator<Permission> IEnumerable<Permission>.GetEnumerator()
        {
            return (IEnumerator<Permission>)this.list.GetEnumerator();
        }
    }

    public static class PermissionCollectionExtensions
    {
        /// <summary>
        /// 包含权限的判断
        /// </summary>
        /// <param name="perms"></param>
        /// <param name="perm"></param>
        /// <returns></returns>
        public static bool ContainsPermission(this PermissionCollection perms, Permission perm)
        {
            foreach (Permission per in perms)
            {
                try
                {
                    //if (per.PermissionValue.Equals(perm.PermissionValue, StringComparison.InvariantCultureIgnoreCase))
                    if (per.ID == perm.ID)
                        return true;
                }
                catch { }
            }
            return false;
        }
        /// <summary>
        /// 包含权限的判断
        /// </summary>
        /// <param name="perms"></param>
        /// <param name="perm"></param>
        /// <returns></returns>
        public static bool ContainsPermission(this List<int> perms, int perm)
        {
            return perms.Contains(perm);
        }
        /// <summary>
        /// 界面中包含权限的判断
        /// </summary>
        /// <param name="perms"></param>
        /// <param name="formName"></param>
        /// <param name="moduleName"></param>
        /// <param name="actionName"></param>
        /// <returns></returns>
        public static bool ContainsControl(this PermissionCollection perms, string formName, string moduleName, string actionName = "")
        {
            try
            {
                foreach (Permission per in perms)
                {
                    if (!per.IsExcept)
                    {
                        if (per.TheModule != null)
                        {
                            if (per.TheAction != null)
                            {
                                if (per.TheModule.FormName.Equals(formName, StringComparison.InvariantCultureIgnoreCase) && per.TheModule.ControlName.Equals(moduleName, StringComparison.InvariantCultureIgnoreCase) && per.TheModule.FormName.Equals(per.TheAction.FormName, StringComparison.InvariantCultureIgnoreCase) && per.TheAction.ControlName.Equals(actionName, StringComparison.InvariantCultureIgnoreCase))
                                {
                                    return true;
                                }
                                else if (actionName.StartsWith("tabControl") && per.TheAction.FormName.Equals(formName, StringComparison.InvariantCultureIgnoreCase) && per.TheAction.ControlName.Equals(actionName, StringComparison.InvariantCultureIgnoreCase))
                                {
                                    return true;
                                }
                            }
                            else
                            {
                                if (per.TheModule.FormName.Equals(formName, StringComparison.InvariantCultureIgnoreCase) && per.TheModule.ControlName.Equals(actionName, StringComparison.InvariantCultureIgnoreCase))
                                    return true;
                            }
                        }
                    }
                }
            }
            catch { }
            return false;
        }
        /// <summary>
        /// 界面中排除权限的判断
        /// </summary>
        /// <param name="perms"></param>
        /// <param name="formName"></param>
        /// <param name="moduleName"></param>
        /// <param name="actionName"></param>
        /// <returns></returns>
        public static bool ExceptControl(this PermissionCollection perms, string formName, string moduleName, string actionName = "")
        {
            try
            {
                foreach (Permission per in perms)
                {
                    if (per.IsExcept)
                    {
                        if (per.TheModule != null)
                        {
                            if (per.TheAction != null)
                            {
                                if (per.TheModule.FormName.Equals(formName, StringComparison.InvariantCultureIgnoreCase) && per.TheModule.ControlName.Equals(moduleName, StringComparison.InvariantCultureIgnoreCase) && per.TheModule.FormName.Equals(per.TheAction.FormName, StringComparison.InvariantCultureIgnoreCase) && per.TheAction.ControlName.Equals(actionName, StringComparison.InvariantCultureIgnoreCase))
                                    return true;
                            }
                            else
                            {
                                if (per.TheModule.FormName.Equals(formName, StringComparison.InvariantCultureIgnoreCase) && per.TheModule.ControlName.Equals(actionName, StringComparison.InvariantCultureIgnoreCase))
                                    return true;
                            }
                        }
                    }
                }
            }
            catch { }
            return false;
        }
    }
}
