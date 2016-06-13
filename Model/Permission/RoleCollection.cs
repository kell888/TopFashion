using System;
using System.Collections;
using System.Text;
using System.Collections.Generic;

namespace TopFashion
{
    /// <summary>
    /// 表示由Role对象组成的集合
    /// </summary>
    [Serializable]
    public class RoleCollection : IList<Role>, IArchitecture
    {
        private IList<Role> list;
        /// <summary>
        /// 构造函数
        /// </summary>
        public RoleCollection()
        {
            list = new List<Role>();
        }

        /// <summary>
        /// 索引器
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Role this[int index]
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
        public void Add(Role item)
        {
            this.list.Add(item);
        }

        public void Insert(int index, Role item)
        {
            this.list.Insert(index, item);
        }

        /// <summary>
        /// 从集合中移出指定元素
        /// </summary>
        /// <param name="item"></param>
        public void Remove(Role item)
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

        public bool Contains(Role item)
        {
            return this.list.Contains(item);
        }

        public int IndexOf(Role item)
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

        void ICollection<Role>.CopyTo(Role[] array, int index)
        {
            this.list.CopyTo(array, index);
        }

        int ICollection<Role>.Count
        {
            get { return this.list.Count; }
        }

        bool ICollection<Role>.Remove(Role item)
        {
            return this.list.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.list.GetEnumerator();
        }

        IEnumerator<Role> IEnumerable<Role>.GetEnumerator()
        {
            return (IEnumerator<Role>)this.list.GetEnumerator();
        }
    }

    public static class RoleCollectionExtensions
    {
        public static bool ContainsRole(this RoleCollection roles, Role role)
        {
            foreach (Role r in roles)
            {
                if (r.ID == role.ID)
                    return true;
            }
            return false;
        }

        public static bool ContainsRole(this List<int> roles, int role)
        {
            return roles.Contains(role);
        }
    }
}
