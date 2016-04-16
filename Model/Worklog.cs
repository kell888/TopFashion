using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TopFashion
{
    public class Worklog
    {
        public int ID { get; set; }

        public Staff 销售 { get; set; }

        public DateTime 日期 { get; set; }

        public string 客户 { get; set; }

        public string 电话 { get; set; }

        public bool 是否自访 { get; set; }

        public bool 是否老会员 { get; set; }

        public bool 是否电话拜访 { get; set; }

        public 性别 性别 { get; set; }

        public string 意向 { get; set; }

        public string 住址 { get; set; }

        public string 备注 { get; set; }

        public override string ToString()
        {
            return 销售 + "=>" + 客户 + "[" + 电话 + "]";
        }
    }
}
