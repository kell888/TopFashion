using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TopFashion
{
    [Serializable]
    public class PersonalTrain
    {
        public int ID { get; set; }

        public Member Member { get; set; }

        public string 私教项目 { get; set; }

        public DateTime 开始日期 { get; set; }

        public DateTime 结束日期 { get; set; }

        public int 次数 { get; set; }

        public Staff 教练 { get; set; }

        public string 备注 { get; set; }

        public DateTime SaleTime { get; set; }

        public override string ToString()
        {
            return Member.姓名 + "(" + 私教项目 + ":" + 教练 + ")";
        }
    }
}
