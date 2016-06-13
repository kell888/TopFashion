using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TopFashion
{
    [Serializable]
    public class Product
    {
        public int ID { get; set; }

        public string 单位 { get; set; }

        public ProductType 种类 { get; set; }

        public string 品名 { get; set; }

        public decimal 进价 { get; set; }

        public decimal 售价 { get; set; }

        public string 厂家 { get; set; }

        public string 姓名 { get; set; }

        public string 电话 { get; set; }

        public string 地址 { get; set; }

        public string 备注 { get; set; }

        public override string ToString()
        {
            return 品名;
        }
    }
}
