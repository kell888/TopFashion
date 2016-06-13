using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TopFashion
{
    [Serializable]
    public class DocObject
    {
        public int ID { get; set; }

        string name = "未命名文档";
        public string Name { get { return name; } set { if (!string.IsNullOrEmpty(value)) { name = value; } } }

        public FormObject Form { get; set; }

        public List<FormItem> DocItems { get; set; }

        public User Owner { get; set; }

        public string Remark { get; set; }

        public int Flag { get; set; }

        public DocObject()
        {
            DocItems = new List<FormItem>();
        }

        public string DocInfo
        {
            get
            {
                string author = " -- 作者：未知";
                if (Owner != null)
                {
                    author = " -- 作者：" + Owner.Username;
                }
                return Name + author + "(阅读次数：" + Flag + ")";
            }
        }

        public override string ToString()
        {
            return DocInfo;
        }
    }
}
