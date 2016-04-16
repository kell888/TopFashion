using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TopFashion
{
    [Serializable]
    public class Alert
    {
        public int ID { get; set; }

        public string 提醒项目 { get; set; }

        public DateTime 提醒时间 { get; set; }

        public 提醒方式 提醒方式 { get; set; }

        public string 提醒对象 { get; set; }

        public string 备注 { get; set; }

        public int Flag { get; set; }

        public override string ToString()
        {
            return 提醒项目;
        }
    }

    public enum 提醒方式 : int
    {
        系统提示 = 0,
        员工短信 = 1,
        会员短信 = 2,
        执行流程 = 3,
        审批流程 = 4
    }
}
