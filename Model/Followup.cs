using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TopFashion
{
    public class Followup
    {
        public int ID { get; set; }

        public Member Member { get; set; }

        public FollowupType 回访方式 { get; set; }

        public Staff 跟进人 { get; set; }

        public DateTime 跟进时间 { get; set; }

        public string 备注 { get; set; }

        public FollowupResult 跟进结果 { get; set; }

        public override string ToString()
        {
            return 跟进人 + "=>" + Member.姓名;
        }
    }
}
