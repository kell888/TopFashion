using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TopFashion
{
    [Serializable]
    public class StaffCondition
    {
        public int ID { get; set; }

        public string 状态 { get; set; }

        public bool 是否在职 { get; set; }

        public string 备注 { get; set; }

        public override string ToString()
        {
            return 状态;
        }
    }
}
