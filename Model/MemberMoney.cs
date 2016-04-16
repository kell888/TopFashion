using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TopFashion
{
    [Serializable]
    public class MemberMoney
    {
        public int ID { get; set; }

        public string 会员姓名 { get; set; }

        public string 会员电话 { get; set; }

        public decimal 账户余额 { get; set; }

        public string 备注 { get; set; }

        public override string ToString()
        {
            return 会员姓名 + "(" + 账户余额 + ")";
        }
    }
}
