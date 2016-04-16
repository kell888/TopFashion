using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TopFashion
{
    public class Finance
    {
        public int ID { get; set; }

        public DateTime 日期 { get; set; }

        public string 项目 { get; set; }

        public decimal 金额 { get; set; }

        public bool 是否进账 { get; set; }

        public decimal 余款 { get; set; }

        public string 经手人 { get; set; }

        public string 接收人 { get; set; }

        public string Detail { get; set; }

        public override string ToString()
        {
            string detail = "[无明细]";
            if (!string.IsNullOrEmpty(Detail))
                detail = "[有明细]";
            return 项目 + "[" + (是否进账 ? "+" : "-") + 金额 + "] " + detail;
        }
    }
}
