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
            roles = new List<int>();
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
            roles = new List<int>();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="roles"></param>
        public UserGroup(int id, string name, List<int> roles)
        {
            this.id = id;
            if (name != null && name.Trim() != "")
                this.name = name;
            if (roles == null)
                this.roles = new List<int>();
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
        List<int> roles;

        /// <summary>
        /// 用户组拥有的角色集合
        /// </summary>
        public List<int> Roles
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

        //private ArrayList list = new ArrayList();

        ///// <summary>
        ///// 索引器
        ///// </summary>
        ///// <param name="index"></param>
        ///// <returns></returns>
        //public User this[int index]
        //{
        //    get
        //    {
        //        return (User)this.list[index];
        //    }
        //    set
        //    {
        //        this.list[index] = value;
        //    }
        //}

        ///// <summary>
        ///// 向集合中添加元素
        ///// </summary>
        ///// <param name="item"></param>
        ///// <returns></returns>
        //public int Add(User item)
        //{
        //    return this.list.Add(item);
        //}

        ///// <summary>
        ///// 从集合中移出指定元素
        ///// </summary>
        ///// <param name="item"></param>
        //public void Remove(User item)
        //{
        //    this.list.Remove(item);
        //}

        ///// <summary>
        ///// 从集合中移出指定索引的元素
        ///// </summary>
        ///// <param name="index"></param>
        //public void RemoveAt(int index)
        //{
        //    this.list.RemoveAt(index);
        //}

        ///// <summary>
        ///// 取得集合元素个数
        ///// </summary>
        //public int Count
        //{
        //    get
        //    {
        //        return this.list.Count;
        //    }
        //}

        //#region ICollection 成员

        //void ICollection.CopyTo(Array array, int index)
        //{
        //    this.list.CopyTo(array, index);
        //}

        //int ICollection.Count
        //{
        //    get { return this.list.Count; }
        //}

        //bool ICollection.IsSynchronized
        //{
        //    get { return this.list.IsSynchronized; }
        //}

        //object ICollection.SyncRoot
        //{
        //    get { return this.list.SyncRoot; }
        //}

        //#endregion

        //#region IEnumerable 成员

        //IEnumerator IEnumerable.GetEnumerator()
        //{
        //    return this.list.GetEnumerator();
        //}

        //#endregion
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
