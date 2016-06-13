using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TopFashion
{
    [Serializable]
    public class Inventory
    {
        public int ID { get; set; }

        public int PID { get; set; }

        public bool IsProduct { get; set; }

        public bool IsIncome { get; set; }

        public decimal 数量 { get; set; }

        public DateTime 更新时间 { get; set; }

        public string 备注 { get; set; }
    }
}
