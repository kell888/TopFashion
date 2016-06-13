using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace TopFashion
{
    public class PersonalTrainLogic
    {        
        SQLDBHelper sqlHelper;
        static PersonalTrainLogic instance;
        public static PersonalTrainLogic GetInstance()
        {
            if (instance == null)
                instance = new PersonalTrainLogic();

            return instance;
        }

        private PersonalTrainLogic()
        {
            sqlHelper = new SQLDBHelper();
        }

        public PersonalTrain GetPersonalTrain(int id)
        {
            string sql = "select * from TF_PersonalTrain where ID=" + id;
            DataTable dt = sqlHelper.Query(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                PersonalTrain element = new PersonalTrain();
                element.ID = id;
                element.Member = MemberLogic.GetInstance().GetMember(Convert.ToInt32(dt.Rows[0]["MemberID"]));
                element.私教项目 = dt.Rows[0]["私教项目"].ToString();
                element.次数 = Convert.ToInt32(dt.Rows[0]["次数"]);
                element.开始日期 = Convert.ToDateTime(dt.Rows[0]["开始日期"]);
                element.结束日期 = Convert.ToDateTime(dt.Rows[0]["结束日期"]);
                element.教练 = StaffLogic.GetInstance().GetStaff(Convert.ToInt32(dt.Rows[0]["教练"]));
                element.备注 = dt.Rows[0]["备注"].ToString();
                element.SaleTime = Convert.ToDateTime(dt.Rows[0]["SaleTime"]);
                return element;
            }
            return null;
        }

        public List<PersonalTrain> GetAllPersonalTrains()
        {
            List<PersonalTrain> elements = new List<PersonalTrain>();
            string sql = "select * from TF_PersonalTrain";
            DataTable dt = sqlHelper.Query(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    PersonalTrain element = new PersonalTrain();
                    element.ID = Convert.ToInt32(dt.Rows[i]["ID"]);
                    element.Member = MemberLogic.GetInstance().GetMember(Convert.ToInt32(dt.Rows[i]["MemberID"]));
                    element.私教项目 = dt.Rows[i]["私教项目"].ToString();
                    element.次数 = Convert.ToInt32(dt.Rows[i]["次数"]);
                    element.开始日期 = Convert.ToDateTime(dt.Rows[i]["开始日期"]);
                    element.结束日期 = Convert.ToDateTime(dt.Rows[i]["结束日期"]);
                    element.教练 = StaffLogic.GetInstance().GetStaff(Convert.ToInt32(dt.Rows[i]["教练"]));
                    element.备注 = dt.Rows[i]["备注"].ToString();
                    element.SaleTime = Convert.ToDateTime(dt.Rows[i]["SaleTime"]);
                    elements.Add(element);
                }
            }
            return elements;
        }

        public int AddPersonalTrain(PersonalTrain element)
        {
            string sql = "insert into TF_PersonalTrain (MemberID, 私教项目, 次数, 开始日期, 结束日期, 教练, 备注) values (" + element.Member.ID + ", '" + element.私教项目 + "', " + element.次数 + ", '" + element.开始日期 + "', '" + element.结束日期 + "', " + element.教练.ID + ", '" + element.备注 + "'); select SCOPE_IDENTITY()";
            object obj = sqlHelper.ExecuteSqlReturn(sql);
            int R;
            if (obj != null && obj != DBNull.Value && int.TryParse(obj.ToString(), out R))
                return R;
            else
                return 0;
        }

        public bool UpdatePersonalTrain(PersonalTrain element)
        {
            string sql = "update TF_PersonalTrain set MemberID=" + element.Member.ID + ", 私教项目='" + element.私教项目 + "', 次数=" + element.次数 + ", 开始日期='" + element.开始日期 + "', 结束日期='" + element.结束日期 + "', 教练=" + element.教练.ID + ", 备注='" + element.备注 + "' where ID=" + element.ID;
            int r = sqlHelper.ExecuteSql(sql);
            return r > 0;
        }

        public bool DeletePersonalTrain(PersonalTrain element)
        {
            string sql = "delete from TF_PersonalTrain where ID=" + element.ID;
            int r = sqlHelper.ExecuteSql(sql);
            return r > 0;
        }
        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool UpgradeList(List<PersonalTrain> list)
        {
            int errCount = 0;
            foreach (PersonalTrain element in list)
            {
                string sqlStr = "if exists (select 1 from TF_PersonalTrain where ID=" + element.ID + ") update TF_PersonalTrain set MemberID=" + element.Member.ID + ", 私教项目='" + element.私教项目 + "', 次数=" + element.次数 + ", 开始日期='" + element.开始日期 + "', 结束日期='" + element.结束日期 + "', 教练=" + element.教练.ID + ", 备注='" + element.备注 + "' where ID=" + element.ID + " else insert into TF_PersonalTrain (MemberID, 私教项目, 次数, 开始日期, 结束日期, 教练, 备注) values (" + element.Member.ID + ", '" + element.私教项目 + "', " + element.次数 + ", '" + element.开始日期 + "', '" + element.结束日期 + "', " + element.教练.ID + ", '" + element.备注 + "')";
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
                return sqlHelper.Exists("select 1 from TF_PersonalTrain " + w);
            }
            return false;
        }

        public DataTable GetPersonalTrains(string where)
        {
            DataTable dt = null;
            string w = "";
            if (!string.IsNullOrEmpty(where))
            {
                w = where.Trim().ToLower();
                if (!w.StartsWith("where "))
                    w = "where " + w;
            }
            string sql = "select * from TF_View_PersonalTrain " + w + "order by 结束日期 desc";
            dt = sqlHelper.Query(sql);
            return dt;
        }
    }
}
