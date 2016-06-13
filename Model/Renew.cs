using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TopFashion
{
    [Serializable]
    public class Renew
    {
        public int ID { get; set; }

        public Member Member { get; set; }

        public CardType 卡种 { get; set; }

        public string 卡号 { get; set; }

        public DateTime 续卡时间 { get; set; }

        public string 备注 { get; set; }

        public Staff 经手人 { get; set; }

        public override string ToString()
        {
            return Member.姓名 + "(" + 卡种 + ")";
        }
    }
}
