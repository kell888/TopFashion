using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace TopFashion
{
    public class MoneyRecordLogic
    {        
        SQLDBHelper sqlHelper;
        static MoneyRecordLogic instance;
        public static MoneyRecordLogic GetInstance()
        {
            if (instance == null)
                instance = new MoneyRecordLogic();

            return instance;
        }

        private MoneyRecordLogic()
        {
            sqlHelper = new SQLDBHelper();
        }

        public MoneyRecord GetMoneyRecord(int id)
        {
            string sql = "select * from TF_MoneyRecord where ID=" + id;
            DataTable dt = sqlHelper.Query(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                MoneyRecord element = new MoneyRecord();
                element.ID = id;
                element.会员账户 = MemberMoneyLogic.GetInstance().GetMemberMoney(Convert.ToInt32(dt.Rows[0]["MID"]));
                element.发生金额 = Convert.ToDecimal(dt.Rows[0]["发生金额"]);
                element.是否充值 = Convert.ToBoolean(dt.Rows[0]["是否充值"]);
                element.操作人 = dt.Rows[0]["操作人"].ToString();
                element.发生时间 = Convert.ToDateTime(dt.Rows[0]["发生时间"]);
                return element;
            }
            return null;
        }

        public List<MoneyRecord> GetAllMoneyRecords()
        {
            List<MoneyRecord> elements = new List<MoneyRecord>();
            string sql = "select * from TF_MoneyRecord";
            DataTable dt = sqlHelper.Query(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                MemberMoneyLogic mml = MemberMoneyLogic.GetInstance();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    MoneyRecord element = new MoneyRecord();
                    element.ID = Convert.ToInt32(dt.Rows[i]["ID"]);
                    element.会员账户 = mml.GetMemberMoney(Convert.ToInt32(dt.Rows[i]["MID"]));
                    element.发生金额 = Convert.ToDecimal(dt.Rows[i]["发生金额"]);
                    element.是否充值 = Convert.ToBoolean(dt.Rows[i]["是否充值"]);
                    element.操作人 = dt.Rows[i]["操作人"].ToString();
                    element.发生时间 = Convert.ToDateTime(dt.Rows[i]["发生时间"]);
                    elements.Add(element);
                }
            }
            return elements;
        }

        public List<MoneyRecord> GetMoneyRecordsByMemberName(string name)
        {
            List<MoneyRecord> elements = new List<MoneyRecord>();
            string sql = "select TF_MoneyRecord.* from TF_MoneyRecord,TF_MemberMoney where TF_MoneyRecord.MID=TF_MemberMoney.ID and TF_MemberMoney.会员姓名='" + name + "' order by TF_MoneyRecord.发生时间 desc";
            DataTable dt = sqlHelper.Query(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                MemberMoneyLogic mml = MemberMoneyLogic.GetInstance();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    MoneyRecord element = new MoneyRecord();
                    element.ID = Convert.ToInt32(dt.Rows[i]["ID"]);
                    element.会员账户 = mml.GetMemberMoney(Convert.ToInt32(dt.Rows[i]["MID"]));
                    element.发生金额 = Convert.ToDecimal(dt.Rows[i]["发生金额"]);
                    element.是否充值 = Convert.ToBoolean(dt.Rows[i]["是否充值"]);
                    element.操作人 = dt.Rows[i]["操作人"].ToString();
                    element.发生时间 = Convert.ToDateTime(dt.Rows[i]["发生时间"]);
                    elements.Add(element);
                }
            }
            return elements;
        }

        public List<MoneyRecord> GetMoneyRecordsByMemberMobile(string mobile)
        {
            List<MoneyRecord> elements = new List<MoneyRecord>();
            string sql = "select TF_MoneyRecord.* from TF_MoneyRecord,TF_MemberMoney where TF_MoneyRecord.MID=TF_MemberMoney.ID and TF_MemberMoney.会员电话='" + mobile + "' order by TF_MoneyRecord.发生时间 desc";
            DataTable dt = sqlHelper.Query(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                MemberMoneyLogic mml = MemberMoneyLogic.GetInstance();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    MoneyRecord element = new MoneyRecord();
                    element.ID = Convert.ToInt32(dt.Rows[i]["ID"]);
                    element.会员账户 = mml.GetMemberMoney(Convert.ToInt32(dt.Rows[i]["MID"]));
                    element.发生金额 = Convert.ToDecimal(dt.Rows[i]["发生金额"]);
                    element.是否充值 = Convert.ToBoolean(dt.Rows[i]["是否充值"]);
                    element.操作人 = dt.Rows[i]["操作人"].ToString();
                    element.发生时间 = Convert.ToDateTime(dt.Rows[i]["发生时间"]);
                    elements.Add(element);
                }
            }
            return elements;
        }

        public List<MoneyRecord> GetMoneyRecordsByOperater(string operater)
        {
            List<MoneyRecord> elements = new List<MoneyRecord>();
            string sql = "select * from TF_MoneyRecord where 操作人='" + operater + "' order by 发生时间 desc";
            DataTable dt = sqlHelper.Query(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                MemberMoneyLogic mml = MemberMoneyLogic.GetInstance();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    MoneyRecord element = new MoneyRecord();
                    element.ID = Convert.ToInt32(dt.Rows[i]["ID"]);
                    element.会员账户 = mml.GetMemberMoney(Convert.ToInt32(dt.Rows[i]["MID"]));
                    element.发生金额 = Convert.ToDecimal(dt.Rows[i]["发生金额"]);
                    element.是否充值 = Convert.ToBoolean(dt.Rows[i]["是否充值"]);
                    element.操作人 = dt.Rows[i]["操作人"].ToString();
                    element.发生时间 = Convert.ToDateTime(dt.Rows[i]["发生时间"]);
                    elements.Add(element);
                }
            }
            return elements;
        }

        public List<MoneyRecord> GetMoneyRecordsBy(string where)
        {
            List<MoneyRecord> elements = new List<MoneyRecord>();
            string w = "";
            if (!string.IsNullOrEmpty(where))
            {
                w = where.Trim().ToLower();
                if (!w.StartsWith("where "))
                    w = "where " + w;
            }
            string sql = "select * from TF_MoneyRecord " + w + " order by 发生时间 desc";
            DataTable dt = sqlHelper.Query(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                MemberMoneyLogic mml = MemberMoneyLogic.GetInstance();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    MoneyRecord element = new MoneyRecord();
                    element.ID = Convert.ToInt32(dt.Rows[i]["ID"]);
                    element.会员账户 = mml.GetMemberMoney(Convert.ToInt32(dt.Rows[i]["MID"]));
                    element.发生金额 = Convert.ToDecimal(dt.Rows[i]["发生金额"]);
                    element.是否充值 = Convert.ToBoolean(dt.Rows[i]["是否充值"]);
                    element.操作人 = dt.Rows[i]["操作人"].ToString();
                    element.发生时间 = Convert.ToDateTime(dt.Rows[i]["发生时间"]);
                    elements.Add(element);
                }
            }
            return elements;
        }

        public int AddMoneyRecord(MoneyRecord element)
        {
            if (element.会员账户 == null)
                return 0;
            string sql = "insert into TF_MoneyRecord (MID, 发生金额, 是否充值, 操作人) values (" + element.会员账户.ID + ", " + element.发生金额 + ", " + (element.是否充值 ? "1" : "0") + ", '" + element.操作人 + "'); select SCOPE_IDENTITY()";
            object obj = sqlHelper.ExecuteSqlReturn(sql);
            int R;
            if (obj != null && obj != DBNull.Value && int.TryParse(obj.ToString(), out R))
            {
                decimal rest = element.会员账户.账户余额;
                if (element.是否充值)
                {
                    rest += element.发生金额;
                }
                else
                {
                    rest -= element.发生金额;
                }
                element.会员账户.账户余额 = rest;
                if (MemberMoneyLogic.GetInstance().UpdateMemberMoney(element.会员账户))
                    return R;
                else
                    return 0;
            }
            else
            {
                return 0;
            }
        }

        public bool UpdateMoneyRecord(MoneyRecord element)
        {
            if (element.会员账户 == null)
                return false;
            string sql = "update TF_MoneyRecord set MID=" + element.会员账户.ID + ", 发生金额=" + element.发生金额 + ", 是否充值=" + (element.是否充值 ? "1" : "0") + ", 操作人='" + element.操作人 + "' where ID=" + element.ID;
            int r = sqlHelper.ExecuteSql(sql);
            return r > 0;
        }

        public bool DeleteMoneyRecord(MoneyRecord element)
        {
            string sql = "delete from TF_MoneyRecord where ID=" + element.ID;
            int r = sqlHelper.ExecuteSql(sql);
            return r > 0;
        }
        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool UpgradeList(List<MoneyRecord> list)
        {
            int errCount = 0;
            foreach (MoneyRecord element in list)
            {
                string sqlStr = "if exists (select 1 from TF_MoneyRecord where ID=" + element.ID + ") update TF_MoneyRecord set MID=" + element.会员账户.ID + ", 发生金额=" + element.发生金额 + ", 是否充值=" + (element.是否充值 ? "1" : "0") + ", 操作人='" + element.操作人 + "' where ID=" + element.ID + " else insert into TF_MoneyRecord (MID, 发生金额, 是否充值, 操作人) values (" + element.会员账户.ID + ", " + element.发生金额 + ", " + (element.是否充值 ? "1" : "0") + ", '" + element.操作人 + "')";
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
                return sqlHelper.Exists("select 1 from TF_MoneyRecord " + w);
            }
            return false;
        }

        public DataTable GetMoneyRecords(string where)
        {
            string w = "";
            if (!string.IsNullOrEmpty(where))
            {
                w = where.Trim().ToLower();
                if (!w.StartsWith("where "))
                    w = "where " + w;
            }
            string sql = "select * from TF_View_MoneyRecord " + w + " order by 发生时间 desc";
            DataTable dt = sqlHelper.Query(sql);
            return dt;
        }
    }
}
