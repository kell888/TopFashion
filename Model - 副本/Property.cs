using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TopFashion
{
    [Serializable]
    public class Property
    {
        public int ID { get; set; }

        public string 单位 { get; set; }

        public string 名称 { get; set; }

        public string 用途 { get; set; }

        public decimal 价格 { get; set; }

        public string 备注 { get; set; }

        public override string ToString()
        {
            return 名称;
        }
    }
}
