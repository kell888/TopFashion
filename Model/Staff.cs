using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TopFashion
{
    [Serializable]
    public class Staff
    {
        public int ID { get; set; }

        public DateTime 生日 { get; set; }

        public Department Depart { get; set; }

        public StaffCondition Condition { get; set; }

        public 性别 性别 { get; set; }

        public string 姓名 { get; set; }

        public string 电话 { get; set; }

        public string 宿舍 { get; set; }

        public string 备注 { get; set; }

        public int 钥匙数 { get; set; }

        public int 工衣数 { get; set; }

        public int 工牌数 { get; set; }

        public bool 是否全部回收 { get; set; }

        /// <summary>
        /// 当前员工是否属于指定的部门
        /// </summary>
        /// <param name="depart"></param>
        /// <returns></returns>
        public bool BelongToDepart(Department depart, bool recursion = true)
        {
            if (this.Depart.ID == depart.ID)
            {
                return true;
            }
            else
            {
                return depart.IsMyChildren(this.Depart, recursion);
            }
        }

        public override string ToString()
        {
            return 姓名;
        }
    }
}
