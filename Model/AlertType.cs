using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TopFashion
{
    [Serializable]
    public class AlertType
    {
        public int ID { get; set; }

        public string 方式 { get; set; }

        public string 备注 { get; set; }

        public bool Flag { get; set; }

        public override string ToString()
        {
            return this.方式;
        }
    }

    public enum 提醒方式 : int
    {
        系统提示 = 0,
        员工短信 = 1,
        会员短信 = 2
    }
}
