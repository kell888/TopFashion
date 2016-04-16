using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace TopFashion
{
    public class MemberMoneyLogic
    {        
        SQLDBHelper sqlHelper;
        static MemberMoneyLogic instance;
        public static MemberMoneyLogic GetInstance()
        {
            if (instance == null)
                instance = new MemberMoneyLogic();

            return instance;
        }

        private MemberMoneyLogic()
        {
            sqlHelper = new SQLDBHelper();
        }

        public MemberMoney GetMemberMoney(int id)
        {
            string sql = "select * from TF_MemberMoney where ID=" + id;
            DataTable dt = sqlHelper.Query(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                MemberMoney element = new MemberMoney();
                element.ID = id;
                element.会员姓名 = dt.Rows[0]["会员姓名"].ToString();
                element.会员电话 = dt.Rows[0]["会员电话"].ToString();
                element.账户余额 = Convert.ToDecimal(dt.Rows[0]["账户余额"]);
                element.备注 = dt.Rows[0]["备注"].ToString();
                return element;
            }
            return null;
        }

        public List<MemberMoney> GetAllMemberMoneys()
        {
            List<MemberMoney> elements = new List<MemberMoney>();
            string sql = "select * from TF_MemberMoney";
            DataTable dt = sqlHelper.Query(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    MemberMoney element = new MemberMoney();
                    element.ID = Convert.ToInt32(dt.Rows[i]["ID"]);
                    element.会员姓名 = dt.Rows[i]["会员姓名"].ToString();
                    element.会员电话 = dt.Rows[i]["会员电话"].ToString();
                    element.账户余额 = Convert.ToDecimal(dt.Rows[i]["账户余额"]);
                    element.备注 = dt.Rows[i]["备注"].ToString();
                    elements.Add(element);
                }
            }
            return elements;
        }

        public List<MemberMoney> GetMemberMoneysByName(string name)
        {
            List<MemberMoney> elements = new List<MemberMoney>();
            string sql = "select * from TF_MemberMoney where 会员姓名 like '%" + name.Trim() + "%'";
            DataTable dt = sqlHelper.Query(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    MemberMoney element = new MemberMoney();
                    element.ID = Convert.ToInt32(dt.Rows[i]["ID"]);
                    element.会员姓名 = dt.Rows[i]["会员姓名"].ToString();
                    element.会员电话 = dt.Rows[i]["会员电话"].ToString();
                    element.账户余额 = Convert.ToDecimal(dt.Rows[i]["账户余额"]);
                    element.备注 = dt.Rows[i]["备注"].ToString();
                    elements.Add(element);
                }
            }
            return elements;
        }

        public List<MemberMoney> GetMemberMoneysByMobile(string mobile)
        {
            List<MemberMoney> elements = new List<MemberMoney>();
            string sql = "select * from TF_MemberMoney where 会员电话 like '%" + mobile.Trim() + "%'";
            DataTable dt = sqlHelper.Query(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    MemberMoney element = new MemberMoney();
                    element.ID = Convert.ToInt32(dt.Rows[i]["ID"]);
                    element.会员姓名 = dt.Rows[i]["会员姓名"].ToString();
                    element.会员电话 = dt.Rows[i]["会员电话"].ToString();
                    element.账户余额 = Convert.ToDecimal(dt.Rows[i]["账户余额"]);
                    element.备注 = dt.Rows[i]["备注"].ToString();
                    elements.Add(element);
                }
            }
            return elements;
        }

        public DataTable GetMemberMoneysBy(string name, string mobile, decimal lessThan = 0)
        {
            List<MemberMoney> elements = new List<MemberMoney>();
            string sql = "select * from TF_MemberMoney where (1=1)";
            if (!string.IsNullOrEmpty(name))
            {
                sql += " and 会员姓名 like '%" + name.Trim() + "%'";
            }
            if (!string.IsNullOrEmpty(mobile))
            {
                sql += " and 会员电话 like '%" + mobile.Trim() + "%'";
            }
            if (lessThan > 0)
            {
                sql += " and 账户余额<" + lessThan;
            }
            DataTable dt = sqlHelper.Query(sql);

            return dt;
        }

        public List<MemberMoney> GetMemberMoneys(string name, string mobile, decimal lessThan = 0)
        {
            List<MemberMoney> elements = new List<MemberMoney>();
            string sql = "select * from TF_MemberMoney where (1=1)";
            if (!string.IsNullOrEmpty(name))
            {
                sql += " and 会员姓名 like '%" + name.Trim() + "%'";
            }
            if (!string.IsNullOrEmpty(mobile))
            {
                sql += " and 会员电话 like '%" + mobile.Trim() + "%'";
            }
            if (lessThan > 0)
            {
                sql += " and 账户余额<" + lessThan;
            }
            DataTable dt = sqlHelper.Query(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    MemberMoney element = new MemberMoney();
                    element.ID = Convert.ToInt32(dt.Rows[i]["ID"]);
                    element.会员姓名 = dt.Rows[i]["会员姓名"].ToString();
                    element.会员电话 = dt.Rows[i]["会员电话"].ToString();
                    element.账户余额 = Convert.ToDecimal(dt.Rows[i]["账户余额"]);
                    element.备注 = dt.Rows[i]["备注"].ToString();
                    elements.Add(element);
                }
            }
            return elements;
        }

        public MemberMoney GetMemberMoney(string name, string mobile)
        {
            MemberMoney element = null;
            string sql = "select * from TF_MemberMoney where 会员姓名='" + name + "' and 会员电话='" + mobile + "'";
            DataTable dt = sqlHelper.Query(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                element = new MemberMoney();
                element.ID = Convert.ToInt32(dt.Rows[0]["ID"]);
                element.会员姓名 = dt.Rows[0]["会员姓名"].ToString();
                element.会员电话 = dt.Rows[0]["会员电话"].ToString();
                element.账户余额 = Convert.ToDecimal(dt.Rows[0]["账户余额"]);
                element.备注 = dt.Rows[0]["备注"].ToString();
            }
            return element;
        }

        public MemberMoney GetMemberMoney(Member member)
        {
            return GetMemberMoney(member.姓名, member.电话);
        }

        public Member GetMember(MemberMoney mm)
        {
            return MemberLogic.GetInstance().GetMember(mm.会员姓名, mm.会员电话);
        }

        public int AddMemberMoney(MemberMoney element)
        {
            string sql = "insert into TF_MemberMoney (会员姓名, 会员电话, 账户余额, 备注) values ('" + element.会员姓名 + "', '" + element.会员电话 + "', " + element.账户余额 + ", '" + element.备注 + "'); select SCOPE_IDENTITY()";
            object obj = sqlHelper.ExecuteSqlReturn(sql);
            int R;
            if (obj != null && obj != DBNull.Value && int.TryParse(obj.ToString(), out R))
                return R;
            else
                return 0;
        }

        public bool UpdateMemberMoney(MemberMoney element)
        {
            string sql = "update TF_MemberMoney set 会员姓名='" + element.会员姓名 + "', 会员电话='" + element.会员电话 + "', 账户余额=" + element.账户余额 + ", 备注='" + element.备注 + "' where ID=" + element.ID;
            int r = sqlHelper.ExecuteSql(sql);
            return r > 0;
        }

        public bool DeleteMemberMoney(MemberMoney element)
        {
            string sql = "delete from TF_MemberMoney where ID=" + element.ID;
            int r = sqlHelper.ExecuteSql(sql);
            return r > 0;
        }
        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool UpgradeList(List<MemberMoney> list)
        {
            int errCount = 0;
            foreach (MemberMoney element in list)
            {
                string sqlStr = "if exists (select 1 from TF_MemberMoney where ID=" + element.ID + ") update TF_MemberMoney set 会员姓名='" + element.会员姓名 + "', 会员电话='" + element.会员电话 + "', 账户余额=" + element.账户余额 + ", 备注='" + element.备注 + "' where ID=" + element.ID + " else insert into TF_MemberMoney (会员姓名, 会员电话, 账户余额, 备注) values ('" + element.会员姓名 + "', '" + element.会员电话 + "', " + element.账户余额 + ", '" + element.备注 + "')";
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
        /// 是否存在同名
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool ExistsName(string name, string mobile)
        {
            return sqlHelper.Exists("select 1 from TF_MemberMoney where 会员姓名='" + name + "' and 会员电话='" + mobile + "'");
        }

        /// <summary>
        /// 是否存在出了自己以外的同名
        /// </summary>
        /// <param name="name"></param>
        /// <param name="mobile"></param>
        /// <param name="myId"></param>
        /// <returns></returns>
        public bool ExistsNameOther(string name, string mobile, int myId)
        {
            return sqlHelper.Exists("select 1 from TF_MemberMoney where ID!=" + myId + " and 会员姓名='" + name + "' and 会员电话='" + mobile + "'");
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
                return sqlHelper.Exists("select 1 from TF_MemberMoney " + w);
            }
            return false;
        }

        public DataTable GetMemberMoneysBy(string where)
        {
            string w = "";
            if (!string.IsNullOrEmpty(where))
            {
                w = where.Trim().ToLower();
                if (!w.StartsWith("where "))
                    w = "where " + w;
            }
            string sql = "select 会员姓名, 会员电话, 账户余额, 备注 from TF_MemberMoney " + w + " order by ID desc";
            DataTable dt = sqlHelper.Query(sql);
            return dt;
        }
    }
}
