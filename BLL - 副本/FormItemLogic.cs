using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace TopFashion
{
    public class FormItemLogic
    {        
        SQLDBHelper sqlHelper;
        static FormItemLogic instance;
        public static FormItemLogic GetInstance()
        {
            if (instance == null)
                instance = new FormItemLogic();

            return instance;
        }

        private FormItemLogic()
        {
            sqlHelper = new SQLDBHelper();
        }

        public FormItem GetFormItem(int id)
        {
            string sql = "select * from TF_FormItem where ID=" + id;
            DataTable dt = sqlHelper.Query(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                FormItem element = new FormItem();
                element.ID = id;
                element.ItemName = dt.Rows[0]["ItemName"].ToString();
                element.ItemValue = dt.Rows[0]["ItemValue"].ToString();
                element.ItemType = dt.Rows[0]["ItemType"].ToString();
                element.Flag = Convert.ToInt32(dt.Rows[0]["Flag"]);
                return element;
            }
            return null;
        }

        public List<FormItem> GetAllFormItems()
        {
            List<FormItem> elements = new List<FormItem>();
            string sql = "select * from TF_FormItem";
            DataTable dt = sqlHelper.Query(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    FormItem element = new FormItem();
                    element.ID = Convert.ToInt32(dt.Rows[i]["ID"]);
                    element.ItemName = dt.Rows[i]["ItemName"].ToString();
                    element.ItemValue = dt.Rows[i]["ItemValue"].ToString();
                    element.ItemType = dt.Rows[i]["ItemType"].ToString();
                    element.Flag = Convert.ToInt32(dt.Rows[i]["Flag"]);
                    elements.Add(element);
                }
            }
            return elements;
        }

        public static string GetItemString(List<FormItem> items)
        {
            if (items != null)
            {
                StringBuilder sb = new StringBuilder();
                foreach (FormItem item in items)
                {
                    if (sb.Length == 0)
                        sb.Append(item.ID);
                    else
                        sb.Append("," + item.ID);
                }
                return sb.ToString();
            }
            return "";
        }

        public List<FormItem> GetFormItemsByIds(string items)
        {
            List<FormItem> elements = new List<FormItem>();
            if (!string.IsNullOrEmpty(items))
            {
                string[] ss = items.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                if (ss.Length > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (string s in ss)
                    {
                        string id = s.Trim();
                        int ID;
                        if (int.TryParse(id, out ID))
                        {
                            if (sb.Length == 0)
                                sb.Append(id);
                            else
                                sb.Append("," + id);
                        }
                    }
                    if (sb.Length > 0)
                    {
                        string sql = "select * from TF_FormItem where ID in (" + sb.ToString() + ")";
                        DataTable dt = sqlHelper.Query(sql);
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                FormItem element = new FormItem();
                                element.ID = Convert.ToInt32(dt.Rows[i]["ID"]);
                                element.ItemName = dt.Rows[i]["ItemName"].ToString();
                                element.ItemValue = dt.Rows[i]["ItemValue"].ToString();
                                element.ItemType = dt.Rows[i]["ItemType"].ToString();
                                element.Flag = Convert.ToInt32(dt.Rows[i]["Flag"]);
                                elements.Add(element);
                            }
                        }
                    }
                }
            }
            return elements;
        }

        public int AddFormItem(FormItem element)
        {
            string sql = "insert into TF_FormItem (ItemName, ItemValue, ItemType, Flag) values ('" + element.ItemName + "', '" + element.ItemValue + "', '" + element.ItemType + "', " + element.Flag + "); select SCOPE_IDENTITY()";
            object obj = sqlHelper.ExecuteSqlReturn(sql);
            int R;
            if (obj != null && obj != DBNull.Value && int.TryParse(obj.ToString(), out R))
                return R;
            else
                return 0;
        }

        public bool UpdateFormItem(FormItem element)
        {
            string sql = "update TF_FormItem set ItemName='" + element.ItemName + "', ItemValue='" + element.ItemValue + "', ItemType='" + element.ItemType + "', Flag=" + element.Flag + " where ID=" + element.ID;
            int r = sqlHelper.ExecuteSql(sql);
            return r > 0;
        }

        public bool SetFlag(int id, int flag)
        {
            string sql = "update TF_FormItem set Flag=" + flag + " where ID=" + id;
            int r = sqlHelper.ExecuteSql(sql);
            return r > 0;
        }

        public bool DeleteFormItem(FormItem element)
        {
            string sql = "delete from TF_FormItem where ID=" + element.ID;
            int r = sqlHelper.ExecuteSql(sql);
            return r > 0;
        }

        public bool DeleteFormItems(string ids)
        {
            if (!string.IsNullOrEmpty(ids))
            {
                string sql = "delete from TF_FormItem where ID in (" + ids + ")";
                int r = sqlHelper.ExecuteSql(sql);
                return r > 0;
            }
            return false;
        }
        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool UpgradeList(List<FormItem> list)
        {
            int errCount = 0;
            foreach (FormItem element in list)
            {
                string sqlStr = "if exists (select 1 from TF_FormItem where ID=" + element.ID + ") update TF_FormItem set ItemName='" + element.ItemName + "', ItemValue='" + element.ItemValue + "', ItemType='" + element.ItemType + "', Flag=" + element.Flag + " where ID=" + element.ID + " else insert into TF_FormItem (ItemName, ItemValue, ItemType, Flag) values ('" + element.ItemName + "', '" + element.ItemValue + "', '" + element.ItemType + "', " + element.Flag + ")";
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

        public int SaveFormItem(FormItem element)
        {
            string sqlStr = "";
            if (element.ID > 0)
            {
                sqlStr = "update TF_FormItem set ItemName='" + element.ItemName + "', ItemValue='" + element.ItemValue + "', ItemType='" + element.ItemType + "', Flag=" + element.Flag + " where ID=" + element.ID;
                sqlHelper.ExecuteSql(sqlStr);
                return element.ID;
            }
            else
            {
                sqlStr = "insert into TF_FormItem (ItemName, ItemValue, ItemType, Flag) values ('" + element.ItemName + "', '" + element.ItemValue + "', '" + element.ItemType + "', " + element.Flag + "); select SCOPE_IDENTITY()";
                object obj = sqlHelper.ExecuteSqlReturn(sqlStr);
                int R;
                if (obj != null && obj != DBNull.Value && int.TryParse(obj.ToString(), out R))
                    return R;
                else
                    return 0;
            }
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
                return sqlHelper.Exists("select 1 from TF_FormItem " + w);
            }
            return false;
        }

        public DataTable GetFormItems(string where)
        {
            DataTable dt = null;
            string w = "";
            if (!string.IsNullOrEmpty(where))
            {
                w = where.Trim().ToLower();
                if (!w.StartsWith("where "))
                    w = "where " + w;
            }
            string sql = "select * from TF_FormItem " + w;
            dt = sqlHelper.Query(sql);
            return dt;
        }
    }
}
