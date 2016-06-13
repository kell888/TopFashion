using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TopFashion
{
    public class Workplan
    {
        public int ID { get; set; }

        public Staff 销售 { get; set; }

        public DateTime 日期 { get; set; }

        public int 带人数 { get; set; }

        public int 号码数 { get; set; }

        public int 成单数 { get; set; }

        public int 回访数 { get; set; }

        public string 备注 { get; set; }

        public override string ToString()
        {
            return 销售 + "[" + 日期 + "]";
        }
    }
}
