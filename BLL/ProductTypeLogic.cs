using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace TopFashion
{
    public class ProductTypeLogic
    {        
        SQLDBHelper sqlHelper;
        static ProductTypeLogic instance;
        public static ProductTypeLogic GetInstance()
        {
            if (instance == null)
                instance = new ProductTypeLogic();

            return instance;
        }

        private ProductTypeLogic()
        {
            sqlHelper = new SQLDBHelper();
        }

        public ProductType GetProductType(int id)
        {
            string sql = "select * from TF_ProductType where ID=" + id;
            DataTable dt = sqlHelper.Query(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                ProductType element = new ProductType();
                element.ID = id;
                element.类型 = dt.Rows[0]["类型"].ToString();
                element.Flag = Convert.ToBoolean(dt.Rows[0]["Flag"]);
                element.备注 = dt.Rows[0]["备注"].ToString();
                return element;
            }
            return null;
        }

        public List<ProductType> GetAllProductTypes()
        {
            List<ProductType> elements = new List<ProductType>();
            string sql = "select * from TF_ProductType";
            DataTable dt = sqlHelper.Query(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ProductType element = new ProductType();
                    element.ID = Convert.ToInt32(dt.Rows[i]["ID"]);
                    element.类型 = dt.Rows[i]["类型"].ToString();
                    element.Flag = Convert.ToBoolean(dt.Rows[i]["Flag"]);
                    element.备注 = dt.Rows[i]["备注"].ToString();
                    elements.Add(element);
                }
            }
            return elements;
        }

        public int AddProductType(ProductType element)
        {
            string sql = "insert into TF_ProductType (类型, Flag, 备注) values ('" + element.类型 + "', " + (element.Flag ? "1" : "0") + ", '" + element.备注 + "'); select SCOPE_IDENTITY()";
            object obj = sqlHelper.ExecuteSqlReturn(sql);
            int R;
            if (obj != null && obj != DBNull.Value && int.TryParse(obj.ToString(), out R))
                return R;
            else
                return 0;
        }

        public bool UpdateProductType(ProductType element)
        {
            string sql = "update TF_ProductType set 类型='" + element.类型 + "', Flag=" + (element.Flag ? "1" : "0") + ", 备注='" + element.备注 + "' where ID=" + element.ID;
            int r = sqlHelper.ExecuteSql(sql);
            return r > 0;
        }

        public bool DeleteProductType(ProductType element)
        {
            string sql = "delete from TF_ProductType where ID=" + element.ID;
            int r = sqlHelper.ExecuteSql(sql);
            return r > 0;
        }
        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool UpgradeList(List<ProductType> list)
        {
            int errCount = 0;
            foreach (ProductType element in list)
            {
                string sqlStr = "if exists (select 1 from TF_ProductType where ID=" + element.ID + ") update TF_ProductType set 类型='" + element.类型 + "', Flag=" + (element.Flag ? "1" : "0") + ", 备注='" + element.备注 + "' where ID=" + element.ID + " else insert into TF_ProductType (类型, Flag, 备注) values ('" + element.类型 + "', " + (element.Flag ? "1" : "0") + ", '" + element.备注 + "')";
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
            return sqlHelper.Exists("select 1 from TF_ProductType where 类型='" + name + "'");
        }

        /// <summary>
        /// 是否存在出了自己以外的同名
        /// </summary>
        /// <param name="name"></param>
        /// <param name="myId"></param>
        /// <returns></returns>
        public bool ExistsNameOther(string name, int myId)
        {
            return sqlHelper.Exists("select 1 from TF_ProductType where ID!=" + myId + " and 类型='" + name + "'");
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
                return sqlHelper.Exists("select 1 from TF_ProductType " + w);
            }
            return false;
        }

        public DataTable GetProductTypes(string where)
        {
            DataTable dt = null;
            string w = "";
            if (!string.IsNullOrEmpty(where))
            {
                w = where.Trim().ToLower();
                if (!w.StartsWith("where "))
                    w = "where " + w;
            }
            string sql = "select * from TF_ProductType " + w;
            dt = sqlHelper.Query(sql);
            return dt;
        }
    }
}
