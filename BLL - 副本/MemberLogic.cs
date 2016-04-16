using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace TopFashion
{
    public class MemberLogic
    {        
        SQLDBHelper sqlHelper;
        static MemberLogic instance;
        public static MemberLogic GetInstance()
        {
            if (instance == null)
                instance = new MemberLogic();

            return instance;
        }

        private MemberLogic()
        {
            sqlHelper = new SQLDBHelper();
        }

        public Member GetMember(int id)
        {
            string sql = "select * from TF_Member where ID=" + id;
            DataTable dt = sqlHelper.Query(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                Member element = new Member();
                element.ID = id;
                element.姓名 = dt.Rows[0]["姓名"].ToString();
                element.性别 = (性别)Enum.ToObject(typeof(性别), Convert.ToInt32(dt.Rows[0]["性别"]));
                element.卡种 = CardTypeLogic.GetInstance().GetCardType(Convert.ToInt32(dt.Rows[0]["卡种"]));
                element.卡号 = dt.Rows[0]["卡号"].ToString();
                element.到期日 = Convert.ToDateTime(dt.Rows[0]["到期日"]);
                element.生日 = Convert.ToDateTime(dt.Rows[0]["生日"]);
                element.电话 = dt.Rows[0]["电话"].ToString();
                element.住址 = dt.Rows[0]["住址"].ToString();
                element.备注 = dt.Rows[0]["备注"].ToString();
                return element;
            }
            return null;
        }

        public Member GetMemberByName(string name)
        {
            string sql = "select * from TF_Member where 姓名 like '%" + name + "%'";
            DataTable dt = sqlHelper.Query(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                Member element = new Member();
                element.ID = Convert.ToInt32(dt.Rows[0]["ID"]);
                element.姓名 = dt.Rows[0]["姓名"].ToString();
                element.性别 = (性别)Enum.ToObject(typeof(性别), Convert.ToInt32(dt.Rows[0]["性别"]));
                element.卡种 = CardTypeLogic.GetInstance().GetCardType(Convert.ToInt32(dt.Rows[0]["卡种"]));
                element.卡号 = dt.Rows[0]["卡号"].ToString();
                element.到期日 = Convert.ToDateTime(dt.Rows[0]["到期日"]);
                element.生日 = Convert.ToDateTime(dt.Rows[0]["生日"]);
                element.电话 = dt.Rows[0]["电话"].ToString();
                element.住址 = dt.Rows[0]["住址"].ToString();
                element.备注 = dt.Rows[0]["备注"].ToString();
                return element;
            }
            return null;
        }

        public Member GetMemberByDataRow(DataRow row)
        {
            Member element = null;
            if (row != null)
            {
                element = new Member();
                element.ID = Convert.ToInt32(row["ID"]);
                element.姓名 = row["姓名"].ToString();
                element.性别 = (性别)Enum.Parse(typeof(性别), row["性别"].ToString());
                element.卡种 = CardTypeLogic.GetInstance().GetCardTypeByName(row["卡种"].ToString());
                element.卡号 = row["卡号"].ToString();
                element.到期日 = Convert.ToDateTime(row["到期日"]);
                element.生日 = Convert.ToDateTime(row["生日"]);
                element.电话 = row["电话"].ToString();
                element.住址 = row["住址"].ToString();
                element.备注 = row["备注"].ToString();
            }
            return element;
        }

        public List<Member> GetAllMembers()
        {
            List<Member> elements = new List<Member>();
            string sql = "select * from TF_Member";
            DataTable dt = sqlHelper.Query(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Member element = new Member();
                    element.ID = Convert.ToInt32(dt.Rows[i]["ID"]);
                    element.姓名 = dt.Rows[i]["姓名"].ToString();
                    element.性别 = (性别)Enum.ToObject(typeof(性别), Convert.ToInt32(dt.Rows[i]["性别"]));
                    element.卡种 = CardTypeLogic.GetInstance().GetCardType(Convert.ToInt32(dt.Rows[i]["卡种"]));
                    element.卡号 = dt.Rows[i]["卡号"].ToString();
                    element.到期日 = Convert.ToDateTime(dt.Rows[i]["到期日"]);
                    element.生日 = Convert.ToDateTime(dt.Rows[i]["生日"]);
                    element.电话 = dt.Rows[i]["电话"].ToString();
                    element.住址 = dt.Rows[i]["住址"].ToString();
                    element.备注 = dt.Rows[i]["备注"].ToString();
                    elements.Add(element);
                }
            }
            return elements;
        }

        public List<Member> GetMemberList(string where)
        {
            List<Member> elements = new List<Member>();
            string w = "";
            if (!string.IsNullOrEmpty(where))
            {
                w = where.Trim().ToLower();
                if (!w.StartsWith("where "))
                    w = "where " + w;
            }
            string sql = "select * from TF_Member " + w + " order by 到期日 desc";
            DataTable dt = sqlHelper.Query(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Member element = new Member();
                    element.ID = Convert.ToInt32(dt.Rows[i]["ID"]);
                    element.姓名 = dt.Rows[i]["姓名"].ToString();
                    element.性别 = (性别)Enum.ToObject(typeof(性别), Convert.ToInt32(dt.Rows[i]["性别"]));
                    element.卡种 = CardTypeLogic.GetInstance().GetCardType(Convert.ToInt32(dt.Rows[i]["卡种"]));
                    element.卡号 = dt.Rows[i]["卡号"].ToString();
                    element.到期日 = Convert.ToDateTime(dt.Rows[i]["到期日"]);
                    element.生日 = Convert.ToDateTime(dt.Rows[i]["生日"]);
                    element.电话 = dt.Rows[i]["电话"].ToString();
                    element.住址 = dt.Rows[i]["住址"].ToString();
                    element.备注 = dt.Rows[i]["备注"].ToString();
                    elements.Add(element);
                }
            }
            return elements;
        }

        public Member GetMember(string name, string mobile)
        {
            string sql = "select * from TF_User where 姓名='" + name + "' and 电话='" + mobile + "'";
            DataTable dt = sqlHelper.Query(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                Member element = new Member();
                element.ID = Convert.ToInt32(dt.Rows[0]["ID"]);
                element.姓名 = dt.Rows[0]["姓名"].ToString();
                element.性别 = (性别)Enum.ToObject(typeof(性别), Convert.ToInt32(dt.Rows[0]["性别"]));
                element.卡种 = CardTypeLogic.GetInstance().GetCardType(Convert.ToInt32(dt.Rows[0]["卡种"]));
                element.卡号 = dt.Rows[0]["卡号"].ToString();
                element.到期日 = Convert.ToDateTime(dt.Rows[0]["到期日"]);
                element.生日 = Convert.ToDateTime(dt.Rows[0]["生日"]);
                element.电话 = dt.Rows[0]["电话"].ToString();
                element.住址 = dt.Rows[0]["住址"].ToString();
                element.备注 = dt.Rows[0]["备注"].ToString();
                return element;
            }
            return null;
        }

        public int AddMember(Member element)
        {
            int cardType = 0;
            if (element.卡种 != null)
                cardType = element.卡种.ID;
            string sql = "insert into TF_Member (卡种, 姓名, 性别, 到期日, 卡号, 生日, 电话, 住址, 备注) values (" + cardType + ", '" + element.姓名 + "', " + (int)element.性别 + ", '" + element.到期日.ToString("yyyy-MM-dd") + "', '" + element.卡号 + "', '" + element.生日.ToString("yyyy-MM-dd") + "', '" + element.电话 + "', '" + element.住址 + "', '" + element.备注 + "'); select SCOPE_IDENTITY()";
            object obj = sqlHelper.ExecuteSqlReturn(sql);
            int R;
            if (obj != null && obj != DBNull.Value && int.TryParse(obj.ToString(), out R))
                return R;
            else
                return 0;
        }

        public bool UpdateMember(Member element)
        {
            string sql = "update TF_Member set 卡种=" + element.卡种.ID + ", 姓名='" + element.姓名 + "', 性别=" + (int)element.性别 + ", 到期日='" + element.到期日.ToString("yyyy-MM-dd") + "', 卡号='" + element.卡号 + "', 生日='" + element.生日.ToString("yyyy-MM-dd") + "', 电话='" + element.电话 + "', 住址='" + element.住址 + "', 备注='" + element.备注 + "' where ID=" + element.ID;
            int r = sqlHelper.ExecuteSql(sql);
            return r > 0;
        }

        public bool DeleteMember(Member element)
        {
            string sql = "delete from TF_Member where ID=" + element.ID;
            int r = sqlHelper.ExecuteSql(sql);
            return r > 0;
        }
        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool UpgradeList(List<Member> list)
        {
            int errCount = 0;
            foreach (Member element in list)
            {
                string sqlStr = "if exists (select 1 from TF_Member where ID=" + element.ID + ") update TF_Member set 卡种=" + element.卡种.ID + ", 姓名='" + element.姓名 + "', 性别=" + (int)element.性别 + ", 到期日='" + element.到期日.ToString("yyyy-MM-dd") + "', 卡号='" + element.卡号 + "', 生日='" + element.生日.ToString("yyyy-MM-dd") + "', 电话='" + element.电话 + "', 住址='" + element.住址 + "', 备注='" + element.备注 + "' where ID=" + element.ID + " else insert into TF_Member (卡种, 姓名, 性别, 到期日, 卡号, 生日, 电话, 住址, 备注) values (" + element.卡种.ID + ", '" + element.姓名 + "', " + (int)element.性别 + ", '" + element.到期日.ToString("yyyy-MM-dd") + "', '" + element.卡号 + "', '" + element.生日.ToString("yyyy-MM-dd") + "', '" + element.电话 + "', '" + element.住址 + "', '" + element.备注 + "')";
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
        public bool ExistsName(string name)
        {
            return sqlHelper.Exists("select 1 from TF_Member where 姓名='" + name + "'");
        }

        /// <summary>
        /// 是否存在出了自己以外的同名
        /// </summary>
        /// <param name="name"></param>
        /// <param name="myId"></param>
        /// <returns></returns>
        public bool ExistsNameOther(string name, int myId)
        {
            return sqlHelper.Exists("select 1 from TF_Member where ID!=" + myId + " and 姓名='" + name + "'");
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
                return sqlHelper.Exists("select 1 from TF_Member " + w);
            }
            return false;
        }

        public DataTable GetMembers(string where)
        {
            DataTable dt = null;
            string w = "";
            if (!string.IsNullOrEmpty(where))
            {
                w = where.Trim().ToLower();
                if (!w.StartsWith("where "))
                    w = "where " + w;
            }
            string sql = "select * from TF_View_Member " + w + " order by 到期日 desc";
            dt = sqlHelper.Query(sql);
            return dt;
        }

        public bool ClearMembers()
        {
            string sql = "delete from TF_Member";
            int r = sqlHelper.ExecuteSql(sql);
            return r > 0;
        }
    }
}
