using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TopFashion
{
    [Serializable]
    public class Dairy
    {
        public int ID { get; set; }

        public decimal Pos机会籍 { get; set; }

        public decimal Pos机私教 { get; set; }

        public decimal 现金会籍 { get; set; }

        public decimal 现金私教 { get; set; }

        public decimal 总金额 { get; set; }

        public decimal 存水费 { get; set; }

        public decimal 水吧余 { get; set; }

        public Staff 经手人 { get; set; }

        public DateTime 日期 { get; set; }

        public string 备注 { get; set; }

        public override string ToString()
        {
            return 日期.ToString("yyyy-MM-dd");
        }
    }
}
