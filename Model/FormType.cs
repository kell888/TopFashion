using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TopFashion
{
    [Serializable]
    public class FormType
    {
        public int ID { get; set; }

        public string TypeName { get; set; }

        public string Remark { get; set; }

        public int Flag { get; set; }

        public override string ToString()
        {
            return this.TypeName;
        }
    }
}
