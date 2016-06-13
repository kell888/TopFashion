using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace TopFashion
{
    public class FieldMapLogic<T, K>
        where T : class
        where K : class
    {        
        SQLDBHelper sqlHelper;
        static FieldMapLogic<T, K> instance;
        public static FieldMapLogic<T, K> GetInstance()
        {
            if (instance == null)
                instance = new FieldMapLogic<T, K>();

            return instance;
        }

        private FieldMapLogic()
        {
            sqlHelper = new SQLDBHelper();
        }

        public FieldMap<T, K> GetFieldMap(int id)
        {
            string sql = "select * from TF_FieldMap where ID=" + id;
            DataTable dt = sqlHelper.Query(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                FieldMap<T, K> element = new FieldMap<T, K>();
                element.ID = id;
                element.Name = dt.Rows[0]["Name"].ToString();
                element.Map = (byte[])dt.Rows[0]["Map"];
                return element;
            }
            return null;
        }

        public List<FieldMap<T, K>> GetAllFieldMaps()
        {
            List<FieldMap<T, K>> elements = new List<FieldMap<T, K>>();
            string sql = "select * from TF_FieldMap";
            DataTable dt = sqlHelper.Query(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    FieldMap<T, K> element = new FieldMap<T, K>();
                    element.ID = Convert.ToInt32(dt.Rows[i]["ID"]);
                    element.Name = dt.Rows[i]["Name"].ToString();
                    element.Map = (byte[])dt.Rows[i]["Map"];
                    elements.Add(element);
                }
            }
            return elements;
        }

        public int AddFieldMap(FieldMap<T, K> element)
        {
            string sql = "insert into TF_FieldMap (Name, Map) values (@Name, @Map); select SCOPE_IDENTITY()";
            SqlParameter[] para = new SqlParameter[]
            {
                new SqlParameter("@Name", element.Name),
                new SqlParameter("@Map", element.Map)
            };
            object obj = sqlHelper.ExecuteSqlReturn(sql, false, para);
            int R;
            if (obj != null && obj != DBNull.Value && int.TryParse(obj.ToString(), out R))
                return R;
            else
                return 0;
        }

        public bool UpdateFieldMap(FieldMap<T, K> element)
        {
            string sql = "update TF_FieldMap set Name=@Name,Map=@Map where ID=@ID";
            SqlParameter[] para = new SqlParameter[]
            {
                new SqlParameter("@ID", element.ID),
                new SqlParameter("@Name", element.Name),
                new SqlParameter("@Map", element.Map)
            };
            int r = sqlHelper.ExecuteSql(sql, false, para);
            return r > 0;
        }

        public bool DeleteFieldMap(FieldMap<T, K> element)
        {
            string sql = "delete from TF_FieldMap where ID=" + element.ID;
            int r = sqlHelper.ExecuteSql(sql);
            return r > 0;
        }
        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool UpgradeList(List<FieldMap<T, K>> list)
        {
            int errCount = 0;
            foreach (FieldMap<T, K> element in list)
            {
                string sqlStr = "if exists (select 1 from TF_FieldMap where ID=" + element.ID + ") update TF_FieldMap set Name=@Name,Map=@Map where ID=@ID else insert into TF_FieldMap (Name, Map) values (@Name, @Map)";
                try
                {
                    SqlParameter[] para = new SqlParameter[]
                {
                new SqlParameter("@ID", element.ID),
                new SqlParameter("@Name", element.Name),
                new SqlParameter("@Map", element.Map)
                };
                    sqlHelper.ExecuteSql(sqlStr, false, para);
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
            return sqlHelper.Exists("select 1 from TF_FieldMap where Name='" + name + "'");
        }

        /// <summary>
        /// 是否存在出了自己以外的同名
        /// </summary>
        /// <param name="name"></param>
        /// <param name="myId"></param>
        /// <returns></returns>
        public bool ExistsNameOther(string name, int myId)
        {
            return sqlHelper.Exists("select 1 from TF_FieldMap where ID!=" + myId + " and Name='" + name + "'");
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
                return sqlHelper.Exists("select 1 from TF_FieldMap " + w);
            }
            return false;
        }

        public DataTable GetFieldMaps(string where)
        {
            DataTable dt = null;
            string w = "";
            if (!string.IsNullOrEmpty(where))
            {
                w = where.Trim().ToLower();
                if (!w.StartsWith("where "))
                    w = "where " + w;
            }
            string sql = "select * from TF_FieldMap " + w + " order by ID desc";
            dt = sqlHelper.Query(sql);
            return dt;
        }
    }
}
