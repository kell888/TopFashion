using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TopFashion
{
    [Serializable]
    public class FormObject
    {
        public int ID { get; set; }

        string name = "未命名表单";
        public string FormName { get { return name; } set { if (!string.IsNullOrEmpty(value)) { name = value; } } }

        public FormType FormType { get; set; }

        public List<FormItem> FormItems { get; set; }

        public User Owner { get; set; }

        public string Remark { get; set; }

        public string FormInfo
        {
            get
            {
                string items = "无字段";
                if (FormItems != null && FormItems.Count > 0)
                {
                    items = "字段数：" + FormItems.Count;
                }
                return FormName + "(表单类型：" + FormType.TypeName + "，" + items + ")";
            }
        }

        public override string ToString()
        {
            return FormName;
        }
    }
}
