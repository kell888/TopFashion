using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace TopFashion
{
    public class PropertyLogic
    {        
        SQLDBHelper sqlHelper;
        static PropertyLogic instance;
        public static PropertyLogic GetInstance()
        {
            if (instance == null)
                instance = new PropertyLogic();

            return instance;
        }

        private PropertyLogic()
        {
            sqlHelper = new SQLDBHelper();
        }

        public Property GetProperty(int id)
        {
            string sql = "select * from TF_Property where ID=" + id;
            DataTable dt = sqlHelper.Query(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                Property property = new Property();
                property.ID = id;
                property.名称 = dt.Rows[0]["名称"].ToString();
                property.单位 = dt.Rows[0]["单位"].ToString();
                property.用途 = dt.Rows[0]["用途"].ToString();
                property.价格 = Convert.ToDecimal(dt.Rows[0]["价格"]);
                property.备注 = dt.Rows[0]["备注"].ToString();
                return property;
            }
            return null;
        }

        public List<Property> GetAllPropertys()
        {
            List<Property> propertys = new List<Property>();
            string sql = "select * from TF_Property";
            DataTable dt = sqlHelper.Query(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Property property = new Property();
                    property.ID = Convert.ToInt32(dt.Rows[i]["ID"]);
                    property.名称 = dt.Rows[i]["名称"].ToString();
                    property.单位 = dt.Rows[i]["单位"].ToString();
                    property.用途 = dt.Rows[i]["用途"].ToString();
                    property.价格 = Convert.ToDecimal(dt.Rows[i]["价格"]);
                    property.备注 = dt.Rows[i]["备注"].ToString();
                    propertys.Add(property);
                }
            }
            return propertys;
        }

        public int AddProperty(Property element)
        {
            string sql = "insert into TF_Property (名称, 单位, 用途, 价格, 备注) values ('" + element.名称 + "', '" + element.单位 + "', '" + element.用途 + "', " + element.价格 + ", '" + element.备注 + "'); select SCOPE_IDENTITY()";
            object obj = sqlHelper.ExecuteSqlReturn(sql);
            int R;
            if (obj != null && obj != DBNull.Value && int.TryParse(obj.ToString(), out R))
                return R;
            else
                return 0;
        }

        public bool UpdateProperty(Property element)
        {
            string sql = "update TF_Property set 名称='" + element.名称 + "', 单位='" + element.单位 + "', 用途='" + element.用途 + "', 价格=" + element.价格 + ", 备注='" + element.备注 + "' where ID=" + element.ID;
            int r = sqlHelper.ExecuteSql(sql);
            return r > 0;
        }

        public bool DeleteProperty(Property element)
        {
            string sql = "delete from TF_Action where ID=" + element.ID;
            int r = sqlHelper.ExecuteSql(sql);
            return r > 0;
        }
        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool UpgradeList(List<Property> list)
        {
            int errCount = 0;
            foreach (Property element in list)
            {
                string sqlStr = "if exists (select 1 from TF_Property where ID=" + element.ID + ") update TF_Property set 名称='" + element.名称 + "', 单位='" + element.单位 + "', 用途='" + element.用途 + "', 价格=" + element.价格 + ", 备注='" + element.备注 + "' where ID=" + element.ID + " else insert into TF_Property (名称, 单位, 用途, 价格, 备注) values ('" + element.名称 + "', '" + element.单位 + "', '" + element.用途 + "', " + element.价格 + ", '" + element.备注 + "')";
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
            return sqlHelper.Exists("select 1 from TF_Property where 名称='" + name + "'");
        }

        /// <summary>
        /// 是否存在出了自己以外的同名
        /// </summary>
        /// <param name="name"></param>
        /// <param name="myId"></param>
        /// <returns></returns>
        public bool ExistsNameOther(string name, int myId)
        {
            return sqlHelper.Exists("select 1 from TF_Property where ID!=" + myId + " and 名称='" + name + "'");
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
                return sqlHelper.Exists("select 1 from TF_Property " + w);
            }
            return false;
        }

        public DataTable GetPropertys(string where)
        {
            DataTable dt = null;
            string w = "";
            if (!string.IsNullOrEmpty(where))
            {
                w = where.Trim().ToLower();
                if (!w.StartsWith("where "))
                    w = "where " + w;
            }
            string sql = "select * from TF_Property " + w;
            dt = sqlHelper.Query(sql);
            return dt;
        }
    }
}
