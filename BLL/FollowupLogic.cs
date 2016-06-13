using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace TopFashion
{
    public class FollowupLogic
    {        
        SQLDBHelper sqlHelper;
        static FollowupLogic instance;
        public static FollowupLogic GetInstance()
        {
            if (instance == null)
                instance = new FollowupLogic();

            return instance;
        }

        private FollowupLogic()
        {
            sqlHelper = new SQLDBHelper();
        }

        public Followup GetFollowup(int id)
        {
            string sql = "select * from TF_Followup where ID=" + id;
            DataTable dt = sqlHelper.Query(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                Followup element = new Followup();
                element.ID = id;
                element.Member = MemberLogic.GetInstance().GetMember(Convert.ToInt32(dt.Rows[0]["MemberID"]));
                element.回访方式 = FollowupTypeLogic.GetInstance().GetFollowupType(Convert.ToInt32(dt.Rows[0]["跟进方式"]));
                element.跟进结果 = FollowupResultLogic.GetInstance().GetFollowupResult(Convert.ToInt32(dt.Rows[0]["跟进结果"]));
                element.跟进时间 = Convert.ToDateTime(dt.Rows[0]["跟进时间"]);
                element.跟进人 = StaffLogic.GetInstance().GetStaff(Convert.ToInt32(dt.Rows[0]["跟进人"]));
                element.备注 = dt.Rows[0]["备注"].ToString();
                return element;
            }
            return null;
        }

        public List<Followup> GetAllFollowups()
        {
            List<Followup> elements = new List<Followup>();
            string sql = "select * from TF_Followup";
            DataTable dt = sqlHelper.Query(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Followup element = new Followup();
                    element.ID = Convert.ToInt32(dt.Rows[i]["ID"]);
                    element.Member = MemberLogic.GetInstance().GetMember(Convert.ToInt32(dt.Rows[i]["MemberID"]));
                    element.回访方式 = FollowupTypeLogic.GetInstance().GetFollowupType(Convert.ToInt32(dt.Rows[i]["跟进方式"]));
                    element.跟进结果 = FollowupResultLogic.GetInstance().GetFollowupResult(Convert.ToInt32(dt.Rows[i]["跟进结果"]));
                    element.跟进时间 = Convert.ToDateTime(dt.Rows[i]["跟进时间"]);
                    element.跟进人 = StaffLogic.GetInstance().GetStaff(Convert.ToInt32(dt.Rows[i]["跟进人"]));
                    element.备注 = dt.Rows[i]["备注"].ToString();
                    elements.Add(element);
                }
            }
            return elements;
        }

        public int AddFollowup(Followup element)
        {
            string sql = "insert into TF_Followup (MemberID, 跟进方式, 跟进结果, 跟进时间, 备注, 跟进人) values (" + element.Member.ID + ", " + element.回访方式.ID + ", " + element.跟进结果.ID + ", '" + element.跟进时间 + "', '" + element.备注 + "', " + element.跟进人.ID + "); select SCOPE_IDENTITY()";
            object obj = sqlHelper.ExecuteSqlReturn(sql);
            int R;
            if (obj != null && obj != DBNull.Value && int.TryParse(obj.ToString(), out R))
                return R;
            else
                return 0;
        }

        public bool UpdateFollowup(Followup element)
        {
            string sql = "update TF_Followup set MemberID=" + element.Member.ID + ", 跟进方式=" + element.回访方式.ID + ", 跟进结果=" + element.跟进结果.ID + ", 跟进时间='" + element.跟进时间 + "', 备注='" + element.备注 + "', 跟进人=" + element.跟进人.ID + " where ID=" + element.ID;
            int r = sqlHelper.ExecuteSql(sql);
            return r > 0;
        }

        public bool DeleteFollowup(Followup element)
        {
            string sql = "delete from TF_Followup where ID=" + element.ID;
            int r = sqlHelper.ExecuteSql(sql);
            return r > 0;
        }
        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool UpgradeList(List<Followup> list)
        {
            int errCount = 0;
            foreach (Followup element in list)
            {
                string sqlStr = "if exists (select 1 from TF_Followup where ID=" + element.ID + ") update TF_Followup set MemberID=" + element.Member.ID + ", 跟进方式=" + element.回访方式.ID + ", 跟进结果=" + element.跟进结果.ID + ", 跟进时间='" + element.跟进时间 + "', 备注='" + element.备注 + "', 跟进人=" + element.跟进人.ID + " where ID=" + element.ID + " else insert into TF_Followup (MemberID, 跟进方式, 跟进结果, 跟进时间, 备注, 跟进人) values (" + element.Member.ID + ", " + element.回访方式.ID + ", " + element.跟进结果.ID + ", '" + element.跟进时间 + "', '" + element.备注 + "', " + element.跟进人.ID + ")";
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
                return sqlHelper.Exists("select 1 from TF_Followup " + w);
            }
            return false;
        }

        public DataTable GetFollowups(string where)
        {
            DataTable dt = null;
            string w = "";
            if (!string.IsNullOrEmpty(where))
            {
                w = where.Trim().ToLower();
                if (!w.StartsWith("where "))
                    w = "where " + w;
            }
            string sql = "select * from TF_View_Followup " + w + " order by 跟进时间 desc";
            dt = sqlHelper.Query(sql);
            return dt;
        }
    }
}
