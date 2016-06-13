using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TopFashion
{
    [Serializable]
    public class FollowupResult
    {
        public int ID { get; set; }

        public string 结果 { get; set; }

        public string 备注 { get; set; }

        public bool Flag { get; set; }

        public override string ToString()
        {
            return this.结果;
        }
    }
}
