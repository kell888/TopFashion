using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TopFashion
{
    public enum 性别 : int
    {
        男 = 0,
        女 = 1
    }

    [Serializable]
    public class Member
    {
        public int ID { get; set; }

        public string 姓名 { get; set; }

        public 性别 性别 { get; set; }

        public CardType 卡种 { get; set; }

        public string 卡号 { get; set; }

        public string 电话 { get; set; }

        public string 住址 { get; set; }

        public DateTime 开卡日 { get; set; }

        public DateTime 到期日 { get; set; }

        public DateTime 生日 { get; set; }

        public string 备注 { get; set; }

        public string MemberInfo
        {
            get
            {
                string cardType = "未知卡种";
                if (卡种 != null)
                    cardType = 卡种.卡种;
                return "姓名: " + 姓名 + " | 卡种：" + cardType + " | 性别：" + 性别.ToString() + " | 到期日：" + 到期日;
            }
        }

        public override string ToString()
        {
            return 姓名;
        }
    }

    [Serializable]
    public class MemberSimply
    {
        public int ID { get; set; }

        public string 姓名 { get; set; }

        public override string ToString()
        {
            return 姓名;
        }
    }
}
