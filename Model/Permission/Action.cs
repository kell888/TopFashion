using System;
using System.Collections.Generic;
using System.Text;

namespace TopFashion
{
    /// <summary>
    /// 操作类
    /// </summary>
    [Serializable]
    public class Action : IArchitecture
    {
        int id;

        /// <summary>
        /// 操作ID
        /// </summary>
        public int ID
        {
            get { return id; }
            set { id = value; }
        }
        string name = "未命名操作";

        /// <summary>
        /// 操作的名称
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
        string formName;

        /// <summary>
        /// 窗体名
        /// </summary>
        public string FormName
        {
            get { return formName; }
            set { formName = value; }
        }
        string controlName;

        /// <summary>
        /// 控件名
        /// </summary>
        public string ControlName
        {
            get { return controlName; }
            set { controlName = value; }
        }

        string remark;

        /// <summary>
        /// 操作的描述
        /// </summary>
        /// </summary>
        public string Remark
        {
            get { return remark; }
            set { remark = value; }
        }

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public Action()
        {
        }

        /// <summary>
        /// 由指定的名称构造操作对象
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        public Action(int id, string name)
        {
            this.id = id;
            if (name != null && name.Trim() != "")
                this.name = name;
        }

        /// <summary>
        /// 操作名称
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.Name;
        }
    }
}
