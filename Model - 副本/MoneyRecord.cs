using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TopFashion
{
    [Serializable]
    public class MoneyRecord
    {
        public int ID { get; set; }

        public MemberMoney 会员账户 { get; set; }

        public decimal 发生金额 { get; set; }

        public bool 是否充值 { get; set; }

        public string 操作人 { get; set; }

        public DateTime 发生时间 { get; set; }

        public override string ToString()
        {
            string cz = 是否充值 ? "充值" : "消费";
            return 会员账户 + "(" + 操作人 + ":" + cz + "于" + 发生时间 + ")";
        }
    }
}
