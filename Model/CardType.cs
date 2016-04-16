using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TopFashion
{
    [Serializable]
    public class CardType
    {
        public int ID { get; set; }

        public string 卡种 { get; set; }

        public bool 是否电子芯片 { get; set; }

        public string 备注 { get; set; }

        public override string ToString()
        {
            return this.卡种;
        }
    }
}
