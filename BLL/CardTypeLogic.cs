using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace TopFashion
{
    public class CardTypeLogic
    {        
        SQLDBHelper sqlHelper;
        static CardTypeLogic instance;
        public static CardTypeLogic GetInstance()
        {
            if (instance == null)
                instance = new CardTypeLogic();

            return instance;
        }

        private CardTypeLogic()
        {
            sqlHelper = new SQLDBHelper();
        }

        public CardType GetCardType(int id)
        {
            string sql = "select * from TF_CardType where ID=" + id;
            DataTable dt = sqlHelper.Query(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                CardType element = new CardType();
                element.ID = id;
                element.卡种 = dt.Rows[0]["卡种"].ToString();
                element.是否电子芯片 = Convert.ToBoolean(dt.Rows[0]["是否电子芯片"]);
                element.备注 = dt.Rows[0]["备注"].ToString();
                return element;
            }
            return null;
        }

        public CardType GetCardTypeByName(string name)
        {
            string sql = "select * from TF_CardType where 卡种='" + name + "'";
            DataTable dt = sqlHelper.Query(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                CardType element = new CardType();
                element.ID = Convert.ToInt32(dt.Rows[0]["ID"]);
                element.卡种 = dt.Rows[0]["卡种"].ToString();
                element.是否电子芯片 = Convert.ToBoolean(dt.Rows[0]["是否电子芯片"]);
                element.备注 = dt.Rows[0]["备注"].ToString();
                return element;
            }
            return null;
        }

        public List<CardType> GetAllCardTypes()
        {
            List<CardType> elements = new List<CardType>();
            string sql = "select * from TF_CardType";
            DataTable dt = sqlHelper.Query(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    CardType element = new CardType();
                    element.ID = Convert.ToInt32(dt.Rows[i]["ID"]);
                    element.卡种 = dt.Rows[i]["卡种"].ToString();
                    element.是否电子芯片 = Convert.ToBoolean(dt.Rows[i]["是否电子芯片"]);
                    element.备注 = dt.Rows[i]["备注"].ToString();
                    elements.Add(element);
                }
            }
            return elements;
        }

        public int AddCardType(CardType element)
        {
            string sql = "insert into TF_CardType (卡种, 是否电子芯片, 备注) values ('" + element.卡种 + "', " + (element.是否电子芯片 ? "1" : "0") + ", '" + element.备注 + "'); select SCOPE_IDENTITY()";
            object obj = sqlHelper.ExecuteSqlReturn(sql);
            int R;
            if (obj != null && obj != DBNull.Value && int.TryParse(obj.ToString(), out R))
                return R;
            else
                return 0;
        }

        public bool UpdateCardType(CardType element)
        {
            string sql = "update TF_CardType set 卡种='" + element.卡种 + "', 是否电子芯片=" + (element.是否电子芯片 ? "1" : "0") + ", 备注='" + element.备注 + "' where ID=" + element.ID;
            int r = sqlHelper.ExecuteSql(sql);
            return r > 0;
        }

        public bool DeleteCardType(CardType element)
        {
            string sql = "delete from TF_CardType where ID=" + element.ID;
            int r = sqlHelper.ExecuteSql(sql);
            return r > 0;
        }
        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool UpgradeList(List<CardType> list)
        {
            int errCount = 0;
            foreach (CardType element in list)
            {
                string sqlStr = "if exists (select 1 from TF_CardType where ID=" + element.ID + ") update TF_CardType set 卡种='" + element.卡种 + "', 是否电子芯片=" + (element.是否电子芯片 ? "1" : "0") + ", 备注='" + element.备注 + "' where ID=" + element.ID + " else insert into TF_CardType (卡种, 是否电子芯片, 备注) values ('" + element.卡种 + "', " + (element.是否电子芯片 ? "1" : "0") + ", '" + element.备注 + "')";
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
            return sqlHelper.Exists("select 1 from TF_CardType where 卡种='" + name + "'");
        }

        /// <summary>
        /// 是否存在出了自己以外的同名
        /// </summary>
        /// <param name="name"></param>
        /// <param name="myId"></param>
        /// <returns></returns>
        public bool ExistsNameOther(string name, int myId)
        {
            return sqlHelper.Exists("select 1 from TF_CardType where ID!=" + myId + " and 卡种='" + name + "'");
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
                return sqlHelper.Exists("select 1 from TF_CardType " + w);
            }
            return false;
        }

        public DataTable GetCardTypes(string where)
        {
            DataTable dt = null;
            string w = "";
            if (!string.IsNullOrEmpty(where))
            {
                w = where.Trim().ToLower();
                if (!w.StartsWith("where "))
                    w = "where " + w;
            }
            string sql = "select * from TF_CardType " + w;
            dt = sqlHelper.Query(sql);
            return dt;
        }
    }
}
