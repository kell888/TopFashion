using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TopFashion
{
    public class FinanceDetail
    {
        public int ID { get; set; }

        public string 项目 { get; set; }

        public decimal 金额 { get; set; }

        public bool 是否进账 { get; set; }

        public Staff 责任人 { get; set; }

        public string 备注 { get; set; }

        public DateTime 提交时间 { get; set; }

        public int Flag { get; set; }

        public string DetailInfo
        {
            get
            {
                return "项目：" + 项目 + " | 金额：" + 金额 + " | 是否进账：" + (是否进账 ? "是" : "否") + " | 责任人：" + 责任人 + 责任人 + " | 是否已经报销：" + (Flag == 1 ? "是" : "否");
            }
        }

        public override string ToString()
        {
            return 项目 + "[" + (是否进账 ? "+" : "-") + 金额 + "]";
        }
    }
}
