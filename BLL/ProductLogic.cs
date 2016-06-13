using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace TopFashion
{
    public class ProductLogic
    {        
        SQLDBHelper sqlHelper;
        static ProductLogic instance;
        public static ProductLogic GetInstance()
        {
            if (instance == null)
                instance = new ProductLogic();

            return instance;
        }

        private ProductLogic()
        {
            sqlHelper = new SQLDBHelper();
        }

        public Product GetProduct(int id)
        {
            string sql = "select * from TF_Product where ID=" + id;
            DataTable dt = sqlHelper.Query(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                Product element = new Product();
                element.ID = id;
                element.品名 = dt.Rows[0]["品名"].ToString();
                element.种类 = ProductTypeLogic.GetInstance().GetProductType(Convert.ToInt32(dt.Rows[0]["种类"]));
                element.单位 = dt.Rows[0]["单位"].ToString();
                element.进价 = Convert.ToDecimal(dt.Rows[0]["进价"]);
                element.售价 = Convert.ToDecimal(dt.Rows[0]["售价"]);
                element.厂家 = dt.Rows[0]["厂家"].ToString();
                element.姓名 = dt.Rows[0]["姓名"].ToString();
                element.电话 = dt.Rows[0]["电话"].ToString();
                element.地址 = dt.Rows[0]["地址"].ToString();
                element.备注 = dt.Rows[0]["备注"].ToString();
                return element;
            }
            return null;
        }

        public List<Product> GetAllProducts()
        {
            List<Product> elements = new List<Product>();
            string sql = "select * from TF_Product";
            DataTable dt = sqlHelper.Query(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Product element = new Product();
                    element.ID = Convert.ToInt32(dt.Rows[i]["ID"]);
                    element.种类 = ProductTypeLogic.GetInstance().GetProductType(Convert.ToInt32(dt.Rows[i]["种类"]));
                    element.品名 = dt.Rows[i]["品名"].ToString();
                    element.单位 = dt.Rows[i]["单位"].ToString();
                    element.进价 = Convert.ToDecimal(dt.Rows[i]["进价"]);
                    element.售价 = Convert.ToDecimal(dt.Rows[i]["售价"]);
                    element.厂家 = dt.Rows[i]["厂家"].ToString();
                    element.姓名 = dt.Rows[i]["姓名"].ToString();
                    element.电话 = dt.Rows[i]["电话"].ToString();
                    element.地址 = dt.Rows[i]["地址"].ToString();
                    element.备注 = dt.Rows[i]["备注"].ToString();
                    elements.Add(element);
                }
            }
            return elements;
        }

        public int AddProduct(Product element)
        {
            string sql = "insert into TF_Product (品名, 种类, 单位, 进价, 售价, 厂家, 姓名, 电话, 地址, 备注) values ('" + element.品名 + "', " + element.种类.ID + ", '" + element.单位 + "', " + element.进价 + ", " + element.售价 + ", '" + element.厂家 + "', '" + element.姓名 + "', '" + element.电话 + "', '" + element.地址 + "', '" + element.备注 + "'); select SCOPE_IDENTITY()";
            object obj = sqlHelper.ExecuteSqlReturn(sql);
            int R;
            if (obj != null && obj != DBNull.Value && int.TryParse(obj.ToString(), out R))
                return R;
            else
                return 0;
        }

        public bool UpdateProduct(Product element)
        {
            string sql = "update TF_Product set 品名='" + element.品名 + "', 种类=" + element.种类.ID + ", 单位='" + element.单位 + "', 进价=" + element.进价 + ", 售价=" + element.售价 + ", 厂家='" + element.厂家 + "', 姓名='" + element.姓名 + "', 电话='" + element.电话 + "', 地址='" + element.地址 + "', 备注='" + element.备注 + "' where ID=" + element.ID;
            int r = sqlHelper.ExecuteSql(sql);
            return r > 0;
        }

        public bool DeleteProduct(Product element)
        {
            string sql = "delete from TF_Product where ID=" + element.ID;
            int r = sqlHelper.ExecuteSql(sql);
            return r > 0;
        }
        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool UpgradeList(List<Product> list)
        {
            int errCount = 0;
            foreach (Product element in list)
            {
                string sqlStr = "if exists (select 1 from TF_Product where ID=" + element.ID + ") update TF_Product set 品名='" + element.品名 + "', 种类=" + element.种类.ID + ", 单位='" + element.单位 + "', 进价=" + element.进价 + ", 售价=" + element.售价 + ", 厂家='" + element.厂家 + "', 姓名='" + element.姓名 + "', 电话='" + element.电话 + "', 地址='" + element.地址 + "', 备注='" + element.备注 + "' where ID=" + element.ID + " else insert into TF_Product (品名, 种类, 单位, 进价, 售价, 厂家, 姓名, 电话, 地址, 备注) values ('" + element.品名 + "', " + element.种类.ID + ", '" + element.单位 + "', " + element.进价 + ", " + element.售价 + ", '" + element.厂家 + "', '" + element.姓名 + "', '" + element.电话 + "', '" + element.地址 + "', '" + element.备注 + "')";
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
            return sqlHelper.Exists("select 1 from TF_Product where 品名='" + name + "'");
        }

        /// <summary>
        /// 是否存在出了自己以外的同名
        /// </summary>
        /// <param name="name"></param>
        /// <param name="myId"></param>
        /// <returns></returns>
        public bool ExistsNameOther(string name, int myId)
        {
            return sqlHelper.Exists("select 1 from TF_Product where ID!=" + myId + " and 品名='" + name + "'");
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
                return sqlHelper.Exists("select 1 from TF_Product " + w);
            }
            return false;
        }

        public DataTable GetProducts(string where)
        {
            DataTable dt = null;
            string w = "";
            if (!string.IsNullOrEmpty(where))
            {
                w = where.Trim().ToLower();
                if (!w.StartsWith("where "))
                    w = "where " + w;
            }
            string sql = "select * from TF_View_Product " + w + " order by 种类 asc";
            dt = sqlHelper.Query(sql);
            return dt;
        }
    }
}
