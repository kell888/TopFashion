using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace TopFashion
{
    public class RenewLogic
    {        
        SQLDBHelper sqlHelper;
        static RenewLogic instance;
        public static RenewLogic GetInstance()
        {
            if (instance == null)
                instance = new RenewLogic();

            return instance;
        }

        private RenewLogic()
        {
            sqlHelper = new SQLDBHelper();
        }

        public Renew GetRenew(int id)
        {
            string sql = "select * from TF_Renew where ID=" + id;
            DataTable dt = sqlHelper.Query(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                Renew element = new Renew();
                element.ID = id;
                element.Member = MemberLogic.GetInstance().GetMember(Convert.ToInt32(dt.Rows[0]["MemberID"]));
                element.卡种 = CardTypeLogic.GetInstance().GetCardType(Convert.ToInt32(dt.Rows[0]["CardType"]));
                element.卡号 = dt.Rows[0]["卡号"].ToString();
                element.续卡时间 = Convert.ToDateTime(dt.Rows[0]["续卡时间"]);
                element.经手人 = StaffLogic.GetInstance().GetStaff(Convert.ToInt32(dt.Rows[0]["经手人"]));
                element.备注 = dt.Rows[0]["备注"].ToString();
                return element;
            }
            return null;
        }

        public List<Renew> GetAllRenews()
        {
            List<Renew> elements = new List<Renew>();
            string sql = "select * from TF_Renew";
            DataTable dt = sqlHelper.Query(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Renew element = new Renew();
                    element.ID = Convert.ToInt32(dt.Rows[i]["ID"]);
                    element.Member = MemberLogic.GetInstance().GetMember(Convert.ToInt32(dt.Rows[i]["MemberID"]));
                    element.卡种 = CardTypeLogic.GetInstance().GetCardType(Convert.ToInt32(dt.Rows[i]["卡种"]));
                    element.卡号 = dt.Rows[i]["卡号"].ToString();
                    element.续卡时间 = Convert.ToDateTime(dt.Rows[i]["续卡时间"]);
                    element.经手人 = StaffLogic.GetInstance().GetStaff(Convert.ToInt32(dt.Rows[i]["经手人"]));
                    element.备注 = dt.Rows[i]["备注"].ToString();
                    elements.Add(element);
                }
            }
            return elements;
        }

        public int AddRenew(Renew element)
        {
            string sql = "insert into TF_Renew (MemberID, 卡种, 卡号, 续卡时间, 备注, 经手人) values (" + element.Member.ID + ", " + element.卡种.ID + ", '" + element.卡号 + "', '" + element.续卡时间 + "', '" + element.备注 + "', " + element.经手人.ID + "); select SCOPE_IDENTITY()";
            object obj = sqlHelper.ExecuteSqlReturn(sql);
            int R;
            if (obj != null && obj != DBNull.Value && int.TryParse(obj.ToString(), out R))
                return R;
            else
                return 0;
        }

        public bool UpdateRenew(Renew element)
        {
            string sql = "update TF_Renew set MemberID=" + element.Member.ID + ", 卡种=" + element.卡种.ID + ", 卡号='" + element.卡号 + "', 续卡时间='" + element.续卡时间 + "', 备注='" + element.备注 + "', 经手人=" + element.经手人.ID + " where ID=" + element.ID;
            int r = sqlHelper.ExecuteSql(sql);
            return r > 0;
        }

        public bool DeleteRenew(Renew element)
        {
            string sql = "delete from TF_Renew where ID=" + element.ID;
            int r = sqlHelper.ExecuteSql(sql);
            return r > 0;
        }
        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool UpgradeList(List<Renew> list)
        {
            int errCount = 0;
            foreach (Renew element in list)
            {
                string sqlStr = "if exists (select 1 from TF_Renew where ID=" + element.ID + ") update TF_Renew set MemberID=" + element.Member.ID + ", 卡种=" + element.卡种.ID + ", 卡号='" + element.卡号 + "', 续卡时间='" + element.续卡时间 + "', 备注='" + element.备注 + "', 经手人=" + element.经手人.ID + " where ID=" + element.ID + " else insert into TF_Renew (MemberID, 卡种, 卡号, 续卡时间, 备注, 经手人) values (" + element.Member.ID + ", " + element.卡种.ID + ", '" + element.卡号 + "', '" + element.续卡时间 + "', '" + element.备注 + "', " + element.经手人.ID + ")";
                try
                {
                    sqlHelper.ExecuteSql(sqlStr);
                }
                catch (Exception)
                {
                    errCount++;
                }
            }
            return errCount == 0;
        }

        /// <summary>
        /// 是否存在指定条件的记录
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool ExistsWhere(string where)
        {
            if (!string.IsNullOrEmpty(where))
            {
                string w = where.Trim().ToLower();
                if (!w.StartsWith("where "))
                    w = "where " + w;
                return sqlHelper.Exists("select 1 from TF_Renew " + w);
            }
            return false;
        }

        public DataTable GetRenews(string where)
        {
            DataTable dt = null;
            string w = "";
            if (!string.IsNullOrEmpty(where))
            {
                w = where.Trim().ToLower();
                if (!w.StartsWith("where "))
                    w = "where " + w;
            }
            string sql = "select * from TF_View_Renew " + w + " order by 续卡时间 desc";
            dt = sqlHelper.Query(sql);
            return dt;
        }
    }
}
