using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TopFashion
{
    [Serializable]
    public class Income
    {
        public int ID { get; set; }

        public int PID { get; set; }

        public bool IsProduct { get; set; }

        public bool IsIncome { get; set; }

        public decimal 数量 { get; set; }

        public decimal 实价 { get; set; }

        public string 经手人 { get; set; }

        public string 备注 { get; set; }

        public DateTime 时间 { get; set; }
    }
}
